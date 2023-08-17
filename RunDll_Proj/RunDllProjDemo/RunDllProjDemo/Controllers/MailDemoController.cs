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

        readonly IConfiguration _configuration;

        readonly MailConfig _mailConfig = new();

        public MailDemoController(IConfiguration configuration)
        {
            _configuration = configuration;

            #region mailSendConfig
            _mailConfig.Host = _configuration.GetValue<string>("MailConfig:Host");
            _mailConfig.Port = _configuration.GetValue<int>("MailConfig:Port");
            _mailConfig.MailAccount = _configuration.GetValue<string>("MailConfig:MailAccount");
            _mailConfig.MailPassword = _configuration.GetValue<string>("MailConfig:MailPassword");
            _mailConfig.SecureSocketOptions = _configuration.GetValue<int>("MailConfig:SecureSocketOptions");
            _mailConfig.MailDisplayName = _configuration.GetValue<string>("MailConfig:MailDisplayName");
            #endregion
        }

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
                MailHelper.SendMail sendMail = new(_mailConfig);

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

                errStr = sendMail.Send(mailDTO);
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
