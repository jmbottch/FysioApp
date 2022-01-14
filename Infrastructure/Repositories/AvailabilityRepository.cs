using ApplicationCore.Abstractions;
using ApplicationCore.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly BusinessDbContext _business;

        public AvailabilityRepository(BusinessDbContext business)
        {
            _business = business;
        }

        public void CreateAvailability(Availability availability)
        {
            _business.Availabilities.Add(availability);
        }

        public IQueryable<Availability> GetAvailability(int id)
        {
            return _business.Availabilities.Where(a => a.Id == id);
        }

        public void Save()
        {
            _business.SaveChanges();
        }
    }
}
