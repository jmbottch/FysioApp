using FysioApp.Models.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class PatientFile
    {
        [Required]
        public int Id { get; set; } // Id van Dossier
        [Required]
        [Display(Name ="Patient")]
        public string PatientId { get; set; } //Id van Patient

        //Algemene Informatie
        [Display(Name = "Leeftijd")]
        public int age { get; set; } //Leeftijd Patient
        [Required]
        [Display(Name = "Omschrijving klachten globaal")]
        public string ComplaintsDescription { get; set; } //Omschrijving Klachten

        //Behandelaars
        [Display(Name = "Intake gedaan door")]
        public string IntakeDoneById { get; set; } //Id van Student die de Intake gedaan heeft
        [Required]
        [Display(Name = "Intake onder toezicht van")]
        public string IntakeSupervisedById { get; set; } //Id van Teacher die toezicht hield bij Intake
        [Required]
        [Display(Name = "Hoofdbehandelaar")]
        public string HeadPractitionerId { get; set; } //Id van Student die hoofdbehandelaar is van de patient

        //Data
        [Required]
        [Display(Name = "Datum van binnenkomst")]
        public DateTime DateOfArrival { get; set; } //Datum van binnenkomst
        [Display(Name = "Datum van ontslag")]
        public DateTime DateOfDeparture { get; set; } //Datum van ontslag

        //Behandelplan
        [Required]
        [Display(Name = "Duur per sessie")]
        public double SessionDuration { get; set; }
        [Required]
        [Display(Name = "Aantal sessies per week")]
        public int AmountOfSessionsPerWeek { get; set; }

        //Virtuals
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("IntakeDoneById")]
        public virtual Student IntakeDoneBy { get; set; }
        [ForeignKey("IntakeSupervisedById")]
        public virtual Teacher IntakeSupervisedBy { get; set; }

        [ForeignKey("HeadPractitionerId")]
        public virtual Student HeadPractitioner { get; set; }
    }
}
