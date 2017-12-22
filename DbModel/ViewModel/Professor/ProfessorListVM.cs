using DbModel.DomainClasses.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Professor
{
    public class ProfessorListVM
    {
        public IEnumerable<ProfessorViewModel> ProfessorList { get; set; }
        public string Term { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public DomainClasses.Enums.Order Order { get; set; }
        public ProfessorSearchBy ProfessorSearchBy { get; set; }
        public int TotalProfessors { get; set; }
    }
}
