using FysioApp.Models.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Abstractions
{
    public interface IStudentRepostitory
    {
        IQueryable<Student> GetStudents();
        IQueryable<Student> GetStudent(string id);
        void CreateStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(string Id);
        void Save();
    }
}
