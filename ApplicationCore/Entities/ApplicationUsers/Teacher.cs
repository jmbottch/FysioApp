using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.ApplicationUsers
{
    public class Teacher : ApplicationUser
    {
        [Required]
        [Display(Name = "Personeelsnummer")]
        public int EmployeeNumber { get; set; }
        [Required]
        [Display(Name = "BIG-Nummer")]
        public int BigNumber { get; set; }
    }
}
