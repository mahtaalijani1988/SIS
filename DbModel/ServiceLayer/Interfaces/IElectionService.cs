using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ViewModel.Election;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ServiceLayer.Interfaces
{
    public interface IElectionService
    {
        Election GetById(int id);
        void Insert(Election article);
        void Delete(int id);
        void Update(EditElectionViewModel viewModel);
        EditElectionViewModel GetForEdit(int id);
        IEnumerable<ElectionViewModel> GetDataTable(out int total, string term, int page,
          Order order, ElectionSearchBy slectionSearchBy, ElectionOrderBy slectionOrderBy, int count = 10);
        ElectionStatus Choosen(long Student_id, int Peresentedc_id);
        IEnumerable<ScoreViewModel> GetDataTableForScore(out int total, string term, int page,
          Order order, ScoreStateType slectionSearchBy, ElectionOrderBy slectionOrderBy, long professor_id, int count = 10);
        SetScoreViewModel GetForSetScore(int Election_id);
        IEnumerable<ElectionViewModel> GetByStudent(long student_id);
        void UpdateSetScore(SetScoreViewModel viewModel);
        IEnumerable<ScoreViewModel> GetDataTableForStudentScore(out int total, string term,
            int? Term_id, int page, Order order, ScoreStateType slectionSearchBy,
            ElectionOrderBy slectionOrderBy, long Student_id, int count = 10);
        IEnumerable<AverageClass> ComputeStudentAvg(Student thStudent);
        IEnumerable<AverageClass> ComputeStudentAvgForTerm(Student thStudent, int Term_id);
        ElectionRemoveStatus Remove(int Election_id);
    }
}
