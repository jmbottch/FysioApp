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
    public class PatientFileRepository : IPatientFileRepository
    {
        private readonly BusinessDbContext _business;

        public PatientFileRepository(BusinessDbContext business)
        {
            _business = business;
        }

        public IQueryable<PatientFile> GetFiles()
        {
            return _business.PatientFile.Include(p => p.HeadPractitioner).Include(p => p.Patient);
        }
        public IQueryable<PatientFile> GetFile(int id)
        {
            return _business.PatientFile.Where(f => f.Id == id).Include(p => p.HeadPractitioner).Include(p => p.Patient).Include(p => p.IntakeDoneBy).Include(p => p.IntakeSupervisedBy);
        }

        public IQueryable<PatientFile> GetFileByPatientId(string id)
        {
            return _business.PatientFile.Where(f => f.PatientId == id).Include(p => p.HeadPractitioner).Include(p => p.Patient).Include(p => p.IntakeDoneBy).Include(p => p.IntakeSupervisedBy);
        }

        public IQueryable<Comment> GetCommentsByPatientFileId(int id)
        {
            return _business.Comment.Where(f => f.PatientFileId == id);
        }
        

        public void CreateFile(PatientFile file)
        {
            _business.PatientFile.Add(file);
        }

        public void AddComment(Comment comment)
        {
            _business.Comment.Add(comment);
        }

        public void DeleteFile(int id)
        {
            PatientFile patient = _business.PatientFile.Where(p => p.Id == id).FirstOrDefault();
            _business.PatientFile.Remove(patient);
        }


        public void Save()
        {
            _business.SaveChanges();
        }

        public void UpdateFile(int id, PatientFile file)
        {
            throw new NotImplementedException();
        }
    }
}
