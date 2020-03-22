using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace JST.TPLMS.Util
{
    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Sussess { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// 表示业务操作结果的枚举
        /// </summary>
        public OperationResultType ResultType { get; set; }

        public string Id { get; set; }

        public string No { get; set; }
    }
}
