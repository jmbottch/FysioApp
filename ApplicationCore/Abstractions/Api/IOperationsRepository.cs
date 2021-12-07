using ApplicationCore.Entities.ApiEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Abstractions.Api
{
    public interface IOperationsRepository
    {
        IQueryable<Operation> GetAll();
        IQueryable<Operation> GetOne(string Code);
    }
}
