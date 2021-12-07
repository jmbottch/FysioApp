using ApplicationCore.Entities.ApiEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Abstractions.Api
{
    public interface IDiagnosesRepository
    {
        IQueryable<Diagnose> GetAll();
        IQueryable<Diagnose> GetOne(string Code);
    }
}
