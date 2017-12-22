using DbModel.DomainClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.PeresentedCourses
{
    public class PeresentedCoursesListVM
    {
        public IEnumerable<PeresentedCoursesViewModel> PeresentedCoursesList { get; set; }
        public string Term { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public DomainClasses.Enums.Order Order { get; set; }
        public PeresentedCoursesOrderBy PeresentedCoursesOrderBy { get; set; }
        public PeresentedCoursesSearchBy PeresentedCoursesSearchBy { get; set; }
        public int TotalPeresentedCourses { get; set; }
    }
}
