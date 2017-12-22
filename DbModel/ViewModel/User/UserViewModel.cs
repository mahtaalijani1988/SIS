
using System;

namespace DbModel.ViewModel.User
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string  RoleDescritpion { get; set; }
        public bool Baned  { get; set; }
        public string RegisterType { get; set; }

        
        public DateTime? RegisterDate { get; set; }
        public DateTime? BanedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string Email { get; set; }
        public string IP { get; set; }

        public virtual DomainClasses.Entities.Professor ProfessorData { get; set; }
        public virtual DomainClasses.Entities.Student StudentData { get; set; }
    }
}
