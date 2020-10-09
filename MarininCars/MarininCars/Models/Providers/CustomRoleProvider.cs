using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MarininCars.Models.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        AccountContext db = new AccountContext();
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] roles = new string[] { };
            BdUsers user = db.BdUsers.FirstOrDefault(u => u.Login == username);
            if (user != null)
            {
                BdRole UserRole = db.Roles.FirstOrDefault(r=>r.Id==user.RoleId);
                if (UserRole != null)
                    roles = new string[]{UserRole.Name};
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool outRes = false;
            BdUsers user = db.BdUsers.FirstOrDefault(u => u.Login == username);
            if (user != null)
            {
                BdRole userRole = db.Roles.Find(user.RoleId);
                if (user.Role != null && userRole.Name == roleName)
                    outRes = true;
            }
            return outRes;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}