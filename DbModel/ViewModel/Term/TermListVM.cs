using DbModel.DomainClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Term
{
    public class TermListVM
    {
        public IEnumerable<TermViewModel> TermsList { get; set; }
        public string Term { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public DomainClasses.Enums.Order Order { get; set; }
        public TermSearchBy TermSearchBy { get; set; }
        public int TotalTerms { get; set; }
    }
}
