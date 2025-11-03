using IndkoebsGenieBackend.DTO.EmailDTO;
using IndkoebsGenieBackend.Helper;
using IndkoebsGenieBackend.Interfaces.IEmailService;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace IndkoebsGenieBackend.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public void SendEmail(EmailResponse mailData)
        {
            using (MimeMessage emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);
                emailMessage.To.Add(MailboxAddress.Parse(mailData.To));

                emailMessage.Subject = mailData.Subject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = mailData.Body;
                emailMessage.Body = new TextPart(TextFormat.Html)
                {
                    Text = mailData.Body
                };

                using SmtpClient mailClient = new SmtpClient();
                mailClient.Connect(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
                mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                mailClient.Send(emailMessage);
                mailClient.Disconnect(true);
            }
        }
    }
}
