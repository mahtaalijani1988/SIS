using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.DomainClasses.Entities
{
    public class Election
    {
        public virtual int Id { get; set; }
        public virtual Student Student { get; set; }

        public virtual PeresentedCourses PeresentedCource { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.#}", ApplyFormatInEditMode = true,
            NullDisplayText = "No grade")]
        public decimal? Score { get; set; }
    }
}
