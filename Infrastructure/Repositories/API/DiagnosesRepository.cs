using ApplicationCore.Abstractions.Api;
using ApplicationCore.Entities.ApiEntities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.API
{
    public class DiagnosesRepository : IDiagnosesRepository
    {
        private readonly ApiDbContext _context;
        public DiagnosesRepository(ApiDbContext context)
        {
            _context = context;
        }

        public IQueryable<Diagnose> GetAll()
        {
            return _context.Diagnoses;
        }
        public IQueryable<Diagnose> GetOne(string Code)
        {
            return _context.Diagnoses.Where(v => v.Code == Code);
        }
    }
}
