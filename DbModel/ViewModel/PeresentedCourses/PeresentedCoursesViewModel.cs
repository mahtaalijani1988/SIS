using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.PeresentedCourses
{
    public class PeresentedCoursesViewModel
    {
        public virtual int Id { get; set; }

        public virtual int Capacity { get; set; }
        public virtual int Remain_Capacity { get; set; }

        public virtual DomainClasses.Entities.Course Course { get; set; }
        public virtual int? Course_Id { get; set; }
        public virtual DomainClasses.Entities.Professor Professor { get; set; }
        public virtual long? Professor_Id { get; set; }
        public virtual DomainClasses.Entities.Term Term { get; set; }
        public virtual int? Term_Id { get; set; }

        public virtual ICollection<DomainClasses.Entities.Election> Elections { get; set; }
    }
}
