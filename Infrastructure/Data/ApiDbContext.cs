using ApplicationCore.Entities.ApiEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.Data
{
    public class ApiDbContext : DbContext
    {     

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
          : base(options)
        {
        }

        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Operation> Operations { get; set; }

    }
}