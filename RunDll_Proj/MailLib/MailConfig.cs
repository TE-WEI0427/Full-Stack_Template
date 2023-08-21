namespace MailLib
{
    /// <summary>
    /// 郵件參數設定
    /// </summary>
    public struct MailConfig
    {
        /// <summary>
        /// 郵主機
        /// </summary>
        public static string Host { get; set; } = "smtp.ethereal.email";
        /// <summary>
        /// 郵件主機端口
        /// </summary>
        public static int Port { get; set; } = 587;
        /// <summary>
        /// 發送郵件端帳戶
        /// </summary>
        public static string MailAccount { get; set; } = "hazel.altenwerth@ethereal.email";
        /// <summary>
        /// 發送郵件端密碼 若為兩步驟驗證帳戶須設定為應用程式密碼
        /// </summary>
        public static string MailPassword { get; set; } = "B914WdUdyaBDxhanWc";
        /// <summary>
        /// 是否開啟驗證
        /// </summary>
        public static bool EnableSsl { get; set; } = false;
        /// <summary>
        /// 指定 SSL 或 TLS 加密的方法，
        /// 枚舉定義參考: http://www.mimekit.net/docs/html/T_MailKit_Security_SecureSocketOptions.htm
        /// </summary>
        public static int SecureSocketOptions { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        //public static string MailFrom { get; set; } = string.Empty;
        /// <summary>
        /// 發送郵件人的顯示名稱
        /// </summary>
        public static string MailDisplayName { get; set; } = "測試用信箱";
    }
}