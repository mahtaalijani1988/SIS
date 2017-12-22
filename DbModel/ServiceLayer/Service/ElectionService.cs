using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Election;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Service
{
    public class ElectionService : IElectionService
    {
        //#region Fields
        private readonly IDbSet<Election> _Election;
        private readonly IDbSet<PeresentedCourses> _PeresentedCourses;
        private readonly IDbSet<Term> _Term;
        private readonly IDbSet<SiteOption> _SiteOption;
        private readonly IDbSet<Student> _Student;
        private readonly IUnitOfWork _unitOfWork;
        //#endregion //Fields
        public ElectionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _Election = _unitOfWork.Set<Election>();
            _PeresentedCourses = _unitOfWork.Set<PeresentedCourses>();
            _SiteOption = unitOfWork.Set<SiteOption>();
            _Student = unitOfWork.Set<Student>();
            _Term = _unitOfWork.Set<Term>();
        }

        public Election GetById(int id)
        {
            return _Election.Find(id);
        }

        public void Insert(Election article)
        {
            _Election.Add(article);
        }

        public void Delete(int id)
        {
            _Election.Where(a => a.Id.Equals(id)).Delete();
        }

        public ElectionStatus Choosen(long Student_id, int Peresentedc_id)
        {
            var result = ElectionStatus.Success;
            var term_name = _SiteOption.ToList().Where(op => op.Name.Equals("Term_Name")).FirstOrDefault().Value;
            var max_unit = _SiteOption.ToList().Where(op => op.Name.Equals("Student_max_Unit")).FirstOrDefault().Value;
            Term thTerm = _Term.Where(x => x.Name == term_name).FirstOrDefault();
            PeresentedCourses peresent = _PeresentedCourses.Where(x => x.Id == Peresentedc_id).FirstOrDefault();
            Election election = _Election.Where(x => x.Student.Id == Student_id && x.PeresentedCource.Id == Peresentedc_id).FirstOrDefault();

            List<Election> maxstuselected = _Election.Where(x => x.Student.Id == Student_id && x.PeresentedCource.Term.Name == term_name).ToList();

            int ii = 0;
            foreach (var item in maxstuselected)
            {
                ii += Convert.ToInt32(item.PeresentedCource.Course.Unit);
            }
            ii += peresent.Course.Unit;

            if (election != null)
                result = ElectionStatus.CannotSelectPrevSelected;
            if (ii > int.Parse(max_unit))
                result = ElectionStatus.CannotSelectOutOfUnit;
            if ((peresent.Remain_Capacity - 1) < 0)
                result = ElectionStatus.CannotSelectCapacityFull;

            return result;
        }

        public ElectionRemoveStatus Remove(int Election_id)
        {
            var result = ElectionRemoveStatus.Success;
            Election election = _Election.Where(x => x.Id == Election_id).FirstOrDefault();
            
            if (election.Score != null)
                result = ElectionRemoveStatus.CannotRemoveScored;

            return result;
        }

        public void Update(EditElectionViewModel viewModel)
        {
            var obj = GetById(viewModel.Id);
            obj.Score = viewModel.Score;
            obj.Student = viewModel.Student;
            obj.PeresentedCource = viewModel.PeresentedCource;
        }
        public EditElectionViewModel GetForEdit(int id)
        {
            return _Election.Where(a => a.Id.Equals(id)).Select(a => new EditElectionViewModel
            {
                Id = a.Id,  
                PeresentedCource = a.PeresentedCource,
                Score = a.Score,
                Student = a.Student,
                
                PeresentedCource_Id = a.PeresentedCource.Id,
                Student_Id = a.Student.Id
            }).FirstOrDefault();
        }
        public IEnumerable<ElectionViewModel> GetByStudent(long student_id)
        {
            return _Election.Where(x => x.Student.Id == student_id).Select(a => new ElectionViewModel
            {
                Id = a.Id, 
                Score = a.Score,
                PeresentedCource = a.PeresentedCource,
                Student = a.Student
            }).ToList();
        }

        public IEnumerable<ElectionViewModel> GetDataTable(out int total, string term, int page,
          Order order, ElectionSearchBy slectionSearchBy, ElectionOrderBy slectionOrderBy, int count = 10)
        {
            var selectedobj = _Election
                .Include(x=>x.PeresentedCource)
                .Include(x=>x.Student)
                .AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                switch (slectionSearchBy)
                {
                    case ElectionSearchBy.Peresented_Cource_Name:
                        selectedobj = selectedobj.Where(a => a.PeresentedCource.Course.Name.Contains(term)).AsQueryable(); break;
                    case ElectionSearchBy.Score:
                        selectedobj = selectedobj.Where(a => a.Score == Convert.ToDecimal(term)).AsQueryable(); break;
                    case ElectionSearchBy.StudentName:
                        selectedobj = selectedobj.Where(a => a.Student.FirstName.Contains(term) ||
                                a.Student.LastName.Contains(term)).AsQueryable(); break;
                }
            }
            if (order == Order.Asscending)
            {
                switch (slectionOrderBy)
                {
                    case ElectionOrderBy.Id:
                        selectedobj = selectedobj.OrderBy(x => x.Id).AsQueryable(); break;
                    case ElectionOrderBy.Score:
                        selectedobj = selectedobj.OrderBy(x => x.Score).AsQueryable(); break;
                }
            }
            else
            {
                switch (slectionOrderBy)
                {
                    case ElectionOrderBy.Id:
                        selectedobj = selectedobj.OrderByDescending(x => x.Id).AsQueryable(); break;
                    case ElectionOrderBy.Score:
                        selectedobj = selectedobj.OrderByDescending(x => x.Score).AsQueryable(); break;
                }
            }

            var totalQuery = selectedobj.FutureCount();
            var query = selectedobj.Skip((page - 1) * count).Take(count)
                .Select(a => new ElectionViewModel
                {
                    Id = a.Id,
                    Score = a.Score,
                    PeresentedCource = a.PeresentedCource,
                    Student = a.Student
                }).Future();
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }

        public SetScoreViewModel GetForSetScore(int Election_id)
        {
            return _Election.Where(a => a.Id.Equals(Election_id)).Select(a => new SetScoreViewModel
            {
                Id = a.Id,
                PeresentedCource = a.PeresentedCource,
                PeresentedCource_Id = a.PeresentedCource.Id,
                Student = a.Student,
                Student_Id = a.Student.Id,
                Score = a.Score
            }).FirstOrDefault();
        }

        public void UpdateSetScore(SetScoreViewModel viewModel)
        {
            var obj = GetById(viewModel.Id);
            obj.Score = viewModel.Score;
        }
        public IEnumerable<ScoreViewModel> GetDataTableForScore(out int total, string term, int page,
          Order order, ScoreStateType slectionSearchBy, ElectionOrderBy slectionOrderBy, long professor_id, int count = 10)
        {
            var selectedobj = _Election
             .Include(x => x.PeresentedCource)
             .Include(x => x.Student)
             .Where(x => x.PeresentedCource.Professor.Id == professor_id)
             .AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(term))
            {
                switch (slectionSearchBy)
                {
                    case ScoreStateType.NotScore:
                        selectedobj = selectedobj.Where(a => !a.Score.HasValue).AsQueryable(); break;
                    case ScoreStateType.Score:
                        selectedobj = selectedobj.Where(a => a.Score.HasValue).AsQueryable(); break;
                }
            }
            if (order == Order.Asscending)
            {
                switch (slectionOrderBy)
                {
                    case ElectionOrderBy.Id:
                        selectedobj = selectedobj.OrderBy(x => x.Id).AsQueryable(); break;
                    case ElectionOrderBy.Score:
                        selectedobj = selectedobj.OrderBy(x => x.Score).AsQueryable(); break;
                }
            }
            else
            {
                switch (slectionOrderBy)
                {
                    case ElectionOrderBy.Id:
                        selectedobj = selectedobj.OrderByDescending(x => x.Id).AsQueryable(); break;
                    case ElectionOrderBy.Score:
                        selectedobj = selectedobj.OrderByDescending(x => x.Score).AsQueryable(); break;
                }
            }

            var totalQuery = selectedobj.FutureCount();
            var query = selectedobj.Skip((page - 1) * count).Take(count)
                .Select(a => new ScoreViewModel
                {
                    Id = a.Id,
                    Score = a.Score,
                    PeresentedCource = a.PeresentedCource,
                    Student = a.Student
                }).Future();
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }
        public IEnumerable<ScoreViewModel> GetDataTableForStudentScore(out int total, string term,
            int? Term_id, int page, Order order, ScoreStateType slectionSearchBy,
            ElectionOrderBy slectionOrderBy, long Student_id, int count = 10)
        {
            var selectedobj = _Election
             .Include(x => x.PeresentedCource)
             .Include(x => x.Student)
             .Where(x => x.Student.Id == Student_id)
             .AsNoTracking().AsQueryable();
            if(Term_id.HasValue)
            {
                selectedobj = selectedobj.Where(x => x.PeresentedCource.Term.Id == Term_id.Value).AsQueryable();
            }
            if (!string.IsNullOrEmpty(term))
            {
                switch (slectionSearchBy)
                {
                    case ScoreStateType.NotScore:
                        selectedobj = selectedobj.Where(a => !a.Score.HasValue).AsQueryable(); break;
                    case ScoreStateType.Score:
                        selectedobj = selectedobj.Where(a => a.Score.HasValue).AsQueryable(); break;
                }
            }
            if (order == Order.Asscending)
            {
                switch (slectionOrderBy)
                {
                    case ElectionOrderBy.Id:
                        selectedobj = selectedobj.OrderBy(x => x.Id).AsQueryable(); break;
                    case ElectionOrderBy.Score:
                        selectedobj = selectedobj.OrderBy(x => x.Score).AsQueryable(); break;
                }
            }
            else
            {
                switch (slectionOrderBy)
                {
                    case ElectionOrderBy.Id:
                        selectedobj = selectedobj.OrderByDescending(x => x.Id).AsQueryable(); break;
                    case ElectionOrderBy.Score:
                        selectedobj = selectedobj.OrderByDescending(x => x.Score).AsQueryable(); break;
                }
            }


            var totalQuery = selectedobj.FutureCount();
            var query = selectedobj.Skip((page - 1) * count).Take(count)
                .Select(a => new ScoreViewModel
                {
                    Id = a.Id,
                    Score = (a.Score.HasValue)?a.Score.Value:0,
                    PeresentedCource = a.PeresentedCource,
                    Student = a.Student
                }).Future();
            foreach (var item in query)
            {
                item.Average = query.Average(x => x.Score.Value * x.PeresentedCource.Course.Unit);
            }
            total = totalQuery.Value;
            var categories = query.ToList();
            return categories;
        }
        public IEnumerable<AverageClass> ComputeStudentAvg(Student thStudent)
        {
            var selectedobj2 = _Election
             .Include(x => x.PeresentedCource)
             .Include(x => x.Student)
             .Where(x => x.Student.Id == thStudent.Id)
              .GroupBy(x => new
              {
                  x.PeresentedCource.Term
              }).Select(g => new
              {
                  Term = g.Key.Term,
                  Average = g.Sum(x => (x.Score.HasValue?x.Score.Value:0) * x.PeresentedCource.Course.Unit)
                    / g.Sum(x => x.PeresentedCource.Course.Unit)
              });
            List<AverageClass> cl = new List<AverageClass>();
            foreach (var item in selectedobj2)
            {
                cl.Add(new AverageClass
                {
                    Term_Id = item.Term.Id,
                    Average = item.Average,
                    Student_Id = thStudent.Id,
                    Student = thStudent,
                    Term = item.Term
                });
            }
            var categories = cl.ToList();
            return categories;
        }
        public IEnumerable<AverageClass> ComputeStudentAvgForTerm(Student thStudent, int Term_id)
        {
            var selectedobj2 = _Election
             .Include(x => x.PeresentedCource)
             .Include(x => x.Student)
             .Where(x => x.Student.Id == thStudent.Id && x.PeresentedCource.Term.Id == Term_id)
              .GroupBy(x => new
              {
                  x.PeresentedCource.Term
              }).Select(g => new
              {
                  Term = g.Key.Term,
                  Average = g.Sum(x => (x.Score.HasValue ? x.Score.Value : 0) * x.PeresentedCource.Course.Unit)
                    / g.Sum(x => x.PeresentedCource.Course.Unit)
              });
            List<AverageClass> cl = new List<AverageClass>();
            foreach (var item in selectedobj2)
            {
                cl.Add(new AverageClass
                {
                    Term_Id = item.Term.Id,
                    Average = item.Average,
                    Student_Id = thStudent.Id,
                    Student = thStudent,
                    Term = item.Term
                });
            }
            var categories = cl.ToList();
            return categories;
        }
    }
}
