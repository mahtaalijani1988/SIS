using System;
using System.ComponentModel;
namespace DbModel.ViewModel.User
{
    public class DetailsUserViewModel
    {
        [DisplayName("RoleName")]
        public string RoleName { get; set; }
        [DisplayName("UserName")]
        public string UserName { get; set; }
        [DisplayName("IP")]
        public string IP { get; set; }
        [DisplayName("RegisterType")]
        public string RegisterType { get; set; }
        
        [DisplayName("BanedDate")]
        public DateTime? BanedDate { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}
