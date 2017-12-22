using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Term
{
    public class TermViewModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }

        public virtual ICollection<DomainClasses.Entities.PeresentedCourses> PeresentedCourcess { get; set; }
    }
}
