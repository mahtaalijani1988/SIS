using DbModel.DomainClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Election
{
    public class ElectionListVM
    {
        public IEnumerable<ElectionViewModel> ElectionList { get; set; }
        public string Term { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public Order Order { get; set; }
        public ElectionOrderBy ElectionOrderBy { get; set; }
        public ElectionSearchBy ElectionSearchBy { get; set; }
        public int TotalElections { get; set; }
    }
}
