using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;
using MailKit.Security;

namespace Services
{
    public class EmailService
    {
        // Récupération du serveur SMTP, de l'identifiant et du mot de passe
        private readonly string smtpServer = Environment.GetEnvironmentVariable("Serveur");
        private readonly string smtpUser = Environment.GetEnvironmentVariable("Identifiant");
        private readonly string smtpPass = Environment.GetEnvironmentVariable("Pwd");
        private readonly string Email = Environment.GetEnvironmentVariable("Email");

        // Récupération du port SMTP et gestion de la conversion
        private readonly int smtpPort;

        public EmailService() // To be reviewed 
        {
            string portEnv = Environment.GetEnvironmentVariable("Port");

            // Convertir le port en entier, ou utiliser 587 par défaut si la conversion échoue
            if (!string.IsNullOrEmpty(portEnv) && int.TryParse(portEnv, out int parsedPort))
            {
                smtpPort = parsedPort;
            }
            else
            {
                smtpPort = 587;  // Port par défaut si la conversion échoue
                Console.WriteLine("Le port SMTP n'est pas valide ou manquant, utilisation du port 587 par défaut.");
            }
        }

		public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
		{
			var email = new MimeMessage();

			//  Adresse expéditeur vérifiée (que tu as validée dans Brevo)
			email.From.Add(MailboxAddress.Parse(Email));

			//  Destinataire
			email.To.Add(MailboxAddress.Parse(toEmail));

			// Sujet et contenu HTML
			email.Subject = subject;
			email.Body = new TextPart("html") { Text = htmlContent };

			//  Envoi via SMTP
			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(smtpUser, smtpPass);
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}

    }
}
