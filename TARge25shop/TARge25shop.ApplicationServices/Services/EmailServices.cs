

using Microsoft.Extensions.Configuration;
using MimeKit;
using TARge25shop.Core.Dto;

namespace TARge25shop.ApplicationServices.Services
{
    public class EmailServices
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
            email.From.Add(MailboxAddress.Parse());
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };
        }
    }
}
