using CaptchaMvc.Infrastructure;
using DbModel.Context;
using DbModel.Context.Migrations;
using DbModel.IocConfig;
using DbModel.ServiceLayer.Interfaces;
using StackExchange.Profiling;
using StructureMap.Web.Pipeline;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Student_Information_System
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);

                MvcHandler.DisableMvcResponseHeader = true;
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new RazorViewEngine());
                CaptchaUtils.CaptchaManager.StorageProvider = new CookieStorageProvider();

                // برای مقدار دهی اولیه
                //SetDbInitializer();
                ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
                // LuceneProducts.CreateIndexes(SampleObjectFactory.Container.GetInstance<IProductService>().GetAllForAddLucene());

            }
            catch
            {
                HttpRuntime.UnloadAppDomain(); // سبب ری استارت برنامه و آغاز مجدد آن با درخواست بعدی می‌شود
                throw;
            }
        }


        private bool ShouldIgnoreRequest()
        {
            string[] reservedPath =
            {
                "/__browserLink",
                "/img",
                "/fonts",
                "/Scripts",
                "/Content",
                "/Uploads"
            };

            var rawUrl = Context.Request.RawUrl;
            if (reservedPath.Any(path => rawUrl.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return BundleTable.Bundles.Select(bundle => bundle.Path.TrimStart('~'))
                      .Any(bundlePath => rawUrl.StartsWith(bundlePath, StringComparison.OrdinalIgnoreCase));
        }
        public class StructureMapControllerFactory : DefaultControllerFactory
        {
            protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                if (controllerType == null)
                {
                    //var url = requestContext.HttpContext.Request.RawUrl;
                    ////string.Format("Page not found: {0}", url).LogException();

                    //requestContext.RouteData.Values["controller"] = MVC.Search.Name;
                    //requestContext.RouteData.Values["action"] = MVC.Search.ActionNames.Index;
                    //requestContext.RouteData.Values["keyword"] = url.GenerateSlug().Replace("-", " ");
                    //requestContext.RouteData.Values["categoryId"] = 0;
                    //return SampleObjectFactory.Container.GetInstance(typeof(SearchController)) as Controller;
                    throw new InvalidOperationException(string.Format("Page not found: {0}", requestContext.HttpContext.Request.RawUrl));
                }

                return SampleObjectFactory.Container.GetInstance(controllerType) as Controller;
            }
        }


        // برای مقدار دهی اولیه
        private static void SetDbInitializer()
        {
            //Database.SetInitializer<ShopDbContext>(null);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyDbContext, Configuration>());
            SampleObjectFactory.Container.GetInstance<IUnitOfWork>().ForceDatabaseInitialize();
        }

        //#endregion

        //#region Application_PreSendRequestHeaders
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app == null || app.Context == null)
                return;
            app.Context.Response.Headers.Remove("Server");
        }
        //#endregion

        //#region Application_AuthenticateRequest
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (ShouldIgnoreRequest()) return;

            if (Context.User == null)
                return;

            var userService = SampleObjectFactory.Container.GetInstance<IUserService>();

            var userStatus = userService.GetStatus(Context.User.Identity.Name);

            if (userStatus.IsBaned)
                FormsAuthentication.SignOut();

            var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
                return;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);


            // retrieve roles from UserData
            if (authTicket == null) return;
            var roles = authTicket.UserData.Split(',');

            if (userStatus.Role != roles[0])
                FormsAuthentication.SignOut();

            Context.User = new GenericPrincipal(Context.User.Identity, roles);
        }
        //#endregion

        //#region Application_EndRequest
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();
            MiniProfiler.Stop();
        }
        //#endregion

        //#region Application_BeginRequest
        private void Application_BeginRequest(object sender, EventArgs e)
        {

            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }
        //#endregion


    }
}
