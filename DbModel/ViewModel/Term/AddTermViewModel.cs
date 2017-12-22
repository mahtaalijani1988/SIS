using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Term
{
    public class AddTermViewModel
    {
        [DisplayName("Term Title")]
        [Required(ErrorMessage = "Title Field is Required.")]
        public string Name { get; set; }
        [DisplayName("Term Start Date")]
        [Required(ErrorMessage = "Start Date Field is Required.")]
        [DataType(DataType.Date, ErrorMessage ="mistake")]
        public DateTime StartDate { get; set; }
        [DisplayName("Term End Date")]
        [Required(ErrorMessage = "End Date Field is Required.")]
        [DataType(DataType.Date, ErrorMessage = "mistake")]
        public DateTime EndDate { get; set; }

        public ICollection<DomainClasses.Entities.PeresentedCourses> PeresentedCourcess { get; set; }
    }
}
