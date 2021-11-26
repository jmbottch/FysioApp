using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.ApplicationUsers
{
    public class ApplicationUser
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

    }
}
