
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Abstractions
{
    public interface IAppointmentRepository
    {
        IQueryable<Appointment> GetAppointments();
        IQueryable<Appointment> GetAppointment(int id);
        IQueryable<Appointment> GetAppointmentsByPatientId(string id);
        IQueryable<Appointment> GetAppointmentsOfPatientWithinOneWeek(string patientId, DateTime startofweek, DateTime endofweek);
        void CreateAppointment(Appointment appointment);
        void UpdateAppointment(int id, Appointment appointment);
        void DeleteAppointment(int id);
        void Save();
    }
}
