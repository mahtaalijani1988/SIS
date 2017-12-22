using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.DomainClasses.Entities
{
    public class GroupCourses
    {
        public virtual int Id { get; set; }
        public virtual Course Course { get; set; }

    }
}
