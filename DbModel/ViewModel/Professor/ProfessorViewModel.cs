using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Professor
{
    public class ProfessorViewModel
    {
        public virtual long Id { get; set; }
        
        public virtual string PNO { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string AvatarPath { get; set; }
        public virtual DateTime? BirthDay { get; set; }

        public virtual bool? Gender { get; set; }
        public virtual string Tendency { get; set; }
        public virtual string Edution { get; set; }


        public virtual DomainClasses.Entities.User User { get; set; }
        public virtual DomainClasses.Entities.Groups Group { get; set; }
        public virtual int? Group_Id { get; set; }

        public virtual ICollection<DomainClasses.Entities.PeresentedCourses> PeresentedCourcess { get; set; }
    }
}
