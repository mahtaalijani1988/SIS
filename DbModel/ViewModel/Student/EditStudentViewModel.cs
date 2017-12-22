using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DbModel.ViewModel.Student
{
    public class EditStudentViewModel
    { 
        public virtual long Id { get; set; }
        
        [Required(ErrorMessage = "Student Number is Required.")]
        public virtual string SNO { get; set; }

        [Required(ErrorMessage = "FirstName is Required.")]
        public virtual string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required.")]
        public virtual string LastName { get; set; }
        public virtual string AvatarPath { get; set; }
        [Required(ErrorMessage = "BirthDay is Required.")]
        public virtual DateTime? BirthDay { get; set; }

        [Required(ErrorMessage = "Gender is Required.")]
        public virtual bool? Gender { get; set; }
        public virtual string City { get; set; }//Make my profile public
        public virtual decimal? Average { get; set; }

        [Required(ErrorMessage = "UserName is Required.")]
        [MinLength(5, ErrorMessage = "UserName must be longer than 5 characters")]
        [MaxLength(30, ErrorMessage = "UserName must be less than 30 characters")]
        [Remote("AdminCheckUserNameIsExistForAdd", "User", "Admin",
            ErrorMessage = "This username has already been selected by members", HttpMethod = "POST")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required.")]
        [MaxLength(15, ErrorMessage = "Password must be less than 15 characters")]
        [MinLength(6, ErrorMessage = "Password must be longer than 65 characters")]
        public string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords and replies should be the same")]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is Invalid")]
        public string Email { get; set; }


        public virtual DomainClasses.Entities.User User { get; set; }
        
    }
}