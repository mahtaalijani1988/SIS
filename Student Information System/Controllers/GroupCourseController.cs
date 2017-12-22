using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.GroupCourse;
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
    [RoutePrefix("GroupCourse")]
    [SiteAuthorize(Roles = "admin")]
    [Route("{action}")]
    public class GroupCourseController : Controller
    {
        //#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGroupCoursesService _GroupCourseService;
        private readonly ICourseService _CourseService;
        //#endregion

        //#region Constructor
        public GroupCourseController(IUnitOfWork unitOfWork, IGroupCoursesService artService,
            ICourseService co)
        {
            _unitOfWork = unitOfWork;
            _GroupCourseService = artService;
            _CourseService = co;
        }
        //#endregion
        // GET: ReciveCource
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual ActionResult List(string term = "", int page = 1, int count = 10,
            Order order = Order.Descending)
        {
            //#region Retrive Data
            int total;
            var articles = _GroupCourseService.GetDataTable(out total, term, page, order, count);
            var model = new GroupCourseListVM
            {
                Order = order,
                PageCount = count,
                PageNumber = page,
                GroupCourseList = articles,
                Term = term,
                TotalGroupCourses = total
            };
            ViewBag.CountList = DropDown.GetCountList(count);
            ViewBag.OrderList = DropDown.GetOrderList(order);
            return PartialView("_ListPartial", model);
        }

        void PopulateCourcesDropDownList(int? selectedId)
        {
            var allcources = _CourseService.GetAllCourses();
            ViewBag.AllCources = new SelectList(allcources, "Id", "Name", selectedId);
        }

        //#region Create
        [HttpGet]
        [Route("Add")]
        public ActionResult Create()
        {
            PopulateCourcesDropDownList(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Add")]
        public virtual async Task<ActionResult> Create(AddGroupCourseViewModel viewModel)
        {
                var adv = new GroupCourses
            { 
                        Course = _CourseService.GetById(viewModel.Course_Id)
                    };
                    _GroupCourseService.Insert(adv);
                    await _unitOfWork.SaveChangesAsync();
                    CacheManager.InvalidateChildActionsCache();
            return RedirectToAction("Index", "GroupCourse");
                PopulateCourcesDropDownList(null);
            return View(viewModel); 
        }
        //#endregion

        //#region Edit
        [HttpGet]
        [Route("Edit/{id}")]
        public virtual ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = _GroupCourseService.GetForEdit(id.Value);
            if (product == null) return HttpNotFound();

            PopulateCourcesDropDownList(product.Course.Id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Edit/{id}")]
        public virtual async Task<ActionResult> Edit(EditGroupCourseViewModel viewModel)
        {
                var obj = new EditGroupCourseViewModel
                {
                    Id = viewModel.Id,
                    Course = _CourseService.GetById(viewModel.Course_Id.Value)
                };
                _GroupCourseService.Update(obj);
                await _unitOfWork.SaveChangesAsync();
                CacheManager.InvalidateChildActionsCache();
                return RedirectToAction("Index", "GroupCourse");
           

            if (viewModel.Course_Id.HasValue)
                PopulateCourcesDropDownList(viewModel.Course_Id.Value);
            else
                PopulateCourcesDropDownList(null);
            
            return View(viewModel);
        }
        //#endregion

        //#region Delete
        public virtual async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return Content(null);
            string resultmsg = "";
            _GroupCourseService.Delete(id.Value);
            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();
            resultmsg = "Remove Sucessful";
            return Json(new { msg = resultmsg }, JsonRequestBehavior.AllowGet);
        }
        //#endregion

    }
}