﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;

namespace King.AbpVnextPro.IdentityServer.Volo.Identity
{
    public class IdentityUserOrgUpdateDto : IdentityUserUpdateDto
    {
        public List<Guid> OrgIds { get; set; }
    }
}
