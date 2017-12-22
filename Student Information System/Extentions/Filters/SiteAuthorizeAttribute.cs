using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Student_Information_System.Extentions.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class SiteAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                //throw new UnauthorizedAccessException(); //to avoid multiple redirects
                //filterContext.Result = new HttpStatusCodeResult(403);
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "action", "Login" },
                        { "controller", "User" },
                        { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                    });
            }
            else
            {
               // handleAjaxRequest(filterContext);
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        private static void handleAjaxRequest(AuthorizationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            if (!ctx.Request.IsAjaxRequest())
                return;

            ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden; //براي درخواست‌هاي اجكسي اعتبار سنجي نشده
            ctx.Response.End();
        }
    }
}