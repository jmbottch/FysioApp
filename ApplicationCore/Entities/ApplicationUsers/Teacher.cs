using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        //AVAILABILITY
        public int AvailabilityId { get; set; }
        [ForeignKey("AvailabilityId")]
        public virtual Availability Availability { get; set; }
    }
}
