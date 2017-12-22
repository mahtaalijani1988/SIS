using DbModel.DomainClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Student
{
    public class StudentsListVM
    {
        public IEnumerable<StudentViewModel> StudentList { get; set; }
        public string Term { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public DomainClasses.Enums.Order Order { get; set; }
        public StudentSearchBy StudentSearchBy { get; set; }
        public int TotalStudents { get; set; }
    }
}
