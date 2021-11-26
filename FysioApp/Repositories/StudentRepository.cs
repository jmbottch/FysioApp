﻿using FysioApp.Abstractions;
using FysioApp.Data;
using FysioApp.Models.ApplicationUsers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioApp.Repositories
{
    public class StudentRepository : IStudentRepostitory
    {
        private readonly BusinessDbContext _business;
        public StudentRepository(BusinessDbContext business)
        {
            _business = business;
        }

        public IQueryable<Student> GetStudents()
        {
            return _business.Student;

        }

        public IQueryable<Student> GetStudent(string id)
        {
            return _business.Student.Where(s => s.Id == id);
        }

        public void CreateStudent(Student student)
        {
            _business.Student.Add(student);
        }

        public void UpdateStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public void DeleteStudent(string id)
        {
            var student = _business.Student.FirstOrDefault(s => s.Id == id);
            _business.Remove(student);
        }

        public void Save()
        {
            _business.SaveChanges();
        }
    }
}
