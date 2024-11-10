using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace M6_MovieClub.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            string host = this._config.GetValue<string>("GEmailConfiguration:Host");
            int port = this._config.GetValue<int>("GEmailConfiguration:Port");
            string senderEmail = this._config.GetValue<string>("GEmailConfiguration:Email");
            string appPassword = this._config.GetValue<string>("GEmailConfiguration:AppPassword");

            using (SmtpClient client = new SmtpClient()
            {
                Host = host,
                Port = port,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(senderEmail, appPassword),
                EnableSsl = true,
            })
            {
                MailMessage message = new MailMessage()
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = htmlMessage,
                    BodyEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8,
                };
                message.To.Add(email);

                try
                {
                    client.Send(message);
                }
                catch (Exception e)
                {
                   ;
                }
            }
            return Task.CompletedTask;
        }
    }
}
