using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Abstractions
{
    public interface IIdentityUserRepository
    {
        IQueryable<IdentityUser> GetUsers();
        IQueryable<IdentityUser> GetUser(string id);
        IQueryable<IdentityUser> GetUserByEmail(string email);
        void CreateUser(IdentityUser user);
        void UpdateUser(IdentityUser user);
        void DeleteUser(string Id);
        void Save();
    }
}
