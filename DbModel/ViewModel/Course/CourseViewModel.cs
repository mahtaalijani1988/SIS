using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Course
{
    public class CourseViewModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual byte Unit { get; set; }

        public virtual DomainClasses.Entities.Course Parent { get; set; }
        public virtual int? Parent_id { get; set; }

        public virtual ICollection<DomainClasses.Entities.Course> Children { get; set; }

        public virtual ICollection<DomainClasses.Entities.GroupCourses> GroupCourcess { get; set; }
        public virtual ICollection<DomainClasses.Entities.PeresentedCourses> PeresentedCources { get; set; }
    }
}
