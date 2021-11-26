using ApplicationCore.Entities;
using ApplicationCore.Entities.ApplicationUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BusinessDbContext : DbContext
    {
        public BusinessDbContext(DbContextOptions<BusinessDbContext> options)
           : base(options)
        {
        }

        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<PatientFile> PatientFile { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}
