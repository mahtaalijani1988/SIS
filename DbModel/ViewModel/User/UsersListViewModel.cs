using System;
using System.Collections.Generic;
using DbModel.DomainClasses.Enums;

namespace DbModel.ViewModel.User
{
    public class UsersListViewModel
    {
        public IEnumerable<UserViewModel> UsersList { get; set; }
        public int PageCount { get; set; }
        public DomainClasses.Enums.Order Order { get; set; }
        public string Term { get; set; }
        public UserOrderBy UserOrderBy { get; set; }
        public int PageNumber { get; set; }
        public int TotalUsers { get; set; }
    }
}
