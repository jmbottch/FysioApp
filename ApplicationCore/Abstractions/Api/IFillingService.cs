using ApplicationCore.Entities.ApiEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Abstractions.Api
{
    public interface IFillingService
    {
        IEnumerable<Diagnose> FillDiagnoses();
        IEnumerable<Operation> FillOperations();
    }
}
