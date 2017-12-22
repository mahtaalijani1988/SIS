using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Term;
using Student_Information_System.Extentions.Caching;
using Student_Information_System.Extentions.Filters;
using Student_Information_System.Extentions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Student_Information_System.Controllers
{
    [RoutePrefix("Term")]
    [SiteAuthorize(Roles = "admin")]
    [Route("{action}")]
    public class TermController : Controller
    {
        //#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITermService _TermService;
        //#endregion

        //#region Constructor
        public TermController(IUnitOfWork unitOfWork, ITermService artService)
        {
            _unitOfWork = unitOfWork;
            _TermService = artService;
        }
        //#endregion
        // GET: Term
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual ActionResult List(string term = "", int page = 1, int count = 10,
            Order order = Order.Descending, TermSearchBy objSearchBy = TermSearchBy.Name)
        {
            //#region Retrive Data
            int total;
            var articles = _TermService.GetDataTable(out total, term, page, order, objSearchBy, count);
            var model = new TermListVM
            {
                Order = order,
                PageCount = count,
                PageNumber = page,
                TermsList = articles,
                Term = term,
                TotalTerms = total
            };
            ViewBag.CountList = DropDown.GetCountList(count);
            ViewBag.OrderList = DropDown.GetOrderList(order);
            return PartialView("_ListPartial", model);
        }

        //#region Create
        [HttpGet]
        [Route("Add")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Add")]
        public virtual async Task<ActionResult> Create(AddTermViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var t = _TermService.GetByTermName(viewModel.Name);
                    if(t!= null)
                    {
                        ModelState.AddModelError("Name", "This Term Name Is Already Exist In Database");
                        return View(viewModel);
                    }
                    var obj = new Term
                    {
                        Name = viewModel.Name,
                        StartDate = viewModel.StartDate,
                        EndDate = viewModel.EndDate
                    };
                    _TermService.Insert(obj);
                    await _unitOfWork.SaveChangesAsync();
                    CacheManager.InvalidateChildActionsCache();
                    return RedirectToAction("Index", "Term");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    string s = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        s += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            s += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    ViewBag.err = s;
                }
            }
            return View(viewModel);
        }
        //#endregion

        //#region Edit
        [HttpGet]
        [Route("Edit/{id}")]
        public virtual ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = _TermService.GetForEdit(id.Value);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Edit/{id}")]
        public virtual async Task<ActionResult> Edit(EditTermViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _TermService.Update(viewModel);
                await _unitOfWork.SaveChangesAsync();
                CacheManager.InvalidateChildActionsCache();
                return RedirectToAction("Index", "Term");
            }
            return View(viewModel);
        }
        //#endregion

        //#region Delete
        public virtual async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return Content(null);
            string resultmsg = "";
            _TermService.Delete(id.Value);
            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();
            resultmsg = "Remove Sucessful";
            return Json(new { msg = resultmsg }, JsonRequestBehavior.AllowGet);
        }
        //#endregion

    }
}