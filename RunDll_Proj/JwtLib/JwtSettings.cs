namespace JwtLib
{
    public struct JwtSettings
    {
        /// <summary>
        /// 發行人
        /// </summary>
        public static string Issuer { get; set; } = "AndyHsu";
        /// <summary>
        /// 接收者
        /// </summary>
        public static string Audience { get; set; } = "WebAppAudience";
        /// <summary>
        /// 密鑰
        /// </summary>
        public static string SecretKey { get; set; } = "Z4T9mdppztWKMsN9+Z6OHoopZOmjmvtDvTzuFkykKRo=";
    }
}
