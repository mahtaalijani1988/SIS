using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.ViewModel.Groups
{
    public class AddGroupsViewModel
    {
        [Required(ErrorMessage = "Name is Required.")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Manager Name is Required.")]
        public virtual string Manager { get; set; }

        public virtual ICollection<DomainClasses.Entities.Professor> Professors { get; set; }
    }
}
