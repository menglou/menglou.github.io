﻿using System.Text.Json.Serialization;

namespace King.AbpVnextPro.Paylink.Alipy.Domain
{
    /// <summary>
    /// AlipayOpenMiniAmpeDevicemodelBatchqueryModel Data Structure.
    /// </summary>
    public class AlipayOpenMiniAmpeDevicemodelBatchqueryModel : AlipayObject
    {
        /// <summary>
        /// 产品ID，必填
        /// </summary>
        [JsonPropertyName("product_id")]
        public long ProductId { get; set; }

        /// <summary>
        /// 场景码，申请后平台分配，必填
        /// </summary>
        [JsonPropertyName("scene_code")]
        public string SceneCode { get; set; }
    }
}
