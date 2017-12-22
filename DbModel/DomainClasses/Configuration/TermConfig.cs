using DbModel.DomainClasses.Entities;
using System.Data.Entity.ModelConfiguration;

namespace DbModel.DomainClasses.Configuration
{
    public class TermConfig : EntityTypeConfiguration<Term>
    {
        public TermConfig()
        {
            Property(x => x.Name).HasMaxLength(200).IsRequired();
        }
    }
}
