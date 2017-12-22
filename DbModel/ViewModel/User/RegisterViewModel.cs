using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DbModel.ViewModel.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "UserName is Required.")]
        [MinLength(5, ErrorMessage = "UserName must be longer than 5 characters")]
        [MaxLength(30, ErrorMessage = "UserName must be less than 30 characters")]
        [Remote("CheckUserNameIsExist", "User","", ErrorMessage = "This username has already been selected by members", HttpMethod = "POST")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        [MaxLength(15, ErrorMessage = "Password must be less than 15 characters")]
        [MinLength(6, ErrorMessage = "Password must be longer than 65 characters")]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", 
            ErrorMessage = "Passwords and replies should be the same")]
        public string ConfirmNewPassword { get; set; }


        [Required(ErrorMessage = "Email is Required"), 
         MaxLength(50, ErrorMessage = "UserName must be longer than 50 characters"),
         MinLength(4, ErrorMessage = "UserName must be longer than 4 characters"),
         RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
            ErrorMessage = "Email is Invalid")]
        public string Email { get; set; }
    }
}
