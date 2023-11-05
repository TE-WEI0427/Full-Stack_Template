namespace Models.MDAppApi
{
    public class MDSys
    {
        /// <summary>
        /// App 使用者 註冊
        /// </summary>
        public class Register
        {
            /// <summary>
            /// 使用者帳號
            /// </summary>
            public string UserId { get; set; } = string.Empty;
            /// <summary>
            /// 使用者密碼
            /// </summary>
            public string Password { get; set; } = string.Empty;
            /// <summary>
            /// 使用者電子郵件
            /// </summary>
            public string Email { get; set; } = string.Empty;
        }
    }
}
