using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace MailLib
{
    public class MailHelper
    {
        /// <summary>
        /// 發送郵件
        /// </summary>
        public class SendMail
        {
            readonly MailConfig _config;

            public SendMail(MailConfig mailConfig)
            {
                _config = mailConfig;
            }

            public string Send(MailDTO dTO)
            {
                string result = "";

                try
                {
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress(_config.MailDisplayName, _config.MailAccount));
                    email.To.Add(MailboxAddress.Parse(dTO.To));
                    email.Subject = dTO.Subject;
                    email.Body = new TextPart(TextFormat.Html) { Text = dTO.Body };

                    SecureSocketOptions options = new();

                    switch (_config.SecureSocketOptions)
                    {
                        case 0:
                            options = SecureSocketOptions.None;
                            break;
                        case 1:
                            options = SecureSocketOptions.Auto;
                            break;
                        case 2:
                            options = SecureSocketOptions.SslOnConnect;
                            break;
                        case 3:
                            options = SecureSocketOptions.StartTls;
                            break;
                        case 4:
                            options = SecureSocketOptions.StartTlsWhenAvailable;
                            break;
                    }

                    using var smtp = new SmtpClient();
                    smtp.Connect(_config.Host, _config.Port, options);
                    smtp.Authenticate(_config.MailAccount, _config.MailPassword);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }

                return result;
            }
        }
    }
}
