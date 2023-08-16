namespace BasicConfig
{
    /// <summary>
    /// App 呼叫 API 開始/結束
    /// </summary>
    public enum WebApiLogActType
    {
        /// <summary>
        /// 開始
        /// </summary>
        Start = 10,
        /// <summary>
        /// 結束
        /// </summary>
        End = 20
    }

    /// <summary>
    /// 回傳代碼
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 失敗: 90
        /// </summary>
        Failed = 90,
        /// <summary>
        /// 成功: 10
        /// </summary>
        Success = 10,
        /// <summary>
        /// 查無資料: 20
        /// </summary>
        NotFound = 20,
        /// <summary>
        /// 其他用途: 30
        /// </summary>
        Other = 30,
        /// <summary>
        /// Exception: 99
        /// </summary>
        Exception = 99
    }

    /// <summary>
    /// 回傳資訊
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 回傳代碼
        /// </summary>
        public ResultCode ResultCode { get; set; } = ResultCode.Success;
        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;
        /// <summary>
        /// 回傳值
        /// </summary>
        public dynamic Data { get; set; } = string.Empty;
        /// <summary>
        /// 集合值
        /// </summary>
        public IEnumerable<dynamic> Source { get; set; } = Enumerable.Empty<dynamic>();
        /// <summary>
        /// 建構子
        /// </summary>
        public Result() { }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="source"></param>
        public Result(IEnumerable<dynamic> source)
        {
            if (source == null)
            {
                this.ResultCode = ResultCode.NotFound;
                this.Message = "無查詢資料";
            }
            else
            {
                this.ResultCode = ResultCode.Success;
                this.Source = source;
            }
        }
        /// <summary>
        /// 回傳模組
        /// </summary>
        /// <param name="ResultCode">ResultCode </param>
        /// <param name="message">回傳訊息</param>
        public Result(ResultCode ResultCode, string message)
        {
            this.ResultCode = ResultCode;
            this.Message = message;
        }
    }
}
