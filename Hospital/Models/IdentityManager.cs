using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital.Models
{
    public class IdentityManager
    {
        // Swap ApplicationRole for IdentityRole:
        RoleManager<Role, int> _roleManager = new RoleManager<Role, int>(
            new RoleStore<Role, int, UserRole>(new DBContext()));

        UserManager<ApplicationUser, int> _userManager = new UserManager<ApplicationUser, int>(
            new UserStore<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>(new DBContext()));

        // ApplicationDbContext _db = new ApplicationDbContext();


        public bool RoleExists(string name)
        {
            return _roleManager.RoleExists(name);
        }


        public IQueryable<Role> GetRoles()
        {
            return _roleManager.Roles;
        }


        public IdentityResult CreateUser(ApplicationUser user, string password)
        {
            var userId = 0;

            var result = _userManager.Create(user, password);
            if (result.Succeeded)
            {

                userId = _userManager.Users.Max(usr => (int?)usr.Id) ?? 0;
            }

            return result;
        }


        public bool AddUserToRole(int userId, string roleName)
        {
            var idResult = _userManager.AddToRole<ApplicationUser, int>(userId, roleName);
            return idResult.Succeeded;
        }

        public object RemoveUserFromRole(int userid, string name)
        {
            var idResult = _userManager.RemoveFromRole<ApplicationUser, int>(userid, name);
            return idResult.Succeeded;
        }
    }

}