using ApplicationCore.Entities.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Treatment
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Omschrijving Behandeling")]
        public string Description { get; set; }

        [Display(Name = "Toelichting")]
        public string Explanation { get; set; }

        [Display(Name = "Ruimte")]
        public string Room { get; set; }

        [Display(Name = "Behandeling uitgevoerd op datum")]
        public DateTime DateTime { get; set; }

        [Display(Name="Behandeling uitgevoerd door")]
        public string StudentId { get; set; }

        [Display(Name = "PatientDossier")]
        public int PatientFileId { get; set; }

        //virtuals
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        [ForeignKey("PatientFileId")]
        public virtual PatientFile PatientFile { get; set; }
    }
}
