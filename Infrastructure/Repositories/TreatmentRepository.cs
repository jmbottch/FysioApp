using ApplicationCore.Abstractions;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TreatmentRepository : ITreatmentRepository
    {

        private readonly BusinessDbContext _business;

        public TreatmentRepository(BusinessDbContext business)
        {
            _business = business;
        }

        public IQueryable<Treatment> GetTreatments()
        {
            return _business.Treatments.Include(s => s.Student).Include(pf => pf.PatientFile).Include(p => p.PatientFile.Patient).OrderBy(x => x.DateTime);
        }
        
        public IQueryable<Treatment> GetTreatment(int id)
        {
            return _business.Treatments.Where(x => x.Id == id).Include(s => s.Student).Include(p => p.PatientFile).Include(p => p.PatientFile.Patient);
        }

        public void CreateTreatment(Treatment treatment)
        {
            _business.Treatments.Add(treatment);
        }

        public void DeleteTreatment(int id)
        {
            Treatment treatment = GetTreatment(id).FirstOrDefault();
            _business.Treatments.Remove(treatment);
        }         

        public void Save()
        {
            _business.SaveChanges();
        }

        public IQueryable<Treatment> GetTreatmentsByPatientFileId(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateTreatment(int id, Treatment treatment)
        {
            throw new NotImplementedException();
        }
    }
}
