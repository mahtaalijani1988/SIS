using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Setting;
using Student_Information_System.Extentions.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Student_Information_System.Controllers
{
    [RoutePrefix("Configuration")]
    [SiteAuthorize(Roles = "admin")]
    [Route("{action}")]
    public class ConfigurationController : Controller
    {
        //#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly ITermService _TermService;
        //#endregion

        //#region Constructor
        public ConfigurationController(IUnitOfWork unitOfWork, ISettingService settingService,
            ITermService ite)
        {
            _unitOfWork = unitOfWork;
            _settingService = settingService;
            _TermService = ite;
        }
        //#endregion


        void PopulateTermDropDownList(int? selectedId)
        {
            var AllTerms = _TermService.GetAllTerms();
            SelectList li = new SelectList(AllTerms, "Id", "Name", selectedId);
            ViewBag.AllTerms = li;
        }

        //#region Edit
        [Route("Edit")]
        [HttpGet]
        public virtual ActionResult Edit()
        {
            var model = _settingService.GetOptionsForEdit();
            if (model.Term_Id.HasValue)
                PopulateTermDropDownList(model.Term_Id.Value);
            else
                PopulateTermDropDownList(null);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public virtual async Task<ActionResult> Edit(EditSettingViewModel viewModel)
        {
            if (viewModel.Term_Id.HasValue)
            {
                Term th = _TermService.GetById(viewModel.Term_Id.Value);
                viewModel.Term = th;
                viewModel.Term_Name = th.Name; 
                PopulateTermDropDownList(viewModel.Term_Id);
                _settingService.Update(viewModel);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                PopulateTermDropDownList(null);
                ModelState.AddModelError("Term_Id", "The Term Cannot Emptye");
                return View(viewModel);
            }
            return View(viewModel);
        }

        //#endregion

    }
}