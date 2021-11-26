using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.ApplicationUsers
{
    public class Student : ApplicationUser
    {       
        [Required]
        [Display(Name = "Student Nummer")]
        public int StudentNumber { get; set; }
    }
}
