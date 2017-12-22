using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.DomainClasses.Entities
{
    public class PeresentedCourses
    {
        public virtual int Id { get; set; }
        public virtual Course Course { get; set; }
        public virtual Professor Professor { get; set; }
        public virtual Term Term { get; set; }

        public virtual int Capacity { get; set; }
        public virtual int Remain_Capacity { get; set; }

        public virtual ICollection<Election> Elections { get; set; }

    }
}
