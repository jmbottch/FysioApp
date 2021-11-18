using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models.ApplicationUsers
{
    public class Patient : ApplicationUser
    {        
        [Required]
        public int AvansNumber { get; set; }
        [Required]
        public string AvansRole { get; set; }
        public byte[] Picture { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public enum EAvansRole { 
            [Display(Name = "Student")]
            Student = 0,
            [Display(Name = "Docent")]
            Teacher = 1 
        }
        public enum EGender {
            [Display(Name = "Man")]
            Male = 0,
            [Display(Name = "Vrouw")]
            Female = 1,
            [Display(Name = "X")]
            X = 2 }
    }

}
