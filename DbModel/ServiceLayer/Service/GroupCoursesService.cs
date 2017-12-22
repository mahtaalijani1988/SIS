using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.GroupCourse;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Service
{
    public class GroupCoursesService : IGroupCoursesService
    {
        //#region Fields
        private readonly IDbSet<GroupCourses> _GroupCources;
        private readonly IUnitOfWork _unitOfWork;
        //#endregion //Fields
        public GroupCoursesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _GroupCources = _unitOfWork.Set<GroupCourses>();
        }

        public GroupCourses GetById(int id)
        {
            return _GroupCources.Find(id);
        }

        public void Insert(GroupCourses article)
        {
            _GroupCources.Add(article);
        }
        public void Delete(int id)
        {
            _GroupCources.Where(a => a.Id.Equals(id)).Delete();
        }

        public void Update(EditGroupCourseViewModel viewModel)
        {
            var obj = GetById(viewModel.Id);
            obj.Course = viewModel.Course;
        }
        public EditGroupCourseViewModel GetForEdit(int id)
        {
            return _GroupCources.Where(a => a.Id.Equals(id)).Select(a => new EditGroupCourseViewModel
            {
                Id = a.Id,
                Course = a.Course,

                Course_Id = a.Course.Id
                //Course_Id = a.
            }).FirstOrDefault();
        }
        public IEnumerable<GroupCourseViewModel> GetDataTable(out int total, string term, int page,
          Order order, int count = 10)
        {
            var selectedobj = _GroupCources
                .Include(x=>x.Course)
                .AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                selectedobj = selectedobj.Where(a => a.Course.Name.Contains(term)).AsQueryable();
            }
            if (order == Order.Asscending)
            {
                selectedobj = selectedobj.OrderBy(x => x.Id).AsQueryable();
            }
            else
            {
                selectedobj = selectedobj.OrderByDescending(x => x.Id).AsQueryable();
            }

            var totalQuery = selectedobj.FutureCount();
            var query = selectedobj.Skip((page - 1) * count).Take(count)
                .Select(a => new GroupCourseViewModel
                {
                    Id = a.Id,
                    Course = a.Course,

                    Course_Id = a.Course.Id
                }).Future();
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }
    }
}
