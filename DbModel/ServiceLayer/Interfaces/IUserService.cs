using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DbModel.ViewModel;
using DbModel.ViewModel.Student;
using DbModel.ViewModel.Professor;
using DbModel.DomainClasses.Enums;
using DbModel.DomainClasses.Entities;
using DbModel.ViewModel.User;

namespace DbModel.ServiceLayer.Interfaces
{
    public class UserIdAndUserName
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    public class UserStatus
    {
        public bool IsBaned { get; set; }
        public string Role { get; set; }
    }
    public interface IUserService
    {
        string GeneratePassword();
        AddUserStatus Add(User user);
        void Remove(int id);
        VerifyUserStatus VerifyUserByUserName(string username, string password, ref string correctUserName, ref int userId, string ip);
        ChangePasswordResult ChangePasswordByUserName(string userName, string oldPassword, string newPassword);
        ChangePasswordResult ChangePasswordByUserId(int Id, string oldPassword, string newPassword);
        void DeActiveUser(int id);
        void ActiveUser(int id);
        User GetUserById(int id);
        User GetUserByUserName(string userName);
        IList<User> GetAllUsers();
        IList<User> GetUser(Expression<Func<User, bool>> expression);
        bool ExistsByUserName(string userName);
        bool ExistsByUserName(string userName, int id);
        bool IsUserActive(int id);

        DetailsUserViewModel GetUserDetail(int id);
        int GetUsersNumber();
        IList<string> SearchUserName(string userName);
        IList<string> SearchUserId(string userId);
        Role GetUserRole(int userId);
        EditUserViewModel GetUserDataForEdit(int userId);
        ProfileViewModel GetUserDataForUpdateProfile(int userId);
        EditedUserStatus EditUser(User user);

        IList<UserViewModel> GetDataTable(out int total, string term, int page, int count,
            DomainClasses.Enums.Order order, UserOrderBy orderBy, UserSearchBy searchBy);

        User Find(string userName);
        IList<string> GetUsersPhoneNumbers();

        User GetUserByEmail(string email);
       
        string GetRoleByUserName(string userName);
        
        IList<string> SearchByUserName(string userName);
        IList<string> SearchByRoleDescription(string roleDescription);
        IList<string> SearchByFirstName(string firstName);
        IList<string> SearchByLastName(string lastName);
        IList<string> SearchByPhoneNumber(string phoneNumber);
        IList<string> SearchByIP(string ip);

        EditedUserStatus UpdateProfile(ProfileViewModel viewModel);
        bool Authenticate(string phoneNumber, string password);
        bool IsBaned(string userName);
        UserStatus GetStatus(string userName);
        IList<UserIdAndUserName> SearchUser(string userName);

        void RemoveByStudentInfo(long Student_id);
        void RemoveByProfessorInfo(long Professor_id);
    }
}
