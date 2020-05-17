using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Models
{
	public class UserToAdd
	{

        [Required(ErrorMessage = "Login is required.")]
        [MaxLength(12, ErrorMessage = "Passwords must be at least 8 characters.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Name must be max 20 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(20, ErrorMessage = "LastName must be max 20 characters.")]
        public string LastName { get; set; }
    }
}
