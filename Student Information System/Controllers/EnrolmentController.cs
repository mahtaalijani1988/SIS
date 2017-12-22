using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.ViewModel.Election;
using Student_Information_System.Extentions.Caching;
using Student_Information_System.Extentions.Filters;
using Student_Information_System.Extentions.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Student_Information_System.Controllers
{
    [RoutePrefix("Enrolment")]
    [SiteAuthorize(Roles = "student")]
    [Route("{action}")]
    public class EnrolmentController : Controller
    {
        //#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElectionService _ElectionService;
        private readonly IStudentService _StudentService;
        private readonly IUserService _UserService;
        private readonly ISettingService _SettingService;
        private readonly ITermService _TermService;
        private readonly IPeresentedCoursesService _PeresentedCoursesService;
        //#endregion

        //#region Constructor
        public EnrolmentController(IUnitOfWork unitOfWork, IElectionService artService
            , IStudentService st, IUserService us, ISettingService set, ITermService tr,
            IPeresentedCoursesService pr)
        {
            _unitOfWork = unitOfWork;
            _ElectionService = artService;
            _StudentService = st;
            _UserService = us;
            _SettingService = set;
            _TermService = tr;
            _PeresentedCoursesService = pr;
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
            Order order = Order.Descending , ElectionSearchBy objSearchBy = ElectionSearchBy.Score,
            ElectionOrderBy objOrderBy = ElectionOrderBy.Id)
        {
            //#region Retrive Data
            int total;
            var articles = _ElectionService.GetDataTable(out total, term, page, order, objSearchBy, objOrderBy, count);
            var model = new ElectionListVM
            {
                Order = order,
                PageCount = count,
                PageNumber = page,
                ElectionList = articles,
                Term = term,
                TotalElections = total
            };
            ViewBag.CountList = DropDown.GetCountList(count);
            ViewBag.OrderList = DropDown.GetOrderList(order);
            return PartialView("_ListPartial", model);
        }

        public void Requiredneeds(bool type)
        {
            var setting = _SettingService.GetOptionsForEdit();
            DateTime start = DateTime.Now, end = DateTime.Now, now = DateTime.Now;
            bool state = false;
            if (type == true &&
                !string.IsNullOrEmpty(setting.Start_Election_Date) && !string.IsNullOrEmpty(setting.Start_Election_Time) &&
                !string.IsNullOrEmpty(setting.End_Election_Date) && !string.IsNullOrEmpty(setting.End_Election_Time))
            {
                start = DateTime.ParseExact(setting.Start_Election_Date + " " + setting.Start_Election_Time,
                            "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);//.TimeOfDay;
                end = DateTime.ParseExact(setting.End_Election_Date + " " + setting.End_Election_Time,
                            "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);//.TimeOfDay;
            }
            else if (type == false &&
                !string.IsNullOrEmpty(setting.Start_Remove_Date) && !string.IsNullOrEmpty(setting.Start_Remove_Time) &&
                !string.IsNullOrEmpty(setting.End_Remove_Date) && !string.IsNullOrEmpty(setting.End_Remove_Time))
            {
                start = DateTime.ParseExact(setting.Start_Remove_Date + " " + setting.Start_Remove_Time,
                            "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);//.TimeOfDay;
                end = DateTime.ParseExact(setting.End_Remove_Date + " " + setting.End_Remove_Time,
                            "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);//.TimeOfDay;
            }
            ////(now >= start)  --> true
            ////(now <= end)  --> false
            if ((start < now) && (now < end))
            // (DateTime.Compare(start, now) == 0 && DateTime.Compare(end, now) == 0)
            {
                state = true;

                var term = setting.Term;//_SettingService.GetOptionsForEdit().Term;
                if (term != null)
                {
                    ViewBag.TermName = term.Name;
                    var plist = _PeresentedCoursesService.GetByTermName(term.Name);
                    if (plist != null)
                    {
                        ViewBag.prlist = plist;
                    }
                    else
                    {
                        IEnumerable<PeresentedCourses> pp = new List<PeresentedCourses>();
                        ViewBag.prlist = pp;
                    }
                }
                string userName = HttpContext.User.Identity.Name;
                var stu = _StudentService.GetByUserId(_UserService.GetUserByUserName(userName).Id);
                if (stu != null)
                {
                    ViewBag.fname = stu.FirstName;
                    ViewBag.lname = stu.LastName;
                    ViewBag.sno = stu.SNO;
                    var userselectedlist = _ElectionService.GetByStudent(stu.Id);
                    if (userselectedlist != null)
                    {
                        ViewBag.usersellist = userselectedlist;
                    }
                    else
                    {
                        IEnumerable<ElectionViewModel> ss = new List<ElectionViewModel>();
                        ViewBag.usersellist = ss;
                    }
                }
            }
            //if (DateTime.Compare(start, now) > 0/* && DateTime.Compare(end, now) < 0*/)
            //    state = true;
            //TimeSpan ts = now - start;

            //ViewBag.now = now;
            //ViewBag.start = start;
            //ViewBag.end = end;
            //ViewBag.state = state;

        }

        //#region Create
        [HttpGet]
        [Route("Add")]
        public ActionResult Create()
        {
            Requiredneeds(true);
            return View();
        }
        public async Task<ActionResult> SelectElectionCource(int? id)
        {
            if (id == null) return Content(null);
            string resultmsg = id.Value.ToString();
            string userName = HttpContext.User.Identity.Name;
            Term term = _SettingService.GetOptionsForEdit().Term;
            Student stu = _StudentService.GetByUserId(_UserService.GetUserByUserName(userName).Id);
            PeresentedCourses pc = _PeresentedCoursesService.GetById(id.Value);
          
                ElectionStatus status = _ElectionService.Choosen(stu.Id, pc.Id);
                switch (status)
                {
                    case ElectionStatus.Success:
                        {
                            var adv = new Election
                            {
                                Student = _StudentService.GetById(stu.Id),
                                PeresentedCource = pc
                            };
                            _ElectionService.Insert(adv);

                            _PeresentedCoursesService.Decrease_Capacity_Remained(pc.Id);
                            await _unitOfWork.SaveAllChangesAsync(false);

                            resultmsg = "Success";
                        }
                        break;
                    case ElectionStatus.CannotSelectCapacityFull:
                        resultmsg = "Cannot Select Beacuse Capacity of Class is Full";
                        break;
                    case ElectionStatus.CannotSelectOutOfUnit:
                        resultmsg = "Cannot Select Beacuse Out Of Your Max Unit Allowed";
                        break;
                    case ElectionStatus.CannotSelectPrevSelected:
                        resultmsg = "Cannot Select Beacuse You Pereviouse Choose This Class";
                        break;
                }

                await _unitOfWork.SaveChangesAsync();
                CacheManager.InvalidateChildActionsCache();
            return Json(new { msg = resultmsg }, JsonRequestBehavior.AllowGet);
        }

        //#region Create
        [HttpGet]
        [Route("Remove")]
        public ActionResult Remove()
        {
            Requiredneeds(false);
            return View();
        }
        public async Task<ActionResult> SelectRemoveCource(int? id)
        {
            if (id == null) return Content(null);
            string resultmsg = id.Value.ToString();
            Election ele = _ElectionService.GetById(id.Value);
            PeresentedCourses pc = _PeresentedCoursesService.GetById(ele.PeresentedCource.Id);
            ElectionRemoveStatus status = _ElectionService.Remove(ele.Id);
            switch (status)
            {
                case ElectionRemoveStatus.Success:
                    {
                        _ElectionService.Delete(id.Value);

                        _PeresentedCoursesService.Increase_Capacity_Remained(pc.Id);
                        await _unitOfWork.SaveAllChangesAsync(false);

                        resultmsg = "Success Remove";
                    }
                    break;
                case ElectionRemoveStatus.CannotRemoveScored:
                    resultmsg = "Cannot Remove Beacuse This Cource has Scored";
                    break;
            }

            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();
            return Json(new { msg = resultmsg }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Add")]
        public virtual async Task<ActionResult> Create(AddElectionViewModel viewModel)
        {
            Requiredneeds(true);
            if (ModelState.IsValid)
            {
                try
                {
                    var adv = new Election
                    {
                        Student = _StudentService.GetById(viewModel.Student_Id), 
                        PeresentedCource = _PeresentedCoursesService.GetById(viewModel.PeresentedCource_Id)
                    };
                    ElectionStatus status = _ElectionService.Choosen(viewModel.Student_Id, viewModel.PeresentedCource_Id);
                    switch (status)
                    {
                        case ElectionStatus.Success:
                            {
                                _ElectionService.Insert(adv);

                                _PeresentedCoursesService.Decrease_Capacity_Remained(viewModel.PeresentedCource_Id);
                                await _unitOfWork.SaveAllChangesAsync(false);
                                
                                return RedirectToAction("Index", "Home");
                            }
                        case ElectionStatus.CannotSelectCapacityFull:
                            ModelState.AddModelError("UserName", "Cannot Select Beacuse Capacity of Class is Full");
                            break;
                        case ElectionStatus.CannotSelectOutOfUnit:
                            ModelState.AddModelError("UserName", "Cannot Select Beacuse Out Of Your Max Unit Allowed");
                            break;
                        case ElectionStatus.CannotSelectPrevSelected:
                            ModelState.AddModelError("UserName", "Cannot Select Beacuse You Pereviouse Choose This Class");
                            break;
                    }

                    await _unitOfWork.SaveChangesAsync();
                    CacheManager.InvalidateChildActionsCache();
                    return RedirectToAction("Index", "Enrolment");
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
            var product = _ElectionService.GetForEdit(id.Value);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Edit/{id}")]
        public virtual async Task<ActionResult> Edit(EditElectionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _ElectionService.Update(viewModel);
                await _unitOfWork.SaveChangesAsync();
                CacheManager.InvalidateChildActionsCache();
                return RedirectToAction("Index", "Enrolment");
            }
            
            return View(viewModel);
        }
        //#endregion

        //#region Delete
        public virtual async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return Content(null);
            string resultmsg = "";
            _ElectionService.Delete(id.Value);
            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();
            resultmsg = "Remove Sucessful";
            return Json(new { msg = resultmsg }, JsonRequestBehavior.AllowGet);
        }
        //#endregion

    }
}