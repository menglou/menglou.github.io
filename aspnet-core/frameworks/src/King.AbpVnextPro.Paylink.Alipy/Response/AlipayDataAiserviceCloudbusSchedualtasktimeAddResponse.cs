﻿using System.Text.Json.Serialization;
using King.AbpVnextPro.Paylink.Alipy.Domain;

namespace King.AbpVnextPro.Paylink.Alipy.Response
{
    /// <summary>
    /// AlipayDataAiserviceCloudbusSchedualtasktimeAddResponse.
    /// </summary>
    public class AlipayDataAiserviceCloudbusSchedualtasktimeAddResponse : AlipayResponse
    {
        /// <summary>
        /// 结果
        /// </summary>
        [JsonPropertyName("result")]
        public CloudbusCommonResult Result { get; set; }
    }
}
