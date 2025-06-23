using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ProjektZaliczeniowyNET.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Sprawdź czy email jest prawidłowy
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email nie może być pusty", nameof(email));

            // Pobierz konfigurację
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            // Sprawdź czy konfiguracja jest kompletna
            if (string.IsNullOrEmpty(senderEmail))
            {
                // Tymczasowo dla testów - wypisz w konsoli
                Console.WriteLine($"EMAIL WYSŁANY DO: {email}");
                Console.WriteLine($"TEMAT: {subject}");
                Console.WriteLine($"TREŚĆ: {htmlMessage}");
                return;
            }

            try
            {
                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd wysyłania emaila: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
    
                Console.WriteLine($"EMAIL MIAŁ BYĆ WYSŁANY DO: {email}");
                Console.WriteLine($"TEMAT: {subject}");
            }
        }
    }
}
