using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SocialNetwork.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Services.BusinessLogic
{
    public static class Constans
    {
        public static int EMAIL_PORT = 587;
        public static bool EMAIL_USE_SSL = false;
    }

    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Support", _configuration["EMAILHOSTUSER"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("Plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["EMAILHOST"], Constans.EMAIL_PORT, Constans.EMAIL_USE_SSL);
                await client.AuthenticateAsync(_configuration["EMAILHOSTUSER"], _configuration["EMAILHOSTPASSWORD"]);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
