using FysioApp.Models.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models
{
    public class Comment
    {
        [Required]        
        public int Id { get; set; }
        [Required]
        [Display(Name = "Opmerking")]
        public string Content { get; set; }
        [Required]
        [Display(Name = "Datum/Tijd")]
        public DateTime TimeOfPosting { get; set; }
        [Required]
        [Display(Name = "Auteur")]
        public string AuthorName { get; set; }
        [Display(Name ="Zichtbaar voor Patient")]
        public bool Visible { get; set; }
        [Required]
        public int PatientFileId { get; set; }

        [ForeignKey("PatientFileId")]
        public virtual PatientFile PatientFile { get; set; }
        
    }
}
