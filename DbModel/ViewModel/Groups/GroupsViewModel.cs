using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Groups
{
    public class GroupsViewModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Manager { get; set; }

        public virtual ICollection<DomainClasses.Entities.Professor> Professors { get; set; }
    }
}
