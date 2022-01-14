using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entities.ApplicationUsers
{
    public class Student : ApplicationUser
    {       
        [Required]
        [Display(Name = "Student Nummer")]
        public int StudentNumber { get; set; }

        //AVAILABILITY
        public int AvailabilityId { get; set; }

        [ForeignKey("AvailabilityId")]
        public virtual Availability Availability { get; set; }
    }
}
