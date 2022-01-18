using ApplicationCore.Abstractions;
using Infrastructure.Data;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly BusinessDbContext _business;

        public AppointmentRepository(BusinessDbContext business)
        {
            _business = business;
        }

        public IQueryable<Appointment> GetAppointments()
        {
            return _business.Appointment.Include(a => a.Student).Include(a => a.Patient).OrderBy(x => x.DateTime);
        }

        public IQueryable<Appointment> GetAppointment(int id)
        {
            return _business.Appointment.Where(a => a.Id == id).Include(a => a.Student).Include(a => a.Patient).OrderBy(x => x.DateTime);
        }

        public IQueryable<Appointment> GetAppointmentsByPatientId(string id)
        {
            return _business.Appointment.Where(a => a.PatientId == id).Include(a => a.Student).Include(a => a.Patient).OrderBy(x => x.DateTime);
        }

        public IQueryable<Appointment> GetAppointmentsOfPatientWithinOneWeek(string patientId, DateTime startofweek, DateTime endofweek)
        {
            return _business.Appointment.Where(a => a.PatientId == patientId).Where(a => a.DateTime >= startofweek && a.DateTime <= endofweek);
        }

        public void CreateAppointment(Appointment appointment)
        {
            _business.Appointment.Add(appointment);
        }

        public void UpdateAppointment(int id, Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public void DeleteAppointment(int id)
        {
            Appointment appointment = _business.Appointment.Where(a => a.Id == id).FirstOrDefault();
            _business.Appointment.Remove(appointment);
        }

        public void Save()
        {
            _business.SaveChanges();
        }
    }
}
