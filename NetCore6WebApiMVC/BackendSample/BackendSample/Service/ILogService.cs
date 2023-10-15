using BasicConfig;

namespace Service
{
    /// <summary>
    /// 寫入紀錄 功能介面
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// 開始呼叫紀錄 POST
        /// </summary>
        /// <param name="input">輸入參數</param>
        /// <param name="userData">用戶端資料</param>
        /// <param name="webApiLogAct">動作類型</param>
        /// <returns></returns>
        string AppStartLog<T>(T input, AppUserData userData, WebApiLogActType webApiLogAct);
        /// <summary>
        /// 開始呼叫紀錄 GET
        /// </summary>
        /// <param name="userData">用戶端資料</param>
        /// <param name="webApiLogAct">動作類型</param>
        /// <returns></returns>
        string AppStartLog(AppUserData userData, WebApiLogActType webApiLogAct);
        /// <summary>
        /// 結束呼叫紀錄 POST
        /// </summary>
        /// <param name="input">輸入參數</param>
        /// <param name="userData">用戶端資料</param>
        /// <param name="result">輸出資料</param>
        /// <param name="webApiLogAct">動作類型</param>
        /// <param name="isBigData">是否為大型資料</param>
        /// <returns></returns>
        string AppEndLog<T>(T input, AppUserData userData, Result result, WebApiLogActType webApiLogAct, bool isBigData = false);
        /// <summary>
        /// 結束呼叫紀錄 GET
        /// </summary>
        /// <param name="userData">用戶端資料</param>
        /// <param name="result">輸出資料</param>
        /// <param name="webApiLogAct">動作類型</param>
        /// <param name="isBigData">是否為大型資料</param>
        /// <returns></returns>
        string AppEndLog(AppUserData userData, Result result, WebApiLogActType webApiLogAct, bool isBigData = false);

    }
}