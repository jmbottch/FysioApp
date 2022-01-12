using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Abstractions
{
    public interface ITreatmentRepository
    {
        IQueryable<Treatment> GetTreatments();
        IQueryable<Treatment> GetTreatment(int id);
        IQueryable<Treatment> GetTreatmentsByPatientFileId(int id);

        void CreateTreatment(Treatment treatment);
        void UpdateTreatment(int id, Treatment treatment);
        void DeleteTreatment(int id);
        void Save();



    }
}
