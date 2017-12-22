using DbModel.DomainClasses.Entities;
using System.Data.Entity.ModelConfiguration;

namespace DbModel.DomainClasses.Configuration
{
    public class RoleConfig:EntityTypeConfiguration<Role>
    {
        public RoleConfig()
        {
            Property(x => x.Name).HasMaxLength(20);
            Property(x => x.Description).HasMaxLength(400);
        }
    }
}
