using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ViewModel.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Interfaces
{
    public interface IGroupsService
    {
        Groups GetById(int id);
        void Insert(Groups article);
        void Delete(int id);
        void Update(EditGroupsViewModel viewModel);
        EditGroupsViewModel GetForEdit(int id);
        IEnumerable<GroupsViewModel> GetDataTable(out int total, string term, int page,
          Order order, GroupsSearchBy slectionSearchBy, int count = 10);
        IList<Groups> GetAllGroups();
    }
}
