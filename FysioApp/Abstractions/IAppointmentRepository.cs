using FysioApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Abstractions
{
    public interface IAppointmentRepository
    {
        IQueryable<Appointment> GetAppointments();
        IQueryable<Appointment> GetAppointment(int id);
        IQueryable<Appointment> GetAppointmentsByPatientId(string id);
        void CreateAppointment(Appointment appointment);
        void UpdateAppointment(int id, Appointment appointment);
        void DeleteAppointment(int id);
        void Save();
    }
}
