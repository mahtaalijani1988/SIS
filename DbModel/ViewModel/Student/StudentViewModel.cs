using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Student
{
    public class StudentViewModel
    {
        public virtual long Id { get; set; }
        
        public virtual string SNO { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string AvatarPath { get; set; }
        public virtual DateTime? BirthDay { get; set; }

        public virtual bool? Gender { get; set; }
        public virtual string City { get; set; }
        public virtual decimal? Average { get; set; }


        public virtual DomainClasses.Entities.User User { get; set; }
    }
}
