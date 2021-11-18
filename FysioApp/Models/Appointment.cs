using FysioApp.Models.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class Appointment
    {   
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Omschrijving")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Datum en Tijd")]
        public DateTime DateTime { get; set; }

        [Required]
        [Display(Name = "Behandelaar")]
        public string TeacherId { get; set; }

        [Required]
        [Display(Name = "Patient")]
        public string PatientId { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}
