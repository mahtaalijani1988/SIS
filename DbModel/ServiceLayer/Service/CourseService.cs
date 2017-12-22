using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Course;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Service
{
    public class CourseService : ICourseService
    {
        //#region Fields
        private readonly IDbSet<Course> _Cource;
        private readonly IUnitOfWork _unitOfWork;
        //#endregion //Fields
        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Cource = _unitOfWork.Set<Course>();
        }

        public Course GetById(int id)
        {
            return _Cource.Find(id);
        }

        public void Insert(Course article)
        {
            _Cource.Add(article);
        }
        public void Delete(int id)
        {
            _Cource.Where(a => a.Id.Equals(id)).Delete();
        }

        public void Update(EditCourseViewModel viewModel)
        {
            var obj = GetById(viewModel.Id);
            obj.Name = viewModel.Name;
            obj.Parent = viewModel.Parent;
            obj.Parent_id = viewModel.Parent_id;
            obj.Unit = viewModel.Unit;
            
        }
        public EditCourseViewModel GetForEdit(int id)
        {
            return _Cource.Where(a => a.Id.Equals(id)).Select(a => new EditCourseViewModel
            {
                Id = a.Id, 
                Name = a.Name,
                Parent = a.Parent,
                Parent_id = a.Parent_id,
                Unit = a.Unit
            }).FirstOrDefault();
        }

        public IEnumerable<Course> GetSecondLevelCourses()
        {
            return
                _Cource.AsNoTracking()
                    .Where(a => a.Parent_id != null)
                    .ToList();
        }
        public IList<Course> GetFirstLevelCourses()
        {
            return
                _Cource.AsNoTracking()
                    .Where(a => a.Parent_id == null)
                    .ToList();
        }
        public IList<Course> GetAllCourses()
        {
            return _Cource.AsNoTracking().ToList();
        }

        public IEnumerable<CourseViewModel> GetDataTable(out int total, string term, int page,
            int count = 10)
        {
            var selectedCategories = _Cource.AsNoTracking().OrderBy(a => a.Id).AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                selectedCategories = selectedCategories.Where(a => a.Name.Contains(term));
            }

            var totalQuery = selectedCategories.FutureCount();
            var query = selectedCategories.Skip((page - 1) * count).Take(count).Select(a => new CourseViewModel
            {
                Name = a.Name,
                Id = a.Id,
                Parent = a.Parent,
                Parent_id = a.Parent_id,
                Unit = a.Unit
            }).Future();
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }
    }
}
