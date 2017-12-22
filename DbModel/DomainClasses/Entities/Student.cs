using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DbModel.DomainClasses.Enums;
using System.ComponentModel.DataAnnotations;

namespace DbModel.DomainClasses.Entities
{
    public class Student
    {
        public virtual long Id { get; set; }

        [Index(IsUnique = true)]
        public virtual string SNO { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string AvatarPath { get; set; }
        public virtual DateTime? BirthDay { get; set; }

        public virtual bool? Gender { get; set; }
        public virtual string City { get; set; }
        public virtual decimal? Average { get; set; } 
        

        public virtual User User { get; set; }

        public virtual ICollection<Election> Elections { get; set; }
    }
}

