using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.IdentityServer.Models
{
	public class ForgotPasswordModel
	{
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress]
		public string Email { get; set; }
		public string ReturnUrl { get; set; }
	}
}
