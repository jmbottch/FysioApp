using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models.ViewModels.ApplicationUserViewModels
{
    public class RegisterPatientViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email adres")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Telefoonnummer")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Avans Nummer")]
        public int AvansNumber { get; set; }
       
        [Display(Name = "Profiel foto")]
        public byte[] Picture { get; set; }

        [Required]
        [Display(Name = "Geslacht")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Geboortedatum")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Rol Binnen Avans")]
        public string AvansRole { get; set; }

    }
}
