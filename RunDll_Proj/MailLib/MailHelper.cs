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
            public static string Send(MailDTO dTO)
            {
                string result = "";

                try
                {
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress(MailConfig.MailDisplayName, MailConfig.MailAccount));
                    email.To.Add(MailboxAddress.Parse(dTO.To));
                    email.Subject = dTO.Subject;
                    email.Body = new TextPart(TextFormat.Html) { Text = dTO.Body };

                    SecureSocketOptions options = new();

                    switch (MailConfig.SecureSocketOptions)
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
                    smtp.Connect(MailConfig.Host, MailConfig.Port, options);
                    smtp.Authenticate(MailConfig.MailAccount, MailConfig.MailPassword);
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
