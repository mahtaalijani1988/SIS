using DbModel.DomainClasses.Entities;
using System.Data.Entity.ModelConfiguration;

namespace DbModel.DomainClasses.Configuration
{
   public class SiteOptionConfig:EntityTypeConfiguration<SiteOption>
   {
       public SiteOptionConfig()
       {
           Property(a => a.Name).HasMaxLength(50);
           Property(a => a.Value).IsMaxLength();
       }
    }
}
