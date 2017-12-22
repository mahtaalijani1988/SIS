using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Election
{
    public class EditElectionViewModel
    { 
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Student is Required.")]
        public virtual DomainClasses.Entities.Student Student { get; set; }
        public virtual long Student_Id { get; set; }

        [Required(ErrorMessage = "Peresented Course is Required.")]
        public virtual DomainClasses.Entities.PeresentedCourses PeresentedCource { get; set; }
        public virtual int PeresentedCource_Id { get; set; }

        public virtual ICollection<DomainClasses.Entities.PeresentedCourses> PeresentedCources { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.#}", ApplyFormatInEditMode = true,
            NullDisplayText = "No grade")]
        public decimal? Score { get; set; }
    }
}
