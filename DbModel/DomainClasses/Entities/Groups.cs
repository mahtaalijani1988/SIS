using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.DomainClasses.Entities
{
    public class Groups
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Manager { get; set; }

        public virtual ICollection<Professor> Professors { get; set; }

    }
}
