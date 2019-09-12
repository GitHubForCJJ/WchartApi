using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdev.Log
{
    /// <summary>
    /// 系统日志等级，按照Syslog的标准，将日志等级分为八个等级
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Emergency
        /// </summary>
        A系统不可用 = 0,
        /// <summary>
        /// Alert
        /// </summary>
        B紧急事件 = 1,
        /// <summary>
        /// Critical
        /// </summary>
        C关键事件 = 2,
        /// <summary>
        /// Error
        /// </summary>
        D错误事件 = 3,
        /// <summary>
        /// Warning
        /// </summary>
        E警告事件 = 4,
        /// <summary>
        /// Notice
        /// </summary>
        F重要事件 = 5,
        /// <summary>
        /// Information
        /// </summary>
        G有用信息 = 6,
        /// <summary>
        /// Debug
        /// </summary>
        H调试信息 = 7
    }
}
