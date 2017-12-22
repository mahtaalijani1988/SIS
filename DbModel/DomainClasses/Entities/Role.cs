using System.Collections.Generic;

namespace DbModel.DomainClasses.Entities
{
    public class Role
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<User>  Users{ get; set; }
        public virtual string Description { get; set; }
    }
}
