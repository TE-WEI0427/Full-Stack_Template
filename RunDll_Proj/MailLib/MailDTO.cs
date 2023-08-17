namespace MailLib
{
    /// <summary>
    /// 郵件 數據傳輸模組
    /// </summary>
    public class MailDTO
    {
        /// <summary>
        /// 接收郵件端帳戶
        /// </summary>
        public string To { get; set; } = string.Empty;
        /// <summary>
        /// 郵件主旨
        /// </summary>
        public string Subject { get; set; } = string.Empty;
        /// <summary>
        /// 郵件內容 可輸入 html 文本內容
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }
}
