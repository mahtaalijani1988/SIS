using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ViewModel.Term;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Interfaces
{
    public interface ITermService
    {
        Term GetById(int id);
        void Insert(Term article);
        void Delete(int id);
        void Update(EditTermViewModel viewModel);
        EditTermViewModel GetForEdit(int id);
        IEnumerable<TermViewModel> GetDataTable(out int total, string term, int page,
          Order order, TermSearchBy slectionSearchBy, int count = 10);
        IList<Term> GetAllTerms();
        Term GetByTermName(string name);
    }
}
