using DbModel.ViewModel.Setting;

namespace Student_Information_System.Extentions.Caching
{
    public interface ICacheService
    {
        SiteConfig GetSiteConfig();
        void RemoveSiteConfig();
        SiteConfig getAllConfigurations();
    }
}