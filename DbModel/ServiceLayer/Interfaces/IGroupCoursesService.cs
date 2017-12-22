using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ViewModel.GroupCourse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Interfaces
{
    public interface IGroupCoursesService
    {
        GroupCourses GetById(int id);
        void Insert(GroupCourses article);
        void Delete(int id);
        void Update(EditGroupCourseViewModel viewModel);
        EditGroupCourseViewModel GetForEdit(int id);
        IEnumerable<GroupCourseViewModel> GetDataTable(out int total, string term, int page,
          Order order, int count = 10);
    }
}
