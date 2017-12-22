using System;
using System.Configuration;
using System.Web;
using DbModel.ServiceLayer.Interfaces;
using Student_Information_System.Extentions.Infrastructure;
using DbModel.ViewModel.Setting;

namespace Student_Information_System.Extentions.Caching
{
    public class Student_Information_SystemCache
    {
        public const string SiteConfigKey = "SiteConfig";

        public static SiteConfig GetSiteConfig(HttpContextBase httpContext, ISettingService optionService)
        {
            var siteConfig = httpContext.CacheRead<SiteConfig>(SiteConfigKey);
            int durationMinutes =
                Convert.ToInt32(ConfigurationManager.AppSettings["CacheOptionsDuration"]);

            if (siteConfig == null)
            {
                siteConfig = optionService.GetAll();
                httpContext.CacheInsert(SiteConfigKey, siteConfig, durationMinutes);
            }
            return siteConfig;
        }

        public static void RemoveSiteConfig(HttpContextBase httpContext)
        {
            httpContext.InvalidateCache(SiteConfigKey);
        }
    }
}