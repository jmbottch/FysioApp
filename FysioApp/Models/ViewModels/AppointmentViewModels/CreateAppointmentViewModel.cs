using ApplicationCore.Entities;
using ApplicationCore.Entities.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Models.ViewModels.AppointmentViewModels
{
    public class CreateAppointmentViewModel
    {
        public Appointment Appointment { get; set; }

        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Patient> Patients { get; set; }
    }
}
