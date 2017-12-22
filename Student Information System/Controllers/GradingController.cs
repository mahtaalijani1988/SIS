using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.Utilities.Security;
using DbModel.ViewModel.Election;
using DbModel.ViewModel.Professor;
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
    [RoutePrefix("Grading")]
    //[SiteAuthorize(Roles = "professor")]
    [Route("{action}")]
    public class GradingController : Controller
    {//#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElectionService _ElectionService;
        private readonly IStudentService _StudentService;
        private readonly IProfessorService _ProfessorService;
        private readonly IUserService _UserService;
        private readonly ISettingService _SettingService;
        private readonly ITermService _TermService;
        private readonly IPeresentedCoursesService _PeresentedCoursesService;
        //#endregion

        //#region Constructor
        public GradingController(IUnitOfWork unitOfWork, IElectionService artService
            , IStudentService st, IUserService us, ISettingService set, ITermService tr,
            IPeresentedCoursesService pr, IProfessorService prof)
        {
            _unitOfWork = unitOfWork;
            _ElectionService = artService;
            _StudentService = st;
            _UserService = us;
            _SettingService = set;
            _TermService = tr;
            _PeresentedCoursesService = pr;
            _ProfessorService = prof;
        }
        //#endregion
        // GET: ReciveCource
        [HttpGet]
        [SiteAuthorize(Roles = "professor")]
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual ActionResult List(string term = "", int page = 1, int count = 10,
            Order order = Order.Descending, ScoreStateType objSearchBy = ScoreStateType.Score,
            ElectionOrderBy objOrderBy = ElectionOrderBy.Id)
        {
            string userName = HttpContext.User.Identity.Name;
            Professor pro = _ProfessorService.GetByUserId(_UserService.GetUserByUserName(userName).Id);
            //#region Retrive Data
            int total;
            var articles = _ElectionService.GetDataTableForScore(out total, term, page, order,
                objSearchBy, objOrderBy, pro.Id, count);
            var model = new ScoreListVM
            {
                Order = order,
                PageCount = count,
                PageNumber = page,
                ScoreList = articles,
                Term = term,
                TotalElections = total
            };
            ViewBag.CountList = DropDown.GetCountList(count);
            ViewBag.OrderList = DropDown.GetOrderList(order);
            return PartialView("_ListPartial", model);
        }

        List<Term> PopulateTermDropDownList(int? selectedId)
        {
            List<Term> AllTerms = new List<Term>();
            var allobj = _TermService.GetAllTerms();
            ViewBag.AllTerms = new SelectList(allobj, "Id", "Name", selectedId);
            if (allobj != null)
                AllTerms = allobj.ToList();
            return AllTerms;
        }

        [HttpGet]
        [SiteAuthorize(Roles = "student")]
        public ActionResult IndexMyGrads()
        {
            PopulateTermDropDownList(null);
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual ActionResult ListMyGrade(int? TermId, int? page = 1)
        {
            string userName = HttpContext.User.Identity.Name;
            Student stu = _StudentService.GetByUserId(_UserService.GetUserByUserName(userName).Id);
            int pageSize = 100;
            //#region Retrive Data
            int total;
            var articles = _ElectionService.GetDataTableForStudentScore(out total, "", TermId, page.Value,
                Order.Asscending, ScoreStateType.Score, ElectionOrderBy.Id, stu.Id, pageSize);
            var model = new ScoreListVM
            {
                TermId = (TermId ?? 1),
                Order = Order.Asscending,
                PageCount = pageSize,
                PageNumber = (page ?? 1),
                ScoreList = articles,
                Term = "",
                TotalElections = total
            };
            List<Term> kol = PopulateTermDropDownList(TermId);
            if (kol != null && kol.Count>0 && TermId == null)
                ViewBag.avglist = _ElectionService.ComputeStudentAvgForTerm(stu, kol[0].Id);
            else if (kol != null && TermId != null)
                ViewBag.avglist = _ElectionService.ComputeStudentAvgForTerm(stu, TermId.Value);
            return PartialView("_ListMyGrade", model);
        }

        //#region Create
        private decimal getGrid(string value)
        {
            decimal ret = 0;
            if (!string.IsNullOrEmpty(value))
            {
                switch (value)
                {
                    case "A+": ret = 4.30m; break;
                    case "A": ret = 4.00m; break;
                    case "A-": ret = 3.70m; break;
                    case "B+": ret = 3.30m; break;
                    case "B": ret = 3.00m; break;
                    case "B-": ret = 2.70m; break;
                    case "C+": ret = 2.30m; break;
                    case "C": ret = 2.00m; break;
                    case "C-": ret = 1.70m; break;
                    case "D+": ret = 1.30m; break;
                    case "D": ret = 1.00m; break;
                    case "D-": ret = 0.70m; break;
                    case "F": ret = 0; break;
                    default: ret = 0; break;
                }
            }
            return ret;
        }
        private string setGrid(decimal? d)
        {
            string ret = "";
            if (d.HasValue)
            {
                switch (d.Value.ToString())
                {
                    case "4.30": ret = "A+"; break;
                    case "4.00": ret = "A"; break;
                    case "3.70": ret = "A-"; break;
                    case "3.30": ret = "B+"; break;
                    case "3.00": ret = "B"; break;
                    case "2.70": ret = "B-"; break;
                    case "2.30": ret = "C+"; break;
                    case "2.00": ret = "C"; break;
                    case "1.70": ret = "C-"; break;
                    case "1.30": ret = "D+"; break;
                    case "1.00": ret = "D"; break;
                    case "0.70": ret = "D-"; break;
                    case "0": ret = "F"; break;
                    default: ret = ""; break;
                }
            }
            return ret;
        }

        [HttpGet]
        [Route("SetGrad/{id}")]
        [SiteAuthorize(Roles = "professor")]
        public ActionResult SetGrad(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var scor =_ElectionService.GetForSetScore(id.Value);
            if (scor == null) return HttpNotFound();
            ViewBag.SSS = setGrid(scor.Score);
            return View(scor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("SetGrad/{id}")]
        [SiteAuthorize(Roles = "professor")]
        public virtual async Task<ActionResult> SetGrad(int? id, SetScoreViewModel viewModel, FormCollection element)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.Score = getGrid(element["scores"]);
                    _ElectionService.UpdateSetScore(viewModel);
                    await _unitOfWork.SaveChangesAsync();
                    CacheManager.InvalidateChildActionsCache();
                    return RedirectToAction("Index", "Grading");
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
                    //throw;
                }
            }
            return View(viewModel);
        }
        //#endregion

        [HttpGet]
        [SiteAuthorize(Roles = "student")]
        public ActionResult IndexKolGrads()
        {
            string userName = HttpContext.User.Identity.Name;
            Student stu = _StudentService.GetByUserId(_UserService.GetUserByUserName(userName).Id);
            IEnumerable<AverageClass> cl = _ElectionService.ComputeStudentAvg(stu);
            ViewBag.avglist = cl;
            ViewBag.koll = cl.Sum(x => ((x.Average.HasValue) ? x.Average.Value : 0))
                        /(cl.Count()>0?cl.Count():1);
            return View();
        }

    }
}