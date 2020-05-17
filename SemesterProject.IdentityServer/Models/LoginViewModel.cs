using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.IdentityServer.Models
{
	public class LoginViewModel
	{
        [Required(ErrorMessage = "Login is required.")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter your password.")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
