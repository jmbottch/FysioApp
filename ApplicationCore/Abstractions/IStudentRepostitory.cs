using ApplicationCore.Entities.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Abstractions
{
    public interface IStudentRepostitory
    {
        IQueryable<Student> GetStudents();
        IQueryable<Student> GetStudent(string id);
        void CreateStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(string id);
        void Save();
    }
}
