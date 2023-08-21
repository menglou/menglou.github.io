﻿using King.AbpVnextPro.IdentityServer.Features;
using King.AbpVnextPro.IdentityServer.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
using static King.AbpVnextPro.IdentityServer.Permissions.IdentityServerPermissions;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace King.AbpVnextPro.IdentityServer.Volo.Identity
{
    [RemoteService(IsEnabled = false)]
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IIdentityUserAppService),
      typeof(IdentityUserAppService),
      typeof(IBasicIdentityUserAppService),
      typeof(BasicIdentityUserAppService))]
    public class BasicIdentityUserAppService:IdentityUserAppService, IBasicIdentityUserAppService
    {
        private readonly IStringLocalizer<IdentityServerResource> _localizer;
        protected IOrganizationUnitRepository _organizationUnitRepository { get; }
        protected IdentityUserStore _store { get; set; }
        public BasicIdentityUserAppService(IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IOptions<IdentityOptions> identityOptions,
            IOrganizationUnitRepository OrganizationUnitRepository,
            IdentityUserStore store,
            IStringLocalizer<IdentityServerResource> localizer) : base(userManager, userRepository, roleRepository, identityOptions)
        {
            _localizer = localizer;
            _organizationUnitRepository = OrganizationUnitRepository;
            _store = store;
        }

        public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            var userCount = (await FeatureChecker.GetOrNullAsync(BasicManagementFeatures.UserCount)).To<int>();
            var currentUserCount = await UserRepository.GetCountAsync();
            if (currentUserCount >= userCount)
            {
                throw new UserFriendlyException(_localizer["Feature:UserCount.Maximum", userCount]);
            }

            return await base.CreateAsync(input);
        }

        [Authorize(IdentityPermissions.Users.Create)]
        [Authorize(BasicIdentityPermissions.Users.DistributionOrganizationUnit)]
        public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserOrgCreateDto input)
        {
            var identity = await CreateAsync(
                ObjectMapper.Map<IdentityUserOrgCreateDto, IdentityUserCreateDto>(input)
            );
            if (input.OrgIds != null)
            {
                await UserManager.SetOrganizationUnitsAsync(identity.Id, input.OrgIds.ToArray());
            }
            return identity;
        }

        [Authorize(BasicIdentityPermissions.Users.DistributionOrganizationUnit)]
        public virtual async Task AddToOrganizationUnitsAsync(Guid userId, List<Guid> ouIds)
        {
            await UserManager.SetOrganizationUnitsAsync(userId, ouIds.ToArray());
        }

        [Authorize(BasicIdentityPermissions.Users.DistributionOrganizationUnit)]
        public virtual async Task<bool> BatchAddToOrganizationUnitsAsync(BatchUseToOrganizationUnitCreationDto input)
        {
            foreach (var item in input.UserId)
            {
                IdentityUser user = await UserRepository.FindAsync(item);

                //根据这个组织获取这个组织下的所有组织
                OrganizationUnit ou = await _organizationUnitRepository.FindAsync(input.OrgId);

                var orglist = await _organizationUnitRepository.GetListAsync();
                List<Guid> guilist = orglist.Where(x => x.Code.StartsWith(ou.Code)).Select(x => x.Id).ToList();

                //判断组织有没有
                await UserRepository.EnsureCollectionLoadedAsync(user, u => u.OrganizationUnits);


                if (user.OrganizationUnits.Any(cou => cou.OrganizationUnitId == ou.Id))
                {
                    return false;
                }

                await CheckMaxUserOrganizationUnitMembershipCountAsync(user.OrganizationUnits.Count + 1);

                foreach (var orgguid in guilist)
                {
                    user.AddOrganizationUnit(orgguid);
                    await UserRepository.UpdateAsync(user);
                }
            }
            return true;
        }

        public virtual async Task<ListResultDto<OrganizationUnitDto>> GetListOrganizationUnitsAsync(Guid id, bool includeDetails = false)
        {
            var list = await UserRepository.GetOrganizationUnitsAsync(id, includeDetails);
            return new ListResultDto<OrganizationUnitDto>(
                ObjectMapper.Map<List<OrganizationUnit>, List<OrganizationUnitDto>>(list)
            );
        }

        [Authorize(IdentityPermissions.Users.Update)]
        [Authorize(BasicIdentityPermissions.Users.DistributionOrganizationUnit)]
        public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserOrgUpdateDto input)
        {
            var update = ObjectMapper.Map<IdentityUserOrgUpdateDto, IdentityUserUpdateDto>(input);
            var result = await base.UpdateAsync(id, update);
            await UserManager.SetOrganizationUnitsAsync(result.Id, input.OrgIds.ToArray());
            return result;
        }

       
        /// <summary>
        /// 移除该组织下所有的用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ouId"></param>
        /// <returns></returns>
        [Authorize(BasicIdentityPermissions.Users.DistributionOrganizationUnit)]
        public virtual async Task<bool> RemoveFromOrganizationUnitAsync(UseToOrganizationUnitDeleteDto input)
        {
            var user = await UserRepository.GetAsync(input.UserId);
            user.RemoveOrganizationUnit(input.OrgId);
            await UserRepository.UpdateAsync(user);
            return true;
        }

        protected async Task CheckMaxUserOrganizationUnitMembershipCountAsync(int requestedCount)
        {
            var maxCount =
                await SettingProvider.GetAsync<int>(IdentitySettingNames.OrganizationUnit.MaxUserMembershipCount);
            if (requestedCount > maxCount)
            {
                throw new BusinessException(IdentityErrorCodes.MaxAllowedOuMembership)
                    .WithData("MaxUserMembershipCount", maxCount);
            }
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateUseLockAsync(Guid id)
        {
            var res = await UserRepository.FindAsync(id);
            if (res == null)
            {
                throw new UserFriendlyException($"该用户不存在，请检查");
            }
            if (res.LockoutEnd == null)
            {
                DateTimeOffset dateTimeOffset = new DateTimeOffset(DateTime.Now.AddHours(1));
                await _store.SetLockoutEndDateAsync(res, dateTimeOffset);
            }
            else
            {
                await _store.SetLockoutEndDateAsync(res, null);
            }
            return true;
        }

        public virtual async Task<List<IdentityUserDto>> GetListAllAsync()
        {
            var users = await UserRepository.GetListAsync();

            return ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(users);
        }

    }
}
