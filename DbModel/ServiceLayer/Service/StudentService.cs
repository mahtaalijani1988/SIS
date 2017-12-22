using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Student;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Service
{
    public class StudentService : IStudentService
    {
        //#region Fields
        private readonly IDbSet<Student> _Student;
        private readonly IDbSet<User> _User;
        private readonly IUnitOfWork _unitOfWork;
        //#endregion //Fields
        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Student = _unitOfWork.Set<Student>();
            _User = _unitOfWork.Set<User>();
        }

        public Student GetById(long id)
        {
            return _Student.Find(id);
        }
        public Student GetByUserId(int Userid)
        {
            return _Student.Where(x=>x.User.Id == Userid).FirstOrDefault();
        }

        public User GetUserById(int id)
        {
            return _User.Find(id);
        }
        public void Insert(Student article)
        {
            _Student.Add(article);
        }

        public void Delete(long id)
        {
            _Student.Where(a => a.Id.Equals(id)).Delete();
        }
        public bool CheckSNO_Exist(string sno)
        {
            return _Student.Any(x => x.SNO == sno);
        }

        public void Update(EditStudentViewModel viewModel)
        {
            var obj = GetById(viewModel.Id);
            obj.AvatarPath = viewModel.AvatarPath;
            obj.Average = viewModel.Average;
            obj.BirthDay = viewModel.BirthDay;
            obj.City = viewModel.City;
            obj.FirstName = viewModel.FirstName;
            obj.Gender = viewModel.Gender;
            obj.LastName = viewModel.LastName;
            obj.SNO = viewModel.SNO;
        }
        public void UpdateUser(EditStudentViewModel viewmodel)
        {
            var obj = GetUserById(viewmodel.User.Id);
            obj.Email = viewmodel.Email;
            obj.Password = viewmodel.Password;
            obj.UserName = viewmodel.UserName;
        }
        public EditStudentViewModel GetForEdit(long id)
        {
            return _Student.Where(a => a.Id.Equals(id)).Select(a => new EditStudentViewModel
            {
                Id = a.Id,
                AvatarPath = a.AvatarPath,
                Average = a.Average,
                BirthDay = a.BirthDay,
                City = a.City,
                FirstName = a.FirstName,
                LastName = a.LastName,
                SNO = a.SNO,
                Gender = a.Gender,
                User = a.User,
                
                Email = a.User.Email,
                Password = a.User.Password,
                UserName = a.User.UserName
            }).FirstOrDefault();
        }
        public IEnumerable<StudentViewModel> GetDataTable(out int total, string term, int page,
          Order order, StudentSearchBy slectionSearchBy, int count = 10)
        {
            var selectedobj = _Student
                .Include(x=>x.User)
                .AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                switch (slectionSearchBy)
                {
                    case StudentSearchBy.BirthDate:
                        selectedobj = selectedobj.Where(a => a.BirthDay.Value.ToShortDateString().Contains(term)).AsQueryable(); break;
                    case StudentSearchBy.City:
                        selectedobj = selectedobj.Where(a => a.City.Contains(term)).AsQueryable(); break;
                    case StudentSearchBy.FirstName:
                        selectedobj = selectedobj.Where(a => a.FirstName.Contains(term)).AsQueryable(); break;
                    case StudentSearchBy.LastName:
                        selectedobj = selectedobj.Where(a => a.LastName.Contains(term)).AsQueryable(); break;
                    case StudentSearchBy.SNO:
                        selectedobj = selectedobj.Where(a => a.SNO.Contains(term)).AsQueryable(); break;
                    case StudentSearchBy.Average:
                        selectedobj = selectedobj.Where(a => a.Average.Value == Convert.ToDecimal(term)).AsQueryable(); break;
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
                .Select(a => new StudentViewModel
                {
                    Id = a.Id,
                    AvatarPath = a.AvatarPath,
                    BirthDay = a.BirthDay,
                    City = a.City,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastName = a.LastName,
                    SNO = a.SNO,
                    Average = a.Average,
                    User = a.User
                }).Future();
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }
    }
}
