using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DbModel.ViewModel.User
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        [DisplayName("UserName")]
        [Required(ErrorMessage = "UserName is Required.")]
        [MinLength(5, ErrorMessage = "UserName must be longer than 5 characters")]
        [MaxLength(30, ErrorMessage = "UserName must be less than 30 characters")]
        [Remote("EditCheckUserNameIsExist", "User", "Admin", ErrorMessage = "This username has already been selected by members",
            HttpMethod = "POST",AdditionalFields = "Id")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required.")]
        [MaxLength(15, ErrorMessage = "Password must be less than 15 characters")]
        [MinLength(6, ErrorMessage = "Password must be longer than 65 characters")]
        public string Password { get; set; }
        [DisplayName("ConfirmPassword")]
        [System.ComponentModel.DataAnnotations.Compare("Password", 
            ErrorMessage = "Passwords and replies should be the same")]
        public string ConfirmPassword { get; set; }
        [DisplayName("Role")]
        public long RoleId { get; set; }

        [DisplayName("IsBaned")]
        public bool IsBaned { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }

    }
}
