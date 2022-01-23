
using ApplicationCore.Abstractions;
using ApplicationCore.Entities.ApplicationUsers;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly BusinessDbContext _business;

        public PatientRepository(BusinessDbContext business)
        {
            _business = business;
        }

        public IQueryable<Patient> GetPatients()
        {
            return _business.Patient;
        }
        public IQueryable<Patient> GetPatient(string id)
        {
            return _business.Patient.Where(p => p.Id == id);
        }

        public void CreatePatient(Patient patient)
        {
            _business.Patient.Add(patient);
        }
        public void UpdatePatient(string id, Patient patient)
        {
            throw new NotImplementedException();
        }

        public void DeletePatient(string id)
        {
            Patient patient = _business.Patient.Where(p => p.Id == id).FirstOrDefault();
            _business.Patient.Remove(patient);
        }

        public void Save()
        {
           _business.SaveChanges();
        }

    }
}
