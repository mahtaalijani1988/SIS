using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.GroupCourse
{
    public class GroupCourseListVM
    {
        public IEnumerable<GroupCourseViewModel> GroupCourseList { get; set; }
        public string Term { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public DomainClasses.Enums.Order Order { get; set; }
        public int TotalGroupCourses { get; set; }
    }
}
