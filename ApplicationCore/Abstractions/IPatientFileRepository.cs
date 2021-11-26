using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Abstractions
{
    public interface IPatientFileRepository
    {
        IQueryable<PatientFile> GetFiles();
        IQueryable<PatientFile> GetFile(int id);
        IQueryable<PatientFile> GetFileByPatientId(string id);

        void CreateFile(PatientFile patient);
        void UpdateFile(int id, PatientFile patient);
        void DeleteFile(int id);

        void AddComment(Comment comment);

        void Save();
    }
}
