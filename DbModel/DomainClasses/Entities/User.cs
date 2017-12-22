using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using DbModel.DomainClasses.Enums;

namespace DbModel.DomainClasses.Entities
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual UserRegisterType RegisterType { get; set; }
        public virtual bool IsBaned { get; set; }
        public virtual DateTime RegisterDate { get; set; }
        public virtual DateTime? BanedDate { get; set; }
        public virtual DateTime? LastLoginDate { get; set; }
        public virtual Role Role { get; set; }
        public virtual byte[] RowVersion { get; set; }
        public virtual string IP { get; set; }

        [Index(IsUnique = true)]
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }

        public virtual Professor ProfessorData { get; set; }
        public virtual Student StudentData { get; set; }
    }

}
