using Microsoft.AspNetCore.Mvc;

using BasicConfig;
using MailLib;

namespace Controllers.API
{
    [Tags("MailDemo")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailDemoController : ControllerBase
    {
        string errStr = "";

        /// <summary>
        /// 電子郵件 寄件測試
        /// </summary>
        /// <param name="userEmail">收件者信箱</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SendTestEmail(string userEmail)
        {
            Result result = new();
            errStr = string.Empty;

            try
            {
                MailDTO mailDTO = new()
                {
                    To = userEmail,
                    Subject = "寄件測試",
                    Body = "" +
                    "＊＊＊＊＊＊＊＊　注意　＊＊＊＊＊＊＊＊<br>" +
                    "<span style='margin-left:50px;color:#EA0000;'>此信為系統發出，請勿回覆</span><br>" +
                    "＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊<br><br>" +
                    "<span>您好，</span><br><br>" +
                    "這是測試用郵件。<br><br>"
                };

                errStr = MailHelper.SendMail.Send(mailDTO);
                if (errStr != "") throw new Exception(errStr);

                result.ResultCode = ResultCode.Success;
                result.Message = "發送成功";
            }
            catch (Exception ex)
            {
                result.ResultCode = ResultCode.Exception;
                result.Message = ex.Message;
            }

            return Ok(result);
        }
    }
}
