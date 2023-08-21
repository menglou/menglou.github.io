﻿using King.AbpVnextPro.Openiddict.Logs.SecurityLogs;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace King.AbpVnextPro.Openiddict.Logs
{
    [RemoteService(Name = SecurityLogRemoteServiceConsts.RemoteServiceName)]
    [ControllerName("SecurityLog")]
    [Area("securitylog")]
    [Route("/api/security-log/security-logs")]
    public class SecurityLogController : OpeniddictController, ISecurityLogAppService
    {
        protected ISecurityLogAppService _securityLogAppService { get; }
        public SecurityLogController(ISecurityLogAppService securityLogAppService)
        {
            _securityLogAppService = securityLogAppService;
        }
        [HttpGet]
        [Route("{id}")]
        public virtual Task<SecurityLogDto> GetAsync(Guid id)
        {
            return _securityLogAppService.GetAsync(id);
        }
        [HttpGet]
        public virtual Task<PagedResultDto<SecurityLogDto>> GetListAsync(GetSecurityLogDto input)
        {
            return _securityLogAppService.GetListAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _securityLogAppService.DeleteAsync(id);
        }

        [HttpDelete]
        [Route("delete-many")]
        public virtual Task DeleteManyAsync(Guid[] ids)
        {
            return _securityLogAppService.DeleteManyAsync(ids);
        }
    }
}
