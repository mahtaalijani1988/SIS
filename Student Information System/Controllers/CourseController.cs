using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Course;
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
using Order = DbModel.DomainClasses.Enums.Order; 

namespace Student_Information_System.Controllers
{
    [RoutePrefix("Course")]
    [SiteAuthorize(Roles = "admin")]
    [Route("{action}")]
    public class CourseController : Controller
    {
        //#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourseService _CourseService;
        //#endregion

        //#region Constructor
        public CourseController(IUnitOfWork unitOfWork, ICourseService artService)
        {
            _unitOfWork = unitOfWork;
            _CourseService = artService;
        }
        //#endregion
        // GET: Cource
        [HttpGet]
        public ActionResult Index()
        {
            PopulateFirstCourcesDropDownList(0);
            return View();
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual ActionResult List(string term = "", int page = 1, int count = 10,
            Order order = Order.Descending)
        {
            //#region Retrive Data
            int total;
            var articles = _CourseService.GetDataTable(out total, term, page, count);
            var model = new CourseListVM
            {
                Order = order,
                PageCount = count,
                PageNumber = page,
                CourseList = articles,
                Term = term,
                TotalCourses = total
            };
            ViewBag.CountList = DropDown.GetCountList(count);
            ViewBag.OrderList = DropDown.GetOrderList(order);
            PopulateFirstCourcesDropDownList(null);
            return PartialView("_ListPartial", model);
        }
        
        void PopulateSecondCourcesDropDownList(int? selectedId)
        {
            var categories = _CourseService.GetSecondLevelCourses();
            ViewBag.Cources = new SelectList(categories, "Id", "Name", selectedId);
        }

        void PopulateFirstCourcesDropDownList(int? selectedId)
        {
            var categories = _CourseService.GetFirstLevelCourses();
            ViewBag.CourcesForSelect = new SelectList(categories, "Id", "Name", selectedId);
        }

        //#region Create
        [HttpGet]
        [Route("Add")]
        public ActionResult Create()
        {
            PopulateFirstCourcesDropDownList(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Add")]
        public virtual async Task<ActionResult> Create(AddCourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var adv = new Course
                    {
                        Name = viewModel.Name,
                        Unit = viewModel.Unit,
                        Parent = viewModel.Parent,
                        Parent_id = viewModel.Parent_id
                    };
                    _CourseService.Insert(adv);
                    await _unitOfWork.SaveChangesAsync();
                    CacheManager.InvalidateChildActionsCache();
                    return RedirectToAction("Index", "Course");
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
            PopulateFirstCourcesDropDownList(null);
            return View(viewModel);
        }
        //#endregion

        //#region Edit
        [HttpGet]
        [Route("Edit/{id}")]
        public virtual ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = _CourseService.GetForEdit(id.Value);
            if (product == null) return HttpNotFound();
            
            PopulateFirstCourcesDropDownList(product.Parent_id);
            
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Edit/{id}")]
        public virtual async Task<ActionResult> Edit(EditCourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var adv = new EditCourseViewModel
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    Unit = viewModel.Unit,
                    Parent = viewModel.Parent,
                    Parent_id = viewModel.Parent_id
                };
                _CourseService.Update(adv);
                await _unitOfWork.SaveChangesAsync();
                CacheManager.InvalidateChildActionsCache();
                return RedirectToAction("Index", "Course");
            }

            PopulateFirstCourcesDropDownList(null);
            return View(viewModel);
        }
        //#endregion

        //#region Delete
        public virtual async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return Content(null);
            string resultmsg = "";
            _CourseService.Delete(id.Value);
            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();
            resultmsg = "Remove Sucessful";
            return Json(new { msg = resultmsg }, JsonRequestBehavior.AllowGet);
        }
        //#endregion
    }
}