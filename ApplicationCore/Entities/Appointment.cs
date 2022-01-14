using ApplicationCore.Entities.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Appointment
    {   
        [Required]
        public int Id { get; set; }

        [Display(Name = "Omschrijving")]
        public string Description { get; set; }     

        [Required]
        [Display(Name = "Datum en Tijd")]        
        public DateTime DateTime { get; set; }

        [Display(Name ="Eindtijd")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Behandelaar")]
        public string StudentId { get; set; }

        [Required]
        [Display(Name = "Patient")]
        public string PatientId { get; set; }

        [Display(Name ="Geannuleerd")]
        public bool IsCancelled { get; set; }

        


        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}
