using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.PeresentedCourses;
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
    [RoutePrefix("AvailableCourses")]
    [SiteAuthorize(Roles = "admin")]
    [Route("{action}")]
    public class AvailableCoursesController : Controller
    {
        //#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPeresentedCoursesService _PeresentedCoursesService;
        private readonly IProfessorService _ProfessorService;
        private readonly ICourseService _CourseService;
        private readonly ITermService _TermService;
        //#endregion

        //#region Constructor
        public AvailableCoursesController(IUnitOfWork unitOfWork, IPeresentedCoursesService artService,
            IProfessorService pro, ICourseService cor, ITermService te)
        {
            _unitOfWork = unitOfWork;
            _PeresentedCoursesService = artService;
            _ProfessorService = pro;
            _CourseService = cor;
            _TermService = te;
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
            Order order = Order.Descending, PeresentedCoursesSearchBy objSearchBy = PeresentedCoursesSearchBy.Cource_Name,
            PeresentedCoursesOrderBy objOrderBy = PeresentedCoursesOrderBy.Id)
        {
            //#region Retrive Data
            int total;
            var articles = _PeresentedCoursesService.GetDataTable(out total, term, page, order, objSearchBy, objOrderBy, count);
            var model = new PeresentedCoursesListVM
            {
                Order = order,
                PageCount = count,
                PageNumber = page,
                PeresentedCoursesList = articles,
                Term = term,
                TotalPeresentedCourses = total
            };
            ViewBag.CountList = DropDown.GetCountList(count);
            ViewBag.OrderList = DropDown.GetOrderList(order);
            return PartialView("_ListPartial", model);
        }

        void PopulateCourcesDropDownList(long? selectedId)
        {
            var allcources = _CourseService.GetAllCourses();
            ViewBag.AllCources = new SelectList(allcources, "Id", "Name", selectedId);
        }
        void PopulateProfessorDropDownList(long? selectedId)
        {
            var AllProfessors = _ProfessorService.GetAllProfessors();
            ViewBag.AllProfessors = new SelectList(AllProfessors, "Id", "LastName", selectedId);
        }
        void PopulateTermDropDownList(long? selectedId)
        {
            var AllTerms = _TermService.GetAllTerms();
            ViewBag.AllTerms = new SelectList(AllTerms, "Id", "Name", selectedId);
        }
        //#region Create
        [HttpGet]
        [Route("Add")]
        public ActionResult Create()
        {
            PopulateCourcesDropDownList(null);
            PopulateProfessorDropDownList(null);
            PopulateTermDropDownList(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Add")]
        public virtual async Task<ActionResult> Create(AddPeresentedCoursesViewModel viewModel)
        {
            var adv = new PeresentedCourses
            {
                Course = _CourseService.GetById(viewModel.Course_Id.Value),
                Professor = _ProfessorService.GetById(viewModel.Professor_Id.Value),
                Term = _TermService.GetById(viewModel.Term_Id.Value),
                Capacity = viewModel.Capacity,
                Remain_Capacity = viewModel.Capacity
            };
            _PeresentedCoursesService.Insert(adv);
            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();


            PopulateCourcesDropDownList(null);
            PopulateProfessorDropDownList(null);
            PopulateTermDropDownList(null);

            return RedirectToAction("Index", "AvailableCourses");
            return View(viewModel);
        }
        //#endregion

        //#region Edit
        [HttpGet]
        [Route("Edit/{id}")]
        public virtual ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = _PeresentedCoursesService.GetForEdit(id.Value);
            if (product == null) return HttpNotFound();

            PopulateCourcesDropDownList(product.Course.Id);
            PopulateProfessorDropDownList(product.Professor.Id);
            PopulateTermDropDownList(product.Term.Id);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Edit/{id}")]
        public virtual async Task<ActionResult> Edit(EditPeresentedCoursesViewModel viewModel)
        {
            var adv = new EditPeresentedCoursesViewModel
            {
                Id = viewModel.Id,
                Course_Id = viewModel.Course_Id,
                Professor_Id = viewModel.Professor_Id,
                Term_Id = viewModel.Term_Id,
                Course = _CourseService.GetById(viewModel.Course_Id.Value),
                Professor = _ProfessorService.GetById(viewModel.Professor_Id.Value),
                Term = _TermService.GetById(viewModel.Term_Id.Value),
                Capacity = viewModel.Capacity,
                Remain_Capacity = viewModel.Capacity
            };
            _PeresentedCoursesService.Update(adv);
            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();
            return RedirectToAction("Index", "AvailableCourses");


            if (viewModel.Course_Id.HasValue)
                PopulateCourcesDropDownList(viewModel.Course_Id.Value);
            else
                PopulateCourcesDropDownList(null);
            if (viewModel.Professor_Id.HasValue)
                PopulateProfessorDropDownList(viewModel.Professor_Id.Value);
            else
                PopulateProfessorDropDownList(null);
            if (viewModel.Term_Id.HasValue)
                PopulateTermDropDownList(viewModel.Term_Id.Value);
            else
                PopulateTermDropDownList(null);

            return View(viewModel);
        }
        //#endregion

        //#region Delete
        public virtual async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return Content(null);
            string resultmsg = "";
            _PeresentedCoursesService.Delete(id.Value);
            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();
            resultmsg = "Remove Sucessful";
            return Json(new { msg = resultmsg }, JsonRequestBehavior.AllowGet);
        }
        //#endregion

    }
}