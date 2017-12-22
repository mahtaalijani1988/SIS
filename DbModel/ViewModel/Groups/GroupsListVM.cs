using DbModel.DomainClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Groups
{
    public class GroupsListVM
    {
        public IEnumerable<GroupsViewModel> GroupsList { get; set; }
        public string Term { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public DomainClasses.Enums.Order Order { get; set; }
        public GroupsSearchBy GroupsSearchBy { get; set; }
        public int TotalGroups { get; set; }
    }
}
