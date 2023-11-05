namespace BasicConfig
{
    /// <summary>
    /// 預設 使用者資料
    /// </summary>
    public class DefalutUserData
    {
        /// <summary>
        /// 使用者 系統代碼
        /// </summary>
        public int SysUserId { get; set; } = 0;
        /// <summary>
        /// 使用者角色
        /// </summary>
        public string Role { get; set; } = string.Empty;
        /// <summary>
        /// 備註
        /// </summary>
        public string memo { get; set; } = string.Empty;
    }

    /// <summary>
    /// App 使用者資料
    /// </summary>
    public class AppUserData
    {
        /// <summary>
        /// 使用者 系統代碼
        /// </summary>
        public int SysUserId { get; set; } = 0;
        /// <summary>
        /// Device 平台
        /// </summary>
        public string Platform { get; set; } = string.Empty;
        /// <summary>
        /// Device UUID
        /// </summary>
        public string UUID { get; set; } = string.Empty;
        /// <summary>
        /// Memo
        /// </summary>
        public string Memo { get; set; } = string.Empty;
        /// <summary>
        /// ClientIp
        /// </summary>
        public string ClientIp { get; set; } = string.Empty;
    }
}
