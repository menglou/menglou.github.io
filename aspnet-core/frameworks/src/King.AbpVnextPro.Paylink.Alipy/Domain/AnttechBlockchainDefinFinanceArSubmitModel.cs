﻿using System.Text.Json.Serialization;

namespace King.AbpVnextPro.Paylink.Alipy.Domain
{
    /// <summary>
    /// AnttechBlockchainDefinFinanceArSubmitModel Data Structure.
    /// </summary>
    public class AnttechBlockchainDefinFinanceArSubmitModel : AlipayObject
    {
        /// <summary>
        /// base64编码的业务参数
        /// </summary>
        [JsonPropertyName("encoded_biz_param")]
        public string EncodedBizParam { get; set; }

        /// <summary>
        /// 系统参数
        /// </summary>
        [JsonPropertyName("sys_param")]
        public SystemParam SysParam { get; set; }
    }
}
