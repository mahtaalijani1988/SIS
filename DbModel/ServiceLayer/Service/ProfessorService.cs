using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Professor;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Service
{
    public class ProfessorService : IProfessorService
    {

        //#region Fields
        private readonly IDbSet<Professor> _Professor;
        private readonly IUnitOfWork _unitOfWork;
        //#endregion //Fields
        public ProfessorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Professor = _unitOfWork.Set<Professor>();
        }

        public Professor GetById(long id)
        {
            return _Professor.Find(id);
        }
        public Professor GetByUserId(int Userid)
        {
            return _Professor.Where(x => x.User.Id == Userid).FirstOrDefault();
        }

        public void Insert(Professor article)
        {
            _Professor.Add(article);
        }
        public void Delete(long id)
        {
            _Professor.Where(a => a.Id.Equals(id)).Delete();
        }
        public bool CheckPNO_Exist(string pno)
        {
            return _Professor.Any(x => x.PNO == pno);
        }
        public void Update(EditProfessorViewModel viewModel)
        {
            var obj = GetById(viewModel.Id);
            obj.AvatarPath = viewModel.AvatarPath;
            obj.BirthDay = viewModel.BirthDay;
            obj.Edution = viewModel.Edution;
            obj.FirstName = viewModel.FirstName;
            obj.Gender = viewModel.Gender;
            obj.Group = viewModel.Group;
            obj.LastName = viewModel.LastName;
            obj.PNO = viewModel.PNO;
            obj.Tendency = viewModel.Tendency;
        }
        public EditProfessorViewModel GetForEdit(long id)
        {
            return _Professor.Where(a => a.Id.Equals(id)).Select(a => new EditProfessorViewModel
            {
                Id = a.Id,
                AvatarPath = a.AvatarPath,
                BirthDay = a.BirthDay,
                Edution = a.Edution,
                FirstName = a.FirstName,
                Gender = a.Gender,
                LastName = a.LastName,
                PNO = a.PNO,
                Tendency = a.Tendency,
                User = a.User,
                Group = a.Group,
                Group_Id = a.Group.Id,

                UserName = a.User.UserName,
                Password = a.User.Password,
                Email = a.User.Email
            }).FirstOrDefault();
        }

        public IList<Professor> GetAllProfessors()
        {
            return _Professor.AsNoTracking().ToList();
        }

        public IEnumerable<ProfessorViewModel> GetDataTable(out int total, string term, int page,
          Order order, ProfessorSearchBy slectionSearchBy, int count = 10)
        {
            var selectedobj = _Professor
                .Include(x=>x.Group)
                .Include(x=>x.User)
                .AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                switch (slectionSearchBy)
                {
                    case ProfessorSearchBy.BirthDate:
                        selectedobj = selectedobj.Where(a => a.BirthDay.Value.ToShortDateString().Contains(term)).AsQueryable(); break;
                    case ProfessorSearchBy.Education:
                        selectedobj = selectedobj.Where(a => a.Edution.Contains(term)).AsQueryable(); break;
                    case ProfessorSearchBy.FirstName:
                        selectedobj = selectedobj.Where(a => a.FirstName.Contains(term)).AsQueryable(); break;
                    case ProfessorSearchBy.LastName:
                        selectedobj = selectedobj.Where(a => a.LastName.Contains(term)).AsQueryable(); break;
                    case ProfessorSearchBy.PNO:
                        selectedobj = selectedobj.Where(a => a.PNO.Contains(term)).AsQueryable(); break;
                    case ProfessorSearchBy.Tendency:
                        selectedobj = selectedobj.Where(a => a.Tendency.Contains(term)).AsQueryable(); break;
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
                .Select(a => new ProfessorViewModel
                {
                    Id = a.Id,
                    AvatarPath = a.AvatarPath,
                    BirthDay = a.BirthDay,
                    Edution = a.Edution,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastName = a.LastName,
                    PNO = a.PNO,
                    Tendency = a.Tendency,
                    PeresentedCourcess = a.PeresentedCourses,
                    User = a.User,
                    Group = a.Group,

                    Group_Id = a.Group.Id
                }).Future();
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }
    }
}
