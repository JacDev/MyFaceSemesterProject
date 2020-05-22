using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SemesterProject.IdentityServer.Services
{
	public class EmailSender : IEmailSender
	{
		private string _apiKey;
		private string _fromName;
		private string _fromEmail;

		public EmailSender(IConfiguration config)
		{
			_apiKey = config.GetSection("SendGrid:ApiKey").Value;
			_fromName = config.GetSection("SendGrid:FromName").Value;
			_fromEmail = config.GetSection("SendGrid:FromEmail").Value;
		}
		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var client = new SendGridClient(_apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress(_fromEmail, _fromName),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(email));
			msg.SetClickTracking(false, false);
			var response = await client.SendEmailAsync(msg);
		}
	}
}
