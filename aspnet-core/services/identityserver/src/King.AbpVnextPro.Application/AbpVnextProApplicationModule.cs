﻿using EasyAbp.Abp.SettingUi;
using King.AbpVnextPro.Dictionary;
using King.AbpVnextPro.File;
using King.AbpVnextPro.IdentityServer;
using King.AbpVnextPro.Institution;
using King.AbpVnextPro.Notice;
using King.AbpVnextPro.PayCenter;
using King.AbpVnextPro.ScheduleTask;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace King.AbpVnextPro;

[DependsOn(
    typeof(IdentityServerApplicationModule),
    typeof(AbpVnextProDomainModule),
    typeof(AbpVnextProApplicationContractsModule),
    typeof(NoticeApplicationModule),
    typeof(DictionaryApplicationModule),
    typeof(FileApplicationModule),
    typeof(AbpSettingUiApplicationModule),
    typeof(ScheduleTaskApplicationModule),
    typeof(PayCenterApplicationModule),
    typeof(InstitutionApplicationModule)
    )]
public class AbpVnextProApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpVnextProApplicationModule>();
        });
    }
}
