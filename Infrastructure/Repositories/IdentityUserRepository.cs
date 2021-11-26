using ApplicationCore.Abstractions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class IdentityUserRepository : IIdentityUserRepository
    {
        private readonly ApplicationDbContext _identity;

        public IdentityUserRepository(ApplicationDbContext identity)
        {
            _identity = identity;
        }

        public IQueryable<IdentityUser> GetUsers()
        {
            return _identity.Users;
        }
        public IQueryable<IdentityUser> GetUser(string id)
        {
            return _identity.Users.Where(u => u.Id == id);
        }
        public IQueryable<IdentityUser> GetUserByEmail(string email)
        {
            return _identity.Users.Where(u => u.NormalizedEmail == email.ToUpper());
        }

        public void CreateUser(IdentityUser user)
        {

            throw new NotImplementedException();
        }

        public void UpdateUser(IdentityUser user)
        {
            throw new NotImplementedException();
        }
        public void DeleteUser(string id)
        {
            var user = _identity.Users.FirstOrDefault(t => t.Id == id);
            _identity.Users.Remove(user);
        }
        public void Save()
        {
            _identity.SaveChanges();
        }

    }
}
