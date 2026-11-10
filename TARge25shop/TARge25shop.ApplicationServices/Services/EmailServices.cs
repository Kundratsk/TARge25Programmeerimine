using Microsoft.Extensions.Configuration;
using MimeKit;
using TARge25shop.Core.Dto;
using MailKit.Net.Smtp;
using TARge25shop.Core.ServiceInterface;


namespace TARge25shop.ApplicationServices.Services
{
    public class EmailServices : IEmailServices
    {

        private readonly IConfiguration _config;

        public EmailServices(IConfiguration config)
        {
            _config = config;
        }



        public void SendEmail(EmailDto dto)
        {
            var email = new MimeMessage();
            // set the sender and recipient addresses
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };

            //failide lisamine

            foreach (var file in dto.Attachments)
            {
                if (file.Length > 0 && file.Length < 10485760) //10mb
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        stream.Position = 0; //Reset the stream position to the beginning
                        builder.Attachments.Add(file.FileName, stream.ToArray());
                    }
                }
            }
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_config.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void SendEmailToken(EmailTokenDto dto, string token)
        {
            dto.Token = token;

            var email = new MimeMessage();
            // set the sender and recipient addresses
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_config.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

    }
}
