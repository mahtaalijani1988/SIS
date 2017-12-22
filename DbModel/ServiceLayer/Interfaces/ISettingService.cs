
using DbModel.ViewModel;
using DbModel.ViewModel.Setting;

namespace DbModel.ServiceLayer.Interfaces
{
    public interface ISettingService
    {
        void Update(EditSettingViewModel viewModel);
        EditSettingViewModel GetOptionsForEdit();
        SiteConfig GetAll();
    }

}
