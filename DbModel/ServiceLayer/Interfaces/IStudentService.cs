using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ViewModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Interfaces
{
    public interface IStudentService
    {
        Student GetById(long id);
        void Insert(Student article);
        void Delete(long id);
        void Update(EditStudentViewModel viewModel);
        void UpdateUser(EditStudentViewModel viewmodel);
        EditStudentViewModel GetForEdit(long id);
        IEnumerable<StudentViewModel> GetDataTable(out int total, string term, int page,
          Order order, StudentSearchBy slectionSearchBy, int count = 10);
        bool CheckSNO_Exist(string sno);
        Student GetByUserId(int Userid);
    }
}
