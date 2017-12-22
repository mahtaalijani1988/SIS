using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Groups;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Service
{
    public class GroupsService : IGroupsService
    {
        //#region Fields
        private readonly IDbSet<Groups> _Groups;
        private readonly IUnitOfWork _unitOfWork;
        //#endregion //Fields
        public GroupsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Groups = _unitOfWork.Set<Groups>();
        }

        public Groups GetById(int id)
        {
            return _Groups.Find(id);
        }

        public void Insert(Groups article)
        {
            _Groups.Add(article);
        }
        public void Delete(int id)
        {
            _Groups.Where(a => a.Id.Equals(id)).Delete();
        }

        public void Update(EditGroupsViewModel viewModel)
        {
            var obj = GetById(viewModel.Id);
            obj.Name = viewModel.Name;
            obj.Manager = viewModel.Manager;
        }
        public EditGroupsViewModel GetForEdit(int id)
        {
            return _Groups.Where(a => a.Id.Equals(id)).Select(a => new EditGroupsViewModel
            {
                Id = a.Id,
                Manager = a.Manager,
                Name = a.Name,

                Professors = a.Professors
            }).FirstOrDefault();
        }
        public IList<Groups> GetAllGroups()
        {
            return _Groups.AsNoTracking().ToList();
        }

        public IEnumerable<GroupsViewModel> GetDataTable(out int total, string term, int page,
          Order order, GroupsSearchBy slectionSearchBy, int count = 10)
        {
            var selectedobj = _Groups
                .Include(x=>x.Professors)
                .AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                switch (slectionSearchBy)
                {
                    case GroupsSearchBy.Manager:
                        selectedobj = selectedobj.Where(a => a.Manager.Contains(term)).AsQueryable(); break;
                    case GroupsSearchBy.Name:
                        selectedobj = selectedobj.Where(a => a.Name.Contains(term)).AsQueryable(); break;
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
                .Select(a => new GroupsViewModel
                {
                    Id = a.Id,
                    Manager = a.Manager,
                    Name = a.Name,
                    Professors = a.Professors
                }).Future();
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }
    }
}
