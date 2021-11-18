using FysioApp.Models.ApplicationUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FysioApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<IdentityStudent> Student { get; set; }
        public DbSet<IdentityTeacher> Teacher { get; set; }
        public DbSet<IdentityPatient> Patient { get; set; }
    }
}
