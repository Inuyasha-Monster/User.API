using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;

namespace Project.Infrastructure.EntityConfigurations
{
    public class ProjectVisableRuleConfig : IEntityTypeConfiguration<ProjectVisableRule>
    {
        public void Configure(EntityTypeBuilder<ProjectVisableRule> builder)
        {
            builder.ToTable("ProjectVisableRules").HasKey(x => x.Id);
        }
    }
}