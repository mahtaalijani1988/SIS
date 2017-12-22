using CaptchaMvc.HtmlHelpers;
using DbModel.Context;
using DbModel.DomainClasses.Entities;
using DbModel.DomainClasses.Enums;
using DbModel.ServiceLayer.Interfaces;
using DbModel.Utilities.Security;
using DbModel.ViewModel;
using DbModel.ViewModel.User;
using Student_Information_System.Extentions.Filters;
using Student_Information_System.Extentions.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace Student_Information_System.Controllers
{
    [RoutePrefix("Customer")]
    [Route("{action}")]
    public class UserController : Controller
    {
        //#region Fields
        private readonly IRoleService _roleService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        //#endregion //Fields

        //#region Constructor
        public UserController(IUserService userService, IUnitOfWork unitOfWork, IRoleService roleService)
        {
            _roleService = roleService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        //#region Index,List
        [HttpGet]
        public virtual ActionResult Index()
        {
            ViewBag.UserSearchByList = DropDown.GetUserSearchByList(UserSearchBy.PhoneNumber);
            return View();
        }
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public virtual ActionResult List(string term = "", int pageNumber = 1, int pageCount = 10,
            Order order = Order.Descending, UserOrderBy userOrderBy
            = UserOrderBy.RegisterDate, UserSearchBy userSearchBy = UserSearchBy.PhoneNumber)
        {
            //#region Retrive Data
            int totalUsers;
            var users = _userService.GetDataTable(out totalUsers, term, pageNumber, pageCount, order, userOrderBy, userSearchBy);
            var model = new UsersListViewModel
            {
                UserOrderBy = userOrderBy,
                Term = term,
                PageNumber = pageNumber,
                Order = order,
                UsersList = users,
                TotalUsers = totalUsers,
                PageCount = pageCount
            };
            //#endregion
            ViewBag.UserSearchByList = DropDown.GetUserSearchByList(userSearchBy);
            ViewBag.UserOrderByList = DropDown.GetUserOrderByList(userOrderBy);
            ViewBag.CountList = DropDown.GetCountList(pageCount);
            ViewBag.OrderList = DropDown.GetOrderList(order);
            ViewBag.UserSearchBy = userSearchBy;
            return PartialView("_ListPartial", model);
        }


        [HttpGet]
        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        public virtual async Task<ActionResult> Login(LoginViewModel viewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var userName = string.Empty;
            int userId = 0;
            var ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            var verificationResult =
                _userService.VerifyUserByUserName(viewModel.UserName, viewModel.Password,
                    ref userName, ref userId, ip);


            switch (verificationResult)
            {
                case VerifyUserStatus.VerifiedSuccessfully:
                    {
                        var roleOfTheUser = await _roleService.GetRoleByUserId(userId);

                        // set user role cookie
                        SetAuthCookie(userName, roleOfTheUser.Name, viewModel.RememberMe);

                        await _unitOfWork.SaveAllChangesAsync(false);
                        if (Request.IsAjaxRequest())
                            return JavaScript(IsValidReturnUrl(returnUrl)
                                ? string.Format("window.location ='{0}';", returnUrl)
                                : "window.location.reload();");
                        if (IsValidReturnUrl(returnUrl))
                            return Redirect(returnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                case VerifyUserStatus.UserIsBaned:
                    ModelState.AddModelError("UserName", "Your account is blocked");
                    ModelState.AddModelError("Password", "Your account is blocked");
                    break;
                default:
                    ModelState.AddModelError("UserName", "The information entered is not correct");
                    ModelState.AddModelError("Password", "The information entered is not correct");
                    break;
            }
            return View(viewModel);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            var roles = _roleService.GetAllRoles().Select(x =>
              new SelectListItem { Text = x.Description, Value = x.Id.ToString(CultureInfo.InvariantCulture) });
            ViewBag.Roles = roles;
            AddUserViewModel model = new AddUserViewModel();
            model.IsBaned = false;
            model.RegisterType = UserRegisterType.Active;
            model.RegisterDate = DateTime.Now;
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual async Task<ActionResult> Create(AddUserViewModel viewModel)
        {
            var roles = _roleService.GetAllRoles().Select(x =>
              new SelectListItem { Text = x.Description, Value = x.Id.ToString(CultureInfo.InvariantCulture) });
            ViewBag.Roles = roles;

            if (!ModelState.IsValid)
                return View(viewModel);
            var user = new User
            {
                RegisterType = UserRegisterType.Active,
                IsBaned = false,
                Email = viewModel.Email,
                IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                RegisterDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                UserName = viewModel.UserName,
                Password = Encryption.EncryptingPassword(viewModel.Password),
                Role = _roleService.GetRoleByRoleId(viewModel.RoleId)
            };

            var addUserStatus = _userService.Add(user);
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
            return View(viewModel);
        }
        //#region Edit User
        [Route("Edit/{id}")]
        [HttpGet]
        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = _userService.GetUserDataForEdit(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.Roles = new SelectList(_roleService.GetAllRoles(), "Id", "Description", user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{id}")]
        public virtual async Task<ActionResult> Edit(EditUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            var editedUser = new User
            {
                Id = viewModel.Id,
                Password =
                    viewModel.Password != null
                        ? Encryption.EncryptingPassword(viewModel.Password) : null,
                Role = _roleService.GetRoleByRoleId(viewModel.RoleId),
                IsBaned = viewModel.IsBaned,
                Email = viewModel.Email,
                UserName = viewModel.UserName
            };

            var status = _userService.EditUser(editedUser);
            if (status == EditedUserStatus.UpdatingUserSuccessfully)
            {
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            switch (status)
            {
                case EditedUserStatus.EmailExist:
                    ModelState.AddModelError("Email", "This Email Is Already Exist In Database");
                    return View(viewModel);
                case EditedUserStatus.UserNameExist:
                    ModelState.AddModelError("UserName", "This UserName Is Already Exist In Database");
                    return View(viewModel);
            }

            return View(viewModel);
        }
        //#endregion



        [HttpGet]
        public virtual ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        public virtual async Task<ActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var newUser = new User
            {
                RegisterDate = DateTime.Now,
                IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                IsBaned = false,
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                Password = viewModel.Password,
                Role = _roleService.GetRoleByName("user"),
                LastLoginDate = DateTime.Now
            };

            var addingNewUserResult = _userService.Add(newUser);

            switch (addingNewUserResult)
            {
                case AddUserStatus.EmailExist:
                    ModelState.AddModelError("Email", "This Email Is Already Exist In Database.");
                    return View(viewModel);
                case AddUserStatus.UserNameExist:
                    ModelState.AddModelError("UserName", "This UserName Is Already Exist In Database.");
                    return View(viewModel);
            }

            await _unitOfWork.SaveAllChangesAsync(false);
            return View();
        }


        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "*")]
        public virtual async Task<ActionResult> ForgetPassword(ForgetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            User thuser = _userService.GetUserByEmail(viewModel.Email);

            await _unitOfWork.SaveAllChangesAsync(false);
            if (true) return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        //#region Vaidation return url
        [NonAction]
        private bool IsValidReturnUrl(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1;
        }
        //#endregion //Validation return url
        //#region Authentication
        [NonAction]
        private void SetAuthCookie(string userName, string roleofUser, bool presistantCookie)
        {
            var timeout = presistantCookie ? FormsAuthentication.Timeout.TotalMinutes : 200;

            var now = DateTime.UtcNow.ToLocalTime();
            var expirationTimeSapne = TimeSpan.FromMinutes(timeout);

            var authTicket = new FormsAuthenticationTicket(
                1,
                userName,
                now,
                now.Add(expirationTimeSapne),
                presistantCookie,
                roleofUser,
                FormsAuthentication.FormsCookiePath
                );

            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (FormsAuthentication.CookieDomain != null)
            {
                authCookie.Domain = FormsAuthentication.CookieDomain;
            }

            if (presistantCookie)
                authCookie.Expires = DateTime.Now.AddMinutes(timeout);

            Response.Cookies.Add(authCookie);
        }
        //#endregion //Authentication

    }
}