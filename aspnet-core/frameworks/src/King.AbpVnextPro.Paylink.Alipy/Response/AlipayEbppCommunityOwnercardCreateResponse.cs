﻿using System.Text.Json.Serialization;

namespace King.AbpVnextPro.Paylink.Alipy.Response
{
    /// <summary>
    /// AlipayEbppCommunityOwnercardCreateResponse.
    /// </summary>
    public class AlipayEbppCommunityOwnercardCreateResponse : AlipayResponse
    {
        /// <summary>
        /// 支付宝证件id
        /// </summary>
        [JsonPropertyName("alipay_card_id")]
        public string AlipayCardId { get; set; }
    }
}
