﻿using System.Text.Json.Serialization;

namespace King.AbpVnextPro.Paylink.Alipy.Domain
{
    /// <summary>
    /// MybankCreditLoanapplyLocallifeBlacklistQueryModel Data Structure.
    /// </summary>
    public class MybankCreditLoanapplyLocallifeBlacklistQueryModel : AlipayObject
    {
        /// <summary>
        /// 站点会员
        /// </summary>
        [JsonPropertyName("site")]
        public string Site { get; set; }

        /// <summary>
        /// 站点的会员ID
        /// </summary>
        [JsonPropertyName("site_user_id")]
        public string SiteUserId { get; set; }
    }
}
