using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DbModel.ViewModel.User
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required"),
        MaxLength(50, ErrorMessage = "UserName must be longer than 50 characters"),
        MinLength(4, ErrorMessage = "UserName must be longer than 4 characters"),
        RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
           ErrorMessage = "Email is Invalid")]
        public string Email { get; set; }
    }
}
