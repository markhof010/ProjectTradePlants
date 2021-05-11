using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace PlantTrade.Services
{
    public class EmailSender : IEmailSender
    {

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }

        public Task Execute(string subject, string message, string email)
        {
            SmtpClient client = new SmtpClient()
            {
                //Use own SmtpClient
            };

            var mail = new MailAddress(email);

            var msg = new MailMessage()
            {
                From = new MailAddress("", "StekExchange"),
                Subject = subject,
                IsBodyHtml = true,
                Body = message
            };
            msg.To.Add(mail);

            return client.SendMailAsync(msg);
        }
    }
}
