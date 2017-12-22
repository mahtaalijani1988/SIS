using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.Utilities.Security;
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
    [RoutePrefix("Professor")]
    [SiteAuthorize(Roles = "admin")]
    [Route("{action}")]
    public class ProfessorController : Controller
    {
        //#region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfessorService _ProfessorService;
        private readonly IGroupsService _GroupsService;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        //#endregion

        //#region Constructor
        public ProfessorController(IUnitOfWork unitOfWork, IProfessorService artService
            ,IGroupsService gr, IRoleService ro, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _ProfessorService = artService;
            _GroupsService = gr;
            _roleService = ro;
            _userService = userService;
        }
        //#endregion
        // GET: Professor
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0)]
        public virtual ActionResult List(string term = "", int page = 1, int count = 10,
            Order order = Order.Descending, ProfessorSearchBy objSearchBy = ProfessorSearchBy.FirstName)
        {
            //#region Retrive Data
            int total;
            var articles = _ProfessorService.GetDataTable(out total, term, page, order, objSearchBy, count);
            var model = new ProfessorListVM
            {
                Order = order,
                PageCount = count,
                PageNumber = page,
                ProfessorList = articles,
                Term = term,
                TotalProfessors = total
            };
            ViewBag.CountList = DropDown.GetCountList(count);
            ViewBag.OrderList = DropDown.GetOrderList(order);
            return PartialView("_ListPartial", model);
        }


        void PopulateGroupsDropDownList(int? selectedId)
        {
            var allcources = _GroupsService.GetAllGroups();
            ViewBag.AllGroups = new SelectList(allcources, "Id", "Name", selectedId);
        }
        //#region Create
        [HttpGet]
        [Route("Add")]
        public ActionResult Create()
        {
            PopulateGroupsDropDownList(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Add")]
        public virtual async Task<ActionResult> Create(AddProfessorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(!string.IsNullOrEmpty(viewModel.PNO))
                    {
                        if(_ProfessorService.CheckPNO_Exist(viewModel.PNO))
                        {
                            PopulateGroupsDropDownList(viewModel.Group_Id);
                            ModelState.AddModelError("PNO", "This Professor.NO Is Already Exist In Database");
                            return View(viewModel);
                        }
                    }
                    var newUser = new User
                    {
                        RegisterType = UserRegisterType.Active,
                        IsBaned = false,
                        IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                        RegisterDate = DateTime.Now,
                        LastLoginDate = DateTime.Now,
                        UserName = viewModel.UserName,
                        Email = viewModel.Email,
                        Password = Encryption.EncryptingPassword(viewModel.Password),
                        Role = _roleService.GetRoleByName("professor"),
                        ProfessorData = new Professor
                        {
                            AvatarPath = viewModel.AvatarPath,
                            BirthDay = viewModel.BirthDay,
                            Edution = viewModel.Edution,
                            FirstName = viewModel.FirstName,
                            Gender = viewModel.Gender,
                            LastName = viewModel.LastName,
                            PNO = viewModel.PNO,
                            Tendency = viewModel.Tendency,

                            Group = _GroupsService.GetById(viewModel.Group_Id)
                        }
                    };

                    var addUserStatus = _userService.Add(newUser);
            PopulateGroupsDropDownList(viewModel.Group_Id);
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
                    return RedirectToAction("Index", "Professor");
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
            PopulateGroupsDropDownList(null);
            return View(viewModel);
        }
        //#endregion

        //#region Edit
        [HttpGet]
        [Route("Edit/{id}")]
        public virtual ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = _ProfessorService.GetForEdit(id.Value);
            if (product == null) return HttpNotFound();

            PopulateGroupsDropDownList(product.Group.Id);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Route("Edit/{id}")]
        public virtual async Task<ActionResult> Edit(EditProfessorViewModel viewModel)
        {
            var editedUser = new User
            {
                Id = _userService.GetUserByUserName(viewModel.UserName).Id,
                Password =
                viewModel.Password != null
                    ? Encryption.EncryptingPassword(viewModel.Password) : null,
                Role = _roleService.GetRoleByName("professor"),
                Email = viewModel.Email,
                UserName = viewModel.UserName
            };
            var prod = new EditProfessorViewModel
            {
                Id = viewModel.Id,

                AvatarPath = viewModel.AvatarPath,
                BirthDay = viewModel.BirthDay,
                Edution = viewModel.Edution,
                FirstName = viewModel.FirstName,
                Gender = viewModel.Gender,
                LastName = viewModel.LastName,
                PNO = viewModel.PNO,
                Tendency = viewModel.Tendency,

                Group = _GroupsService.GetById(viewModel.Group_Id),
                Group_Id = viewModel.Group_Id,
                User = editedUser
            };
            _ProfessorService.Update(prod);
            var status = _userService.EditUser(editedUser);
            if (status == EditedUserStatus.UpdatingUserSuccessfully)
            {
                await _unitOfWork.SaveChangesAsync();
                CacheManager.InvalidateChildActionsCache();
                return RedirectToAction("Index", "Professor");
            }
            PopulateGroupsDropDownList(viewModel.Group.Id);
            return View(viewModel);
        }
        //#endregion

        //#region Delete
        public virtual async Task<ActionResult> Delete(long? id)
        {
            if (id == null) return Content(null);
            string resultmsg = "";
            _userService.RemoveByProfessorInfo(id.Value);
            await _unitOfWork.SaveChangesAsync();
            CacheManager.InvalidateChildActionsCache();
            resultmsg = "Remove Sucessful";
            return Json(new { msg = resultmsg }, JsonRequestBehavior.AllowGet);
        }
        //#endregion


    }
}