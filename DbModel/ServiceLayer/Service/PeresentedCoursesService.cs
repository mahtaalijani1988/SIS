using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.PeresentedCourses;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Service
{
    public class PeresentedCoursesService : IPeresentedCoursesService
    {
        //#region Fields
        private readonly IDbSet<PeresentedCourses> _PeresentedCources;
        private readonly IUnitOfWork _unitOfWork;
        //#endregion //Fields
        public PeresentedCoursesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _PeresentedCources = _unitOfWork.Set<PeresentedCourses>();
        }

        public PeresentedCourses GetById(int id)
        {
            return _PeresentedCources.Find(id);
        }
        public void Increase_Capacity_Remained(int id)
        {
            var pr = GetById(id);
            if (pr == null) return;
            pr.Remain_Capacity += 1;
        }
        public void Decrease_Capacity_Remained(int id)
        {
            var pr = GetById(id);
            if (pr == null) return;
            pr.Remain_Capacity -= 1;
        }
        public void Insert(PeresentedCourses article)
        {
            _PeresentedCources.Add(article);
        }
        public void Delete(int id)
        {
            _PeresentedCources.Where(a => a.Id.Equals(id)).Delete();
        }
        public IEnumerable<PeresentedCourses> GetByTermName(string tname)
        {
            return _PeresentedCources.Where(x => x.Term.Name == tname).ToList();
        }
        public void Update(EditPeresentedCoursesViewModel viewModel)
        {
            var obj = GetById(viewModel.Id);
            obj.Capacity = viewModel.Capacity;
            obj.Course = viewModel.Course;
            obj.Professor = viewModel.Professor;
            obj.Remain_Capacity = viewModel.Remain_Capacity;
            obj.Term = viewModel.Term;
        }
        public EditPeresentedCoursesViewModel GetForEdit(int id)
        {
            return _PeresentedCources.Where(a => a.Id.Equals(id)).Select(a => new EditPeresentedCoursesViewModel
            {
                Id = a.Id,
                Professor = a.Professor,
                Professor_Id = a.Professor.Id,
                Capacity = a.Capacity,
                Course = a.Course,
                Course_Id = a.Course.Id,
                Remain_Capacity = a.Remain_Capacity,
                Term = a.Term,
                Term_Id = a.Term.Id,
                 
                Elections = a.Elections
            }).FirstOrDefault();
        }
        public IEnumerable<PeresentedCoursesViewModel> GetDataTable(out int total, string term, int page,
          Order order, PeresentedCoursesSearchBy slectionSearchBy, PeresentedCoursesOrderBy slectionOrderBy, int count = 10)
        {
            var selectedobj = _PeresentedCources
                .Include(x=>x.Course)
                .Include(x=>x.Professor)
                .Include(x=>x.Term)
                .AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                switch (slectionSearchBy)
                {
                    case PeresentedCoursesSearchBy.Cource_Name:
                        selectedobj = selectedobj.Where(a => a.Course.Name.Contains(term)).AsQueryable(); break;
                    case PeresentedCoursesSearchBy.Term:
                        selectedobj = selectedobj.Where(a => a.Term.Name.Contains(term)).AsQueryable(); break;
                    case PeresentedCoursesSearchBy.Professor_Name:
                        selectedobj = selectedobj.Where(a => a.Professor.FirstName.Contains(term) ||
                                a.Professor.LastName.Contains(term)).AsQueryable(); break;
                }
            }
            if (order == Order.Asscending)
            {
                switch (slectionOrderBy)
                {
                    case PeresentedCoursesOrderBy.Id:
                        selectedobj = selectedobj.OrderBy(x => x.Id).AsQueryable(); break;
                    case PeresentedCoursesOrderBy.Cource:
                        selectedobj = selectedobj.OrderBy(x => x.Course.Id).AsQueryable(); break;
                    case PeresentedCoursesOrderBy.Term:
                        selectedobj = selectedobj.OrderBy(x => x.Term.Id).AsQueryable(); break;
                }
            }
            else
            {
                switch (slectionOrderBy)
                {
                    case PeresentedCoursesOrderBy.Id:
                        selectedobj = selectedobj.OrderByDescending(x => x.Id).AsQueryable(); break;
                    case PeresentedCoursesOrderBy.Cource:
                        selectedobj = selectedobj.OrderByDescending(x => x.Course.Id).AsQueryable(); break;
                    case PeresentedCoursesOrderBy.Term:
                        selectedobj = selectedobj.OrderByDescending(x => x.Term.Id).AsQueryable(); break;
                }
            }

            var totalQuery = selectedobj.FutureCount();
            var query = selectedobj.Skip((page - 1) * count).Take(count)
                .Select(a => new PeresentedCoursesViewModel
                {
                    Id = a.Id,
                    Course = a.Course,
                    Elections = a.Elections,
                    Professor = a.Professor,
                    Term = a.Term,
                    Capacity = a.Capacity,
                    Remain_Capacity = a.Remain_Capacity,

                    Course_Id = a.Course.Id,
                    Professor_Id = a.Professor.Id,
                    Term_Id = a.Term.Id
                }).Future();
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }
    }
}
