using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Services
{
    public static class Constans
    {
        public static string EMAIL_HOST = "smtp.gmail.com";
        public static int EMAIL_PORT = 587;
        public static string EMAIL_HOST_USER = "socialnetworkaspnetcore@gmail.com";
        public static string EMAIL_HOST_PASSWORD = "art23.art23";
        public static bool EMAIL_USE_SSL = false;
    }

    public class MailService : IMailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Support", Constans.EMAIL_HOST_USER));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("Plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Constans.EMAIL_HOST, Constans.EMAIL_PORT, Constans.EMAIL_USE_SSL);
                await client.AuthenticateAsync(Constans.EMAIL_HOST_USER, Constans.EMAIL_HOST_PASSWORD);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
