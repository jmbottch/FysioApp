using FysioApp.Models.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Abstractions
{
    public interface ITeacherRepository
    {
        IQueryable<Teacher> GetTeachers();
        IQueryable<Teacher> GetTeacher(string id);

        void CreateTeacher(Teacher teacher);
        void UpdateTeacher(string id, Teacher teacher);
        void DeleteTeacher(string id);

        void Save();
    }
}
