using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DbModel.ViewModel.User
{
    public class LoginViewModel
    {
        [DisplayName("UserName")]
        [Required(ErrorMessage = "UserName is Required")]
      //  [RegularExpression("09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}", ErrorMessage = "لطفا شماره همراه خود را به شکل صحیح وارد کنید")]
        public string UserName { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
