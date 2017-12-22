using DbModel.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Interfaces
{

    public interface IRoleService
    {
        bool CreateRole(string roleName, string description = "");

        void AddUserToRole(User user, string roleName);
        bool RoleExist(string roleName);
        void RemoveRole(string roleName);
        IList<Role> GetAllRoles();
        IList<User> UsersInRole(string roleName);
        Role GetRoleByUserName(string userName);
        Task<Role> GetRoleByUserId(long userId);
        void RemoveUserFromRole(string userName);
        void EditRoleForUser(string userName, string roleName);
        Role GetRoleByName(string roleName);
        Role GetRoleByRoleId(long roleId);
    }
}
