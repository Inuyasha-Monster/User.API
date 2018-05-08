using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectPropertyConfig : IEntityTypeConfiguration<ProjectPropetry>
    {
        public void Configure(EntityTypeBuilder<ProjectPropetry> builder)
        {
            builder.ToTable("ProjectPropetries");

            builder.Property(x => x.Key).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Value).IsRequired().HasMaxLength(100);

            builder.HasKey(x => new { x.ProjectId, x.Key, x.Value });
        }
    }
}