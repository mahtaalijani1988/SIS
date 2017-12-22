using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DbModel.DomainClasses.Enums
{
    public enum AddStatus
    {
        Successful,
        Unsuccessful
    }
    public enum EditStatus
    {
        Successful,
        Unsuccessful
    }
    public enum UserOperations
    {
        [Display(Name = "Collective work")]
        AllWorks,
        [Display(Name = "Delete")]
        DeleteSelectedUses,
        [Display(Name = "Lock")]
        BanedSelectedUsers,
        [Display(Name = "Active")]
        UnBanedSelectedUsers
    }
    public enum Roles
    {
        [Display(Name = "Change Role")]
        NoRole,
        [Display(Name = "Administrator")]
        Admin//,
        //[Display(Name = "مشتری")]
        //Customer
    }
    public enum UserOrderBy
    {

        [Display(Name = "User Name")]
        UserName,
        [Display(Name = "Register Date")]
        RegisterDate//,
        //[Display(Name = "تعداد خرید")]
        //OrderCount
    }

    public enum UserSearchBy
    {
        [Display(Name = "User Name")]
        UserName,
        [Display(Name = "Phone Number")]
        PhoneNumber,
        [Display(Name = "Ip")]
        Ip,
        [Display(Name = "RoleDescription")]
        RoleDescription
    }
    public enum VerifyUserStatus
    {
        VerifiedSuccessfully,
        UserIsBaned,
        VerifiedFaild
    }

    public enum AddUserStatus
    {
        EmailExist,
        UserNameExist,
        PhoneNumberExist,
        AddingUserSuccessfully
    }
    public enum EditedUserStatus
    {
        EmailExist,
        UserNameExist,
        PhoneNumberExist,
        UpdatingUserSuccessfully
    }

    public enum ChangePasswordResult
    {
        ChangedSuccessfully,
        ChangedFaild
    }


    public enum Order
    {
        [Display(Name = "Asscending")]
        Asscending,
        [Display(Name = "Descending")]
        Descending
    }

    public enum PageCount
    {
        [Display(Name = "10")]
        Count10 = 10,
        [Display(Name = "30")]
        Count30 = 30,
        [Display(Name = "50")]
        Count50 = 50
    }
    
    public enum CacheControlType
    {
        [Description("public")]
        Public,
        [Description("private")]
        Private,
        [Description("no-cache")]
        Nocache,
        [Description("no-store")]
        Nostore
    }
    public enum OrderStatus
    {
        Delivered,
        Posted,
        Seen,
        NoSeen
    }

    public enum ElectionStatus
    {
        CannotSelectPrevSelected,
        CannotSelectOutOfUnit,
        CannotSelectCapacityFull,
        Success
    }
    public enum ElectionRemoveStatus
    {
        CannotRemoveScored,
        Success
    }

    public enum UserRegisterType
    {
        Active,
        NotActive
    }
    public enum ScoreStateType
    {
        Score,
        NotScore
    }

    public enum AttributeType
    {
        Checkbox,
        Text
    }

    public enum ElectionSearchBy
    {
        StudentName,
        Peresented_Cource_Name,
        Score
    }
    public enum ElectionOrderBy
    {
        Id,
        Score
    }
    public enum GroupsSearchBy
    {
        Name,
        Manager
    }
    public enum PeresentedCoursesSearchBy
    {
        Cource_Name,
        Professor_Name,
        Term
    }
    public enum PeresentedCoursesOrderBy
    {
        Id,
        Cource,
        Term
    }
    public enum ProfessorSearchBy
    {
        PNO,
        FirstName,
        LastName,
        BirthDate,
        Tendency,
        Education,

    }
    public enum StudentSearchBy
    {
        SNO,
        FirstName,
        LastName,
        BirthDate,
        Average,
        City,

    }
    public enum TermSearchBy
    {
        Name,
        StartDate,
        EndDate
    }

}
