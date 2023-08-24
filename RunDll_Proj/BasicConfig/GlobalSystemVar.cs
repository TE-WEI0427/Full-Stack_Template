using System.Runtime.InteropServices;

namespace BasicConfig
{
    /// <summary>
    /// 全域參數
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct GlobalSystemVar
    {
        /// <summary>
        /// 網站子路徑
        /// </summary>
        public static string ServerSubPath = string.Empty;
    }
}
