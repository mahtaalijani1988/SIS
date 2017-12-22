using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModel.Context;
using DbModel.ServiceLayer.Interfaces;
using DbModel.DomainClasses.Entities;

namespace ServiceLayer.EFServices
{
    public class RoleService : IRoleService
    {
        //#region Fields
        private readonly IDbSet<Role> _roles;
        private readonly IUnitOfWork _unitOfWork;
        //#endregion //Fields
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _roles = _unitOfWork.Set<Role>();
        }
        public Role GetRoleByName(string name)
        {
            return _roles.Where(role => role.Name.Equals(name)).FirstOrDefault();
        }

        public async Task<Role> GetRoleByUserId(long userId)
        {
            return  await _roles.Where(role => role.Users.Where(user => user.Id == userId).FirstOrDefault().Id == userId)
                .FirstOrDefaultAsync();
        }

        public bool CreateRole(string roleName, string description = "")
        {
            throw new NotImplementedException();
        }

        public void AddUserToRole(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public bool RoleExist(string roleName)
        {
            throw new NotImplementedException();
        }

        public void RemoveRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public IList<Role> GetAllRoles()
        {
            return _roles.ToList();
        }

        public IList<User> UsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public Role GetRoleByUserName(string userName)
        {
            throw new NotImplementedException();
        }



        public void RemoveUserFromRole(string userName)
        {
            throw new NotImplementedException();
        }

        public void EditRoleForUser(string userName, string roleName)
        {
            throw new NotImplementedException();
        }


        public Role GetRoleByRoleId(long roleId)
        {
            return _roles.Find(roleId);
        }
    }
}
