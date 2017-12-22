using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Election
{
    public class AverageClass
    {
        public virtual DomainClasses.Entities.Student Student { get; set; }
        public virtual long Student_Id { get; set; }

        public virtual DomainClasses.Entities.Term Term { get; set; }
        public virtual int? Term_Id { get; set; }

        public decimal? Average { get; set; }
    }
}
