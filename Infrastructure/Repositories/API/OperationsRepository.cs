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
    public class OperationsRepository : IOperationsRepository
    {
        private readonly ApiDbContext _context;

        public OperationsRepository(ApiDbContext context)
        {
            _context = context;
        }
        public IQueryable<Operation> GetAll()
        {
            return _context.Operations;
        }

        public IQueryable<Operation> GetOne(string Code)
        {
            return _context.Operations.Where(o => o.Code == Code);
        }
    }
}
