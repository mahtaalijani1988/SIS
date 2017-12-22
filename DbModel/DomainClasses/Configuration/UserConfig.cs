using DbModel.DomainClasses.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace DbModel.DomainClasses.Configuration
{
    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            Property(x => x.UserName).HasMaxLength(50).IsRequired()
                 .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_UserName"){IsUnique = true}));
            Property(x => x.Password).HasMaxLength(200).IsRequired();
           
            Property(x => x.Email).HasMaxLength(100).IsOptional();
            Property(x => x.IP).HasMaxLength(20).IsOptional();
            Property(x => x.RowVersion).IsRowVersion();

            HasOptional(x => x.StudentData).WithRequired(x => x.User).WillCascadeOnDelete(true);
            HasOptional(x => x.ProfessorData).WithRequired(x => x.User).WillCascadeOnDelete(true);

        }
    }
}
