using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Election
{
    public class SetScoreViewModel
    {
        public virtual int Id { get; set; }
        public virtual DomainClasses.Entities.Student Student { get; set; }
        public virtual long Student_Id { get; set; }

        public virtual DomainClasses.Entities.PeresentedCourses PeresentedCource { get; set; }
        public virtual int PeresentedCource_Id { get; set; }

        public decimal? Score { get; set; }
    }
}
