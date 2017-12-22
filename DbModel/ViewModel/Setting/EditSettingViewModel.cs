using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DbModel.ViewModel.Setting
{
    public class EditSettingViewModel
    {
        [Required(ErrorMessage = "Term is Required.")]
        public virtual DomainClasses.Entities.Term Term { get; set; }
        public virtual int? Term_Id { get; set; }
        public virtual string Term_Name { get; set; }


        public string Department_Name { get; set; }

        public string Start_Election_Date { get; set; }
        public string Start_Election_Time { get; set; }
        public string End_Election_Date { get; set; }
        public string End_Election_Time { get; set; }

        public string Start_Remove_Date { get; set; }
        public string Start_Remove_Time { get; set; }
        public string End_Remove_Date { get; set; }
        public string End_Remove_Time { get; set; }

        [Required(ErrorMessage = "Capacity is Required.")]
        [Range(1, 30, ErrorMessage = "Capacity must be Between 1 and 30")]
        public string Student_max_Unit { get; set; }

    }
}
