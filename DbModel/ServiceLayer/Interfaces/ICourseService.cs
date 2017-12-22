using DbModel.DomainClasses.Entities;
using DbModel.ViewModel.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Interfaces
{
    public interface ICourseService
    {
        Course GetById(int id);
        void Insert(Course article);
        void Delete(int id);
        void Update(EditCourseViewModel viewModel);
        EditCourseViewModel GetForEdit(int id);
        IEnumerable<Course> GetSecondLevelCourses();
        IList<Course> GetFirstLevelCourses();
        IEnumerable<CourseViewModel> GetDataTable(out int total, string term, int page,
            int count = 10);
        IList<Course> GetAllCourses();
    }
}
