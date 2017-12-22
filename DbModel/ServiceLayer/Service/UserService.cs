using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DbModel.ServiceLayer.Interfaces;
using DbModel.DomainClasses.Enums;
using DbModel.Context;
using System.Data.Entity;
using DbModel.DomainClasses.Entities;
using DbModel.Utilities.Security;
using System.Globalization;
using EntityFramework.Extensions;
using DbModel.ViewModel;
using System.Linq.Expressions;
using DbModel.ViewModel.Student;
using DbModel.ViewModel.Professor;
using DbModel.ViewModel.User;

namespace DbModel.ServiceLayer.EFServices
{
    public class UserService : IUserService
    {
        //#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<User> _users;

        //#endregion //Fields

        //#region Cosntructor
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _users = _unitOfWork.Set<User>();
        }
        //#endregion //Constructor

        //#region Private Members
        private static VerifyUserStatus Verify(User selectedUser, string password)
        {
            var result = VerifyUserStatus.VerifiedFaild;

            bool verifiedPassword = Encryption.VerifyPassword(password, selectedUser.Password);

            if (verifiedPassword)
            {
                if (selectedUser.IsBaned)
                {
                    result = VerifyUserStatus.UserIsBaned;
                }
                else
                {
                    selectedUser.LastLoginDate = DateTime.Now;
                    result = VerifyUserStatus.VerifiedSuccessfully;
                }
            }

            return result;
        }

        //#endregion // Private Members

        //#region CRUD
        public EditedUserStatus EditUser(User user)
        {
            if (ExistsByUserName(user.UserName, user.Id)) return EditedUserStatus.UserNameExist;
            var selectedUser = GetUserById(user.Id);
            if (user.Password != null) selectedUser.Password = user.Password;
            selectedUser.UserName = user.UserName;
            selectedUser.IsBaned = user.IsBaned;
            selectedUser.Role = user.Role;
            
            selectedUser.BanedDate = user.BanedDate;
            selectedUser.Email = user.Email;
            selectedUser.IP = user.IP;
            return EditedUserStatus.UpdatingUserSuccessfully;
        }

        public IList<UserViewModel> GetDataTable(out int total, string term, int page, int count, DomainClasses.Enums.Order order, UserOrderBy orderBy, UserSearchBy searchBy)
        {
            var selectedUsers = _users.AsNoTracking().Include(a => a.Role).AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                switch (searchBy)
                {
                    case UserSearchBy.UserName:
                        selectedUsers = selectedUsers.Where(user => user.UserName.Contains(term)).AsQueryable();
                        break;
                    case UserSearchBy.RoleDescription:
                        selectedUsers = selectedUsers.Where(user => user.Role.Description.Contains(term)).AsQueryable();
                        break;
                    case UserSearchBy.Ip:
                        selectedUsers =
                            selectedUsers.Where(user => user.IP.Contains(term)).AsQueryable();
                        break;
                }
            }


