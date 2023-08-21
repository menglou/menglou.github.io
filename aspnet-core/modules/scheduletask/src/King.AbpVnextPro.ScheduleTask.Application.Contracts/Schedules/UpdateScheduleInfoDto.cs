﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace King.AbpVnextPro.ScheduleTask.Schedules
{
    public class UpdateScheduleInfoDto : EntityDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 任务分组
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>  
        public JobTypeEnum JobType { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否周期运行
        /// </summary>
        public bool RunLoop { get; set; }

        /// <summary>
        /// cron表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 任务所在程序集名称
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 执行类名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public ScheduleStatus Status { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime? StartDate
        {
            get
            {
                return !string.IsNullOrEmpty(StartDateStr) ? Convert.ToDateTime(StartDateStr) : null;
            }
            set
            {
                value = StartDate;
            }
        }

        public string StartDateStr { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                return !string.IsNullOrEmpty(EndDateStr) ? Convert.ToDateTime(EndDateStr) : null;
            }
            set
            {
                value = EndDate;
            }
        }


        public string EndDateStr { get; set; }
        /// <summary>
        /// 是否有重试
        /// </summary>
        public bool IsHaveRetry { get; set; }

        /// <summary>
        /// 最大重试次数
        /// </summary>
        public int? MaxRetryCount { get; set; }

        /// <summary>
        /// 重试间隔 单位秒
        /// </summary>
        public int? RetryInterval { get; set; }



        /// <summary>
        /// 是否同意发送邮件
        /// </summary>
        public bool IsAllowMail { get; set; }
        /// <summary>
        /// 是否同意发送短信
        /// </summary>
        public bool IsAllowSms { get; set; }
        /// <summary>
        /// 是否同意signarl
        /// </summary>
        public bool IsAllowSignarl { get; set; }

        public List<KeepersInfo> KeeperInfo { get; set; }


        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 数据格式
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 自定义请求头（json格式）
        /// </summary>
        public string Headers { get; set; }

        /// <summary>
        /// 数据内容（json格式）
        /// </summary>
        public string Body { get; set; }


        public UpdateScheduleInfoDto()
        {
            KeeperInfo = new List<KeepersInfo>();
        }
    }
}
