using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Abstractions
{
    public interface IAvailabilityRepository
    {
        IQueryable<Availability> GetAvailability(int id);
        void CreateAvailability(Availability availability);
        void Save();
    }
}