            if (order == DomainClasses.Enums.Order.Asscending)
            {
                switch (orderBy)
                {
                    case UserOrderBy.UserName:
                        selectedUsers = selectedUsers.OrderBy(user => user.UserName).AsQueryable();
                        break;
                    case UserOrderBy.RegisterDate:
                        selectedUsers = selectedUsers.OrderBy(user => user.RegisterDate).AsQueryable();
                        break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case UserOrderBy.UserName:
                        selectedUsers = selectedUsers.OrderByDescending(user => user.UserName).AsQueryable();
                        break;
                    case UserOrderBy.RegisterDate:
                        selectedUsers = selectedUsers.OrderByDescending(user => user.RegisterDate).AsQueryable();
                        break;
                }
            }
            var totalQuery = selectedUsers.FutureCount();
            var selectQuery = selectedUsers.Skip((page - 1) * count).Take(count)
                .Select(a => new UserViewModel
                {
                    RegisterType = a.RegisterType == UserRegisterType.Active ? "Active" : "DeActie",
                    Baned = a.IsBaned,
                    Id = a.Id,
                    RoleDescritpion = a.Role.Description,
                    
                    BanedDate = a.BanedDate,
                    Email = a.Email,
                    IP = a.IP,
                    LastLoginDate  = a.LastLoginDate,
                    RegisterDate = a.RegisterDate,
                    UserName = a.UserName,

                    ProfessorData = a.ProfessorData,
                    StudentData = a.StudentData
                }).Future();
            total = totalQuery.Value;
            var users = selectQuery.ToList();
            return users;
        }


        public AddUserStatus Add(User user)
        {
            if (ExistsByEmail(user.Email)) return AddUserStatus.EmailExist;
            if (ExistsByUserName(user.UserName)) return AddUserStatus.UserNameExist;
            _users.Add(user);
            return AddUserStatus.AddingUserSuccessfully;
        }

        public DetailsUserViewModel GetUserDetail(int id)
        {
            return
                _users.Where(x => x.Id == id)
                    .Include(x => x.Role)
                    .Select(
                        x =>
                            new DetailsUserViewModel
                            {
                                RegisterType =
                                    x.RegisterType == UserRegisterType.Active
                                        ? "User Logged in"
                                        : "User Not Loggin",
                                RoleName = x.Role.Description,
                                UserName = x.UserName,
                                BanedDate = x.BanedDate,
                                Email = x.Email
                            })
                    .FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return _users.Where(x=>x.Email == email).FirstOrDefault();
        }
        
        public bool ExistsByUserName(string userName)
        {
            return
                _users.Any(
                    user => user.UserName == userName);
        }

        public bool ExistsByEmail(string email)
        {
            return
                _users.Any(
                    user => user.Email == email);
        }
        
        public bool ExistsByUserName(string userName, int id)
        {
            return
                _users.Any(
                    user => user.Id != id && user.UserName == userName);
        }
        
        public string GeneratePassword()
        {
            var _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            int _passwordLength = 6;
            char[] chars = new char[_passwordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < _passwordLength; i++)
            {
                chars[i] = _allowedChars[(int)((allowedCharCount) * randNum.NextDouble())];
            }
            return new string(chars);
        }
        
        public VerifyUserStatus VerifyUserByUserName(string username, string password, ref string correctUserName, ref int userId, string ip)
        {
            User selectedUser = _users.SingleOrDefault(x => x.UserName == username);
            var result = VerifyUserStatus.VerifiedFaild;
            if (selectedUser != null)
            {
                result = Verify(selectedUser, password);
                if (result == VerifyUserStatus.VerifiedSuccessfully)
                {
                    correctUserName = selectedUser.UserName;
                    userId = selectedUser.Id;
                    selectedUser.IP = ip;
                    selectedUser.RegisterType = UserRegisterType.Active;
                }
            }
            return result;
        }


        public ChangePasswordResult ChangePasswordByUserName(string userName, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public ChangePasswordResult ChangePasswordByUserId(int Id, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void DeActiveUser(int id)
        {
            throw new NotImplementedException();
        }

        public void DeActiveUsers(int[] usersId)
        {
            throw new NotImplementedException();
        }

        public void ActiveUser(int id)
        {
            throw new NotImplementedException();
        }

        public void ActiveUsers(int[] usersId)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(int id)
        {
            return _users.Find(id);
        }

        public User GetUserByUserName(string userName)
        {
            return _users.FirstOrDefault(a => a.UserName == userName);
        }
        
        public IList<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public IList<User> GetUser(Expression<Func<User, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public bool IsUserActive(int id)
        {
            throw new NotImplementedException();
        }

        public int GetUsersNumber()
        {
            throw new NotImplementedException();
        }

        public IList<string> SearchUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public IList<string> SearchUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Role GetUserRole(int userId)
        {
            throw new NotImplementedException();
        }

        public EditUserViewModel GetUserDataForEdit(int userId)
        {
            return _users.Include(a => a.Role).Where(a => a.Id == userId)
                .Select(a => new EditUserViewModel
                {
                    UserName = a.UserName,
                    Id = a.Id,
                    RoleId = a.Role.Id,
                    IsBaned = a.IsBaned,
                    Email = a.Email
                }).FirstOrDefault();
        }

        public User Find(string userName)
        {
            return _users.FirstOrDefault(a => a.UserName == userName);
        }

        public IList<UserIdAndUserName> SearchUser(string userName)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetUsersPhoneNumbers()
        {
            throw new NotImplementedException();
        }

        public string GetRoleByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public string GetRoleByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public string GetUserNameByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public IList<string> SearchByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public IList<string> SearchByRoleDescription(string roleDescription)
        {
            throw new NotImplementedException();
        }

        public IList<string> SearchByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public IList<string> SearchByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public IList<string> SearchByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public IList<string> SearchByIP(string ip)
        {
            throw new NotImplementedException();
        }

        public EditedUserStatus UpdateProfile(ProfileViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public bool Authenticate(string phoneNumber, string password)
        {
            throw new NotImplementedException();
        }

        public bool IsBaned(string userName)
        {
            throw new NotImplementedException();
        }


        public UserStatus GetStatus(string userName)
        {
            return _users.AsNoTracking()
                .Where(user => user.UserName == userName)
                .Select(user => new UserStatus { IsBaned = user.IsBaned, Role = user.Role.Name })
                .Single();
        }
        


        public ProfileViewModel GetUserDataForUpdateProfile(int userId)
        {
            throw new NotImplementedException();
        }


        private void RemoveUserComments(int userId)
        {

        }

        public void Remove(int id)
        {
            var user = _users.Include(a => a.Role).Where(a => a.Id == id && a.Role.Name != "admin").Delete();
        }

        public void RemoveByStudentInfo(long Student_id)
        {
            var user = _users.Include(a => a.Role).Include(x=>x.StudentData)
                .Where(a => a.StudentData.Id == Student_id && a.Role.Name != "admin").Delete();
        }
        public void RemoveByProfessorInfo(long Professor_id)
        {
            var user = _users.Include(a => a.Role).Include(x=>x.ProfessorData)
                .Where(a => a.ProfessorData.Id == Professor_id && a.Role.Name != "admin").Delete();
        }
        
    }
}
