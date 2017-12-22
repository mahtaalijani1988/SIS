using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.DomainClasses.Entities
{
    public class Course
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual byte Unit { get; set; }

        public virtual Course Parent { get; set; }
        public virtual int? Parent_id { get; set; }

        public virtual ICollection<Course> Children { get; set; }

        public virtual ICollection<GroupCourses> GroupCourcess { get; set; }
        public virtual ICollection<PeresentedCourses> PeresentedCources { get; set; }
    }
}
