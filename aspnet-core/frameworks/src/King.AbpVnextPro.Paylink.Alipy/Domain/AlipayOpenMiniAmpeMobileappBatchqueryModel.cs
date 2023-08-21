﻿using System.Text.Json.Serialization;

namespace King.AbpVnextPro.Paylink.Alipy.Domain
{
    /// <summary>
    /// AlipayOpenMiniAmpeMobileappBatchqueryModel Data Structure.
    /// </summary>
    public class AlipayOpenMiniAmpeMobileappBatchqueryModel : AlipayObject
    {
        /// <summary>
        /// 场景码，申请后平台分配
        /// </summary>
        [JsonPropertyName("scene_code")]
        public string SceneCode { get; set; }
    }
}
