using System.Runtime.InteropServices;

namespace SqlLib.SqlTool
{
    /// <summary>
    /// Sql 全域參數
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct SqlSetting
    {
        /// <summary>
        /// 連線參數 1
        /// </summary>
        public static string StrConnection1 = string.Empty;
        /// <summary>
        /// 連線參數 2
        /// </summary>
        public static string StrConnection2 = string.Empty;
        /// <summary>
        /// 連線參數 3
        /// </summary>
        public static string StrConnection3 = string.Empty;
        /// <summary>
        /// 資料庫名稱 1
        /// </summary>
        public static string DbName1 = string.Empty;
        /// <summary>
        /// 資料庫名稱 2
        /// </summary>
        public static string DbName2 = string.Empty;
        /// <summary>
        /// 資料庫名稱 3
        /// </summary>
        public static string DbName3 = string.Empty;
        /// <summary>
        /// 資料庫IP 1
        /// </summary>
        public static string DbIp1 = string.Empty;
        /// <summary>
        /// 資料庫IP 2
        /// </summary>
        public static string DbIp2 = string.Empty;
        /// <summary>
        /// 資料庫IP 3
        /// </summary>
        public static string DbIp3 = string.Empty;
    }
}
