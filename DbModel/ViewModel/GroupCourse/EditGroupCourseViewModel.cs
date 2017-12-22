using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.GroupCourse
{
    public class EditGroupCourseViewModel
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Course is Required.")]
        public virtual DomainClasses.Entities.Course Course { get; set; }
        public virtual int? Course_Id { get; set; }
    }
}
