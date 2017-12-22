using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ViewModel.Professor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Interfaces
{
    public interface IProfessorService
    {
        Professor GetById(long id);
        void Insert(Professor article);
        void Delete(long id);
        void Update(EditProfessorViewModel viewModel);
        EditProfessorViewModel GetForEdit(long id);
        IEnumerable<ProfessorViewModel> GetDataTable(out int total, string term, int page,
          Order order, ProfessorSearchBy slectionSearchBy, int count = 10);
        IList<Professor> GetAllProfessors();
        bool CheckPNO_Exist(string pno);

        Professor GetByUserId(int Userid);
    }
}
