using FysioApp.Models.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models.ViewModels.AppointmentViewModels
{
    public class CreateAppointmentViewModel
    {
        public Appointment Appointment { get; set; }

        public IEnumerable<Teacher> Teachers { get; set; }
        public IEnumerable<Patient> Patients { get; set; }
    }
}
