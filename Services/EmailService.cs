using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;
using MailKit.Security;
using DTO;
namespace Services
{
	public class EmailService
	{
		// Récupération du serveur SMTP, de l'identifiant et du mot de passe
		private readonly string smtpServer = Environment.GetEnvironmentVariable("SMTP_HOST");
		private readonly string smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
		private readonly string smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");
		private readonly string SenderName = Environment.GetEnvironmentVariable("SenderName");
		private readonly string SenderEmail = Environment.GetEnvironmentVariable("SenderEmail");

		private readonly int smtpPort=587;



	public async Task<EmailSendResult> SendVerificationEmailAsync(string toEmail, string Name, string Token)
{
	try
	{
		string html = await System.IO.File.ReadAllTextAsync("Templates/Emails/EmailVerification.html");
		html = html
			.Replace("{{Name}}", Name)
			.Replace("{{token}}", Token);

		var email = new MimeMessage();

		// Ajout de l'expéditeur
		email.From.Add(new MailboxAddress(SenderName, SenderEmail));

		// Destinataire
		email.To.Add(MailboxAddress.Parse(toEmail));

		// Sujet et corps
		email.Body = new TextPart("html") { Text = html };

		using var smtp = new SmtpClient();
		await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
		await smtp.AuthenticateAsync(smtpUser, smtpPass);
		
		await smtp.SendAsync(email);
		await smtp.DisconnectAsync(true);
		return new EmailSendResult {Success = true  };
		
	}
	catch (Exception ex)
	{
			return new EmailSendResult {Success = false , ErrorMessage=ex.Message   };

	}
}



	}
}
