using ApplicationCore.Abstractions;
using Infrastructure.Data;
using ApplicationCore.Entities.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly BusinessDbContext _business;
        public TeacherRepository(BusinessDbContext business)
        {
            _business = business;
        }

        public IQueryable<Teacher> GetTeachers()
        {
            return _business.Teacher;
        }
        public IQueryable<Teacher> GetTeacher(string id)
        {
            return _business.Teacher.Where(t => t.Id == id).Include(a => a.Availability);
        }

        public void CreateTeacher(Teacher teacher)
        {
            _business.Teacher.Add(teacher);
        }

        public void UpdateTeacher(string id, Teacher teacher)
        {
            throw new NotImplementedException();
        }

        public void DeleteTeacher(string id)
        {
            var teacher = _business.Teacher.FirstOrDefault();
            _business.Remove(teacher);
        }

        public void Save()
        {
            _business.SaveChanges();
        }
    }
}
