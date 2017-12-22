using DbModel.DomainClasses.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DbModel.DomainClasses.Configuration
{
    public class StudentConfig : EntityTypeConfiguration<Student>
    {
        public StudentConfig()
        {
            Property(x => x.SNO).HasMaxLength(50).IsRequired()
                          .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_SNO") { IsUnique = true }));

            Property(x => x.FirstName).HasMaxLength(50).IsOptional();
            Property(x => x.LastName).HasMaxLength(50).IsOptional();
            Property(x => x.AvatarPath).HasMaxLength(200);
        }
    }
}
