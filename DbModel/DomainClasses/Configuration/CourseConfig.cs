using DbModel.DomainClasses.Entities;
using System.Data.Entity.ModelConfiguration;

namespace DbModel.DomainClasses.Configuration
{
    public class CourseConfig : EntityTypeConfiguration<Course>
    {
        public CourseConfig()
        {
            Property(a => a.Name).HasMaxLength(100).IsRequired();

            HasMany(a => a.Children).WithOptional(a => a.Parent).HasForeignKey(a => a.Parent_id);

        }
    }
}
