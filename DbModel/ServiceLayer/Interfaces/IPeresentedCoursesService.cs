using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ViewModel.PeresentedCourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Interfaces
{
    public interface IPeresentedCoursesService
    {
        PeresentedCourses GetById(int id);
        void Insert(PeresentedCourses article);
        void Delete(int id);
        void Update(EditPeresentedCoursesViewModel viewModel);
        EditPeresentedCoursesViewModel GetForEdit(int id);
        IEnumerable<PeresentedCoursesViewModel> GetDataTable(out int total, string term, int page,
          Order order, PeresentedCoursesSearchBy slectionSearchBy, PeresentedCoursesOrderBy slectionOrderBy, int count = 10);
        IEnumerable<PeresentedCourses> GetByTermName(string tname);
        void Increase_Capacity_Remained(int id);
        void Decrease_Capacity_Remained(int id);
    }
}
