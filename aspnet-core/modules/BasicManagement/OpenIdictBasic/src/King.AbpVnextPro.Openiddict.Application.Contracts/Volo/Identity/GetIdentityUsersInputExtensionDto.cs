﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;

namespace King.AbpVnextPro.Openiddict.Volo.Identity
{
    public class GetIdentityUsersInputExtensionDto : GetIdentityUsersInput
    {
        public bool includeChildren { get; set; } = false;
    }
}
