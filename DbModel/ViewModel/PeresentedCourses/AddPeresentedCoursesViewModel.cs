using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.PeresentedCourses
{
    public class AddPeresentedCoursesViewModel
    {
        [Required(ErrorMessage = "Capacity is Required.")]
        [Range(1,100, ErrorMessage = "Capacity must be Between 1 and 100")]
        public virtual int Capacity { get; set; }

        public virtual int Remain_Capacity { get; set; }
        
        [Required(ErrorMessage = "Course is Required.")]
        public virtual DomainClasses.Entities.Course Course { get; set; }
        public virtual int? Course_Id { get; set; }

        [Required(ErrorMessage = "Professor is Required.")]
        public virtual DomainClasses.Entities.Professor Professor { get; set; }
        public virtual long? Professor_Id { get; set; }

        [Required(ErrorMessage = "Term is Required.")]
        public virtual DomainClasses.Entities.Term Term { get; set; }
        public virtual int? Term_Id { get; set; }

        public virtual ICollection<DomainClasses.Entities.Election> Elections { get; set; }
    }
}
