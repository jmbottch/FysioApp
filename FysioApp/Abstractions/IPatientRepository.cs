using FysioApp.Models.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Abstractions
{
    public interface IPatientRepository
    {
        IQueryable<Patient> GetPatients();
        IQueryable<Patient> GetPatient(string id);

        void CreatePatient(Patient patient);
        void UpdatePatient(string id, Patient patient);
        void DeletePatient(string id);

        void Save();
    }
}
