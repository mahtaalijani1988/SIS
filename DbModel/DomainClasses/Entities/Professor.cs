using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.DomainClasses.Entities
{
    public class Professor
    {
        public virtual long Id { get; set; }

        [Index(IsUnique = true)]
        public virtual string PNO { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string AvatarPath { get; set; }
        public virtual DateTime? BirthDay { get; set; }

        public virtual bool? Gender { get; set; }
        public virtual string Tendency { get; set; }
        public virtual string Edution { get; set; } 
        

        public virtual User User { get; set; }
        public virtual Groups Group { get; set; }

        public virtual ICollection<PeresentedCourses> PeresentedCourses { get; set; }
    }
}
