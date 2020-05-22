using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.IdentityServer.Models
{
	public class ResetPasswordModel
	{

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter your password.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm yout password")]
        [Compare("Password", ErrorMessage = "Passwords must match.")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
		public string Code { get; set; }
		public Guid UserId { get; set; }
	}
}
