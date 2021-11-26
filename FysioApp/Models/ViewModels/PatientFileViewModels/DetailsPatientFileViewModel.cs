using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models.ViewModels.PatientFileViewModels
{
    public class DetailsPatientFileViewModel
    {
        public PatientFile PatientFile { get; set; }
        public Comment Comment { get; set; }
    }
}
