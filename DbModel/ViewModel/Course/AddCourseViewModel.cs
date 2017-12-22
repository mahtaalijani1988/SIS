using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Course
{
    public class AddCourseViewModel
    {
        [Required(ErrorMessage = "Name is Required.")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Unit is Required.")]
        [Range(0, 9, ErrorMessage = "Unit has to be between 0 and 9")]
        public virtual byte Unit { get; set; } 

        public virtual DomainClasses.Entities.Course Parent { get; set; }
        public virtual int? Parent_id { get; set; }

        public virtual ICollection<DomainClasses.Entities.Course> Children { get; set; }

        public virtual ICollection<DomainClasses.Entities.GroupCourses> GroupCourcess { get; set; }
        public virtual ICollection<DomainClasses.Entities.PeresentedCourses> PeresentedCources { get; set; }
    }
}
