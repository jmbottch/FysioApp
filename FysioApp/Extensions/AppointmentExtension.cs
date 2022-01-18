using ApplicationCore.Entities;
using FysioApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Extensions
{
    public static class AppointmentExtension
    {
        public static bool CanMakeAnotherAppointment(this AppointmentsController controller, IEnumerable<Appointment> appointments, int maxAmount)
        {
            if(appointments.Count() >= maxAmount)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
