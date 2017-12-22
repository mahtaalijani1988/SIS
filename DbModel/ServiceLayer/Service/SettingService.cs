using System;
using System.Data.Entity;
using System.Linq;
using DbModel.Context;
using DbModel.DomainClasses.Entities;
using EFSecondLevelCache;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel;
using DbModel.ViewModel.Setting;
using DbModel.Utilities.Caching;
using System.Collections.Generic;

namespace DbModel.ServiceLayer.EFServices
{
    public class SettingService : ISettingService
    {
        //#region Fields

        private readonly IDbSet<SiteOption> _options;
        private readonly IDbSet<Term> _term;
        //#endregion

        //#region Consructor

        public SettingService(IUnitOfWork unitOfWork)
        {
            _options = unitOfWork.Set<SiteOption>();
            _term = unitOfWork.Set<Term>();
        }

        //#endregion
        public void Update(EditSettingViewModel viewModel)
        {
            var settings = _options.ToList();
            settings.Where(a => a.Name.Equals("Department_Name")).FirstOrDefault().Value = viewModel.Department_Name;
            settings.Where(a => a.Name.Equals("Term_Name")).FirstOrDefault().Value = viewModel.Term.Name;
            settings.Where(a => a.Name.Equals("Start_Election_Date")).FirstOrDefault().Value = viewModel.Start_Election_Date;
            settings.Where(a => a.Name.Equals("Start_Election_Time")).FirstOrDefault().Value = viewModel.Start_Election_Time;
            settings.Where(a => a.Name.Equals("End_Election_Date")).FirstOrDefault().Value = viewModel.End_Election_Date;
            settings.Where(a => a.Name.Equals("End_Election_Time")).FirstOrDefault().Value = viewModel.End_Election_Time;

            settings.Where(a => a.Name.Equals("Start_Remove_Date")).FirstOrDefault().Value = viewModel.Start_Remove_Date;
            settings.Where(a => a.Name.Equals("Start_Remove_Time")).FirstOrDefault().Value = viewModel.Start_Remove_Time;
            settings.Where(a => a.Name.Equals("End_Remove_Date")).FirstOrDefault().Value = viewModel.End_Remove_Date;
            settings.Where(a => a.Name.Equals("End_Remove_Time")).FirstOrDefault().Value = viewModel.End_Remove_Time;

            settings.Where(a => a.Name.Equals("Student_max_Unit")).FirstOrDefault().Value = viewModel.Student_max_Unit;
        }

        public EditSettingViewModel GetOptionsForEdit()
        {
            var settings = _options.ToList();
            var model = new EditSettingViewModel
            {
                Department_Name = settings.Where(a => a.Name.Equals("Department_Name")).FirstOrDefault().Value,
                Term_Name = settings.Where(a => a.Name.Equals("Term_Name")).FirstOrDefault().Value,
                Start_Election_Date = settings.Where(a => a.Name.Equals("Start_Election_Date")).FirstOrDefault().Value,
                Start_Election_Time = settings.Where(a => a.Name.Equals("Start_Election_Time")).FirstOrDefault().Value,
                End_Election_Date = settings.Where(a => a.Name.Equals("End_Election_Date")).FirstOrDefault().Value,
                End_Election_Time = settings.Where(a => a.Name.Equals("End_Election_Time")).FirstOrDefault().Value,

                Start_Remove_Date = settings.Where(a => a.Name.Equals("Start_Remove_Date")).FirstOrDefault().Value,
                Start_Remove_Time = settings.Where(a => a.Name.Equals("Start_Remove_Time")).FirstOrDefault().Value,
                End_Remove_Date = settings.Where(a => a.Name.Equals("End_Remove_Date")).FirstOrDefault().Value,
                End_Remove_Time = settings.Where(a => a.Name.Equals("End_Remove_Time")).FirstOrDefault().Value,

                Student_max_Unit = settings.Where(a => a.Name.Equals("Student_max_Unit")).FirstOrDefault().Value
            };
            if (!string.IsNullOrEmpty(model.Term_Name))
            {
                Term tht =_term.Where(x => x.Name.Equals(model.Term_Name)).FirstOrDefault();
                model.Term = tht;
                model.Term_Id = tht.Id;
            }
            return model;

        }

        [CacheMethod(SecondsToCache = 20)]
        public SiteConfig GetAll()
        {
            List<SiteOption> options = _options.ToList();
            var model = new SiteConfig
            {
                Department_Name = options.Where(op => op.Name.Equals("Department_Name")).FirstOrDefault().Value,
                Start_Election_Date = options.Where(op => op.Name.Equals("Start_Election_Date")).FirstOrDefault().Value,
                Start_Election_Time = options.Where(op => op.Name.Equals("Start_Election_Time")).FirstOrDefault().Value,
                End_Election_Date = options.Where(op => op.Name.Equals("End_Election_Date")).FirstOrDefault().Value,
                End_Election_Time = options.Where(op => op.Name.Equals("End_Election_Time")).FirstOrDefault().Value,

                Start_Remove_Date = options.Where(op => op.Name.Equals("Start_Remove_Date")).FirstOrDefault().Value,
                Start_Remove_Time = options.Where(op => op.Name.Equals("Start_Remove_Time")).FirstOrDefault().Value,
                End_Remove_Date = options.Where(op => op.Name.Equals("End_Remove_Date")).FirstOrDefault().Value,
                End_Remove_Time = options.Where(op => op.Name.Equals("End_Remove_Time")).FirstOrDefault().Value,

                Student_max_Unit = options.Where(op => op.Name.Equals("Student_max_Unit")).FirstOrDefault().Value,
                Term_Name =options.Where(op => op.Name.Equals("Term_Name")).FirstOrDefault().Value
            };
            model.Term = _term.Where(x => x.Name.Equals(model.Term_Name)).FirstOrDefault();
            return model;
        }
    }
}
