using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Term;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Service
{
    public class TermService : ITermService
    {
        //#region Fields
        private readonly IDbSet<Term> _Term;
        private readonly IUnitOfWork _unitOfWork;
        //#endregion //Fields
        public TermService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Term = _unitOfWork.Set<Term>();
        }

        public Term GetById(int id)
        {
            return _Term.Find(id);
        }

        public Term GetByTermName(string name)
        {
            return _Term.Where(x=>x.Name.Equals(name)).FirstOrDefault();
        }

        public void Insert(Term article)
        {
            _Term.Add(article);
        }

        public void Delete(int id)
        {
            _Term.Where(a => a.Id.Equals(id)).Delete();
        }

        public void Update(EditTermViewModel viewModel)
        {
            var obj = GetById(viewModel.Id);
            obj.Name = viewModel.Name;
            obj.StartDate = viewModel.StartDate;
            obj.EndDate = viewModel.EndDate;
        }
        public IList<Term> GetAllTerms()
        {
            return _Term.AsNoTracking().ToList();
        }

        public EditTermViewModel GetForEdit(int id)
        {
            return _Term.Where(a => a.Id.Equals(id)).Select(a => new EditTermViewModel
            {
                Id = a.Id,
                Name = a.Name,
                EndDate = a.EndDate,
                StartDate = a.StartDate, 


                PeresentedCourcess = a.PeresentedCourcess
            }).FirstOrDefault();
        }
        public IEnumerable<TermViewModel> GetDataTable(out int total, string term, int page,
          Order order, TermSearchBy slectionSearchBy, int count = 10)
        {
            var selectedobj = _Term.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                switch (slectionSearchBy)
                {
                    case TermSearchBy.Name:
                        selectedobj = selectedobj.Where(a => a.Name.Contains(term)).AsQueryable(); break;
                    case TermSearchBy.StartDate:
                        selectedobj = selectedobj.Where(a => a.StartDate.ToShortDateString().Contains(term)).AsQueryable(); break;
                    case TermSearchBy.EndDate:
                        selectedobj = selectedobj.Where(a => a.EndDate.ToShortDateString().Contains(term)).AsQueryable(); break;
                }
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
                .Select(a => new TermViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    EndDate = a.EndDate,
                    StartDate = a.StartDate,
                    PeresentedCourcess = a.PeresentedCourcess
                }).Future();
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }
    }
}
