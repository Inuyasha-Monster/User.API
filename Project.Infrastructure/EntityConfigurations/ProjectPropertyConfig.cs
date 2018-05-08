using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectPropertyConfig : IEntityTypeConfiguration<ProjectPropetry>
    {
        public void Configure(EntityTypeBuilder<ProjectPropetry> builder)
        {

        }
    }
}