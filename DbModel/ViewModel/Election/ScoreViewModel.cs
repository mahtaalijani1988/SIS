using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Election
{
    public class ScoreViewModel
    {
        public virtual int Id { get; set; }
        public virtual DomainClasses.Entities.Student Student { get; set; }

        public virtual DomainClasses.Entities.PeresentedCourses PeresentedCource { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.#}", ApplyFormatInEditMode = true,
            NullDisplayText = "No grade")]
        public decimal? Score { get; set; }

        public decimal? Average { get; set; }
    }
}
