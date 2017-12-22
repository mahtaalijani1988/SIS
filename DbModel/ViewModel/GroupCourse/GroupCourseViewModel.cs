using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.GroupCourse
{
    public class GroupCourseViewModel
    {
        public virtual int Id { get; set; }
        public virtual DomainClasses.Entities.Course Course { get; set; }

        public virtual int? Course_Id { get; set; }
    }
}
