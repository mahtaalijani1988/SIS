using DbModel.DomainClasses.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DbModel.DomainClasses.Configuration
{
    public class ProfessorConfig : EntityTypeConfiguration<Professor>
    {
        public ProfessorConfig()
        {
            Property(x => x.FirstName).HasMaxLength(50).IsOptional();
            Property(x => x.LastName).HasMaxLength(50).IsOptional();
            Property(x => x.AvatarPath).HasMaxLength(200);

            Property(x => x.PNO).HasMaxLength(50).IsRequired()
                          .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PNO") { IsUnique = true }));
          
        }
    }
}
