using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.Utilities.Security;
using DbModel.ViewModel.Student;
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
    [RoutePrefix("Student")]
    [SiteAuthorize(Roles = "admin")]
    [Route("{action}")]
    public class StudentController : Controller
    {
        //#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentService _StudentService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        //#endregion

        //#region Constructor
        public StudentController(IUnitOfWork unitOfWork, IStudentService artService
            ,IRoleService ro, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _StudentService = artService;
            _roleService = ro;
            _userService = userService;
        }
        //#endregion
        // GET: Student
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual ActionResult List(string term = "", int page = 1, int count = 10,
            Order order = Order.Descending, StudentSearchBy objSearchBy = StudentSearchBy.SNO)
        {
            //#region Retrive Data
            int total;
            var articles = _StudentService.GetDataTable(out total, term, page, order, objSearchBy, count);
            var model = new StudentsListVM
            {
                Order = order,
                PageCount = count,
                PageNumber = page,
                StudentList = articles,
                Term = term,
                TotalStudents = total
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
        public virtual async Task<ActionResult> Create(AddStudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(viewModel.SNO))
                    {
                        if (_StudentService.CheckSNO_Exist(viewModel.SNO))
                        {
                            ModelState.AddModelError("SNO", "This Student.NO Is Already Exist In Database");
                            return View(viewModel);
                        }
                    }
                    var newUser = new User
                    {
                        RegisterType = UserRegisterType.Active,
                        IsBaned = false,
                        Email = viewModel.Email,
                        IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                        RegisterDate = DateTime.Now,
                        LastLoginDate = DateTime.Now,
                        UserName = viewModel.UserName,
                        Password = Encryption.EncryptingPassword(viewModel.Password),
                        Role = _roleService.GetRoleByName("student"),
                        StudentData = new Student
                        {
                            AvatarPath = viewModel.AvatarPath,
                            Average = viewModel.Average,
                            BirthDay = viewModel.BirthDay,
                            City = viewModel.City,
                            //Elections = viewModel.Elections,
                            FirstName = viewModel.FirstName,
                            Gender = viewModel.Gender,
                            LastName = viewModel.LastName,
                            SNO = viewModel.SNO
                        }
                    };
                    var addUserStatus = _userService.Add(newUser);
                    if (addUserStatus == AddUserStatus.AddingUserSuccessfully)
                    {
                        await _unitOfWork.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    switch (addUserStatus)
                    {
                        case AddUserStatus.EmailExist:
                            ModelState.AddModelError("Email", "This Email Is Already Exist In Database");
                            return View(viewModel);
                        case AddUserStatus.UserNameExist:
                            ModelState.AddModelError("UserName", "This UserName Is Already Exist In Database");
                            return View(viewModel);
                    }
                    await _unitOfWork.SaveChangesAsync();
                    CacheManager.InvalidateChildActionsCache();
                    return RedirectToAction("Index", "Student");
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

        //#region Edit
        [HttpGet]
        [Route("Edit/{id}")]
        public virtual ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = _StudentService.GetForEdit(id.Value);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Edit/{id}")]
        public virtual async Task<ActionResult> Edit(EditStudentViewModel viewModel)
        {
                    var editedUser = new User
                    {
                        Id = _userService.GetUserByUserName(viewModel.UserName).Id,
                        Password =
                            viewModel.Password != null
                                ? Encryption.EncryptingPassword(viewModel.Password) : null,
                        Role = _roleService.GetRoleByName("student"),
                        Email = viewModel.Email,
                        UserName = viewModel.UserName
                    };
            var stt = new EditStudentViewModel
            {
                Id = viewModel.Id,

                AvatarPath = viewModel.AvatarPath,
                BirthDay = viewModel.BirthDay,
                Average = viewModel.Average,
                FirstName = viewModel.FirstName,
                Gender = viewModel.Gender,
                LastName = viewModel.LastName,
                SNO = viewModel.SNO,
                City = viewModel.City,
                
                User = editedUser
            };
            _StudentService.Update(stt);
                    var status = _userService.EditUser(editedUser);
                    if (status == EditedUserStatus.UpdatingUserSuccessfully)
                    {
                        await _unitOfWork.SaveChangesAsync();
                        CacheManager.InvalidateChildActionsCache();
                        return RedirectToAction("Index", "Student");
                    }
            return View(viewModel);
        }
        //#endregion

        //#region Delete
        public virtual async Task<ActionResult> Delete(long? id)
        {
            if (id == null) return Content(null);
            string resultmsg = "";
            _userService.RemoveByStudentInfo(id.Value);
            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();
            resultmsg = "Remove Sucessful";
            return Json(new { msg = resultmsg }, JsonRequestBehavior.AllowGet);
        }
        //#endregion

    }
}