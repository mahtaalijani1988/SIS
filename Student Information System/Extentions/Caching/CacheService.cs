using System;
using System.Configuration;
using System.Web;
using DbModel.ViewModel.Setting;
using Student_Information_System.Extentions.Infrastructure;
using DbModel.ServiceLayer.Interfaces;

namespace Student_Information_System.Extentions.Caching
{
    public class CacheService : ICacheService
    {
        public const string SiteConfigKey = "SiteConfig";

        private readonly HttpContextBase _httpContext;
        private readonly ISettingService _optionService;

        public CacheService(HttpContextBase httpContext, ISettingService optionService)
        {
            _httpContext = httpContext;
            _optionService = optionService;
        }

        public SiteConfig GetSiteConfig()
        {
            var siteConfig = _httpContext.CacheRead<SiteConfig>(SiteConfigKey);
            var durationMinutes =
                Convert.ToInt32(ConfigurationManager.AppSettings["CacheOptionsDuration"]);

            if (siteConfig != null) return siteConfig;

            siteConfig = _optionService.GetAll();
            _httpContext.CacheInsert(SiteConfigKey, siteConfig, durationMinutes);

            return siteConfig;
        }

        //public string getAdminMail()
        //{
        //    SiteConfig AllOptions = _optionService.GetAll();
        //    return AllOptions.AdminEmail;
        //}
        public SiteConfig getAllConfigurations()
        {
            return _optionService.GetAll();
        }

        public void RemoveSiteConfig()
        {
            _httpContext.InvalidateCache(SiteConfigKey);
        }

    }
}