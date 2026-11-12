using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.Assessor.Domain.Entities;

namespace SFA.DAS.Assessor.Data.Configuration
{
    [ExcludeFromCodeCoverage]
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User), tableBuilder =>
                tableBuilder.UseSqlOutputClause(false))
                .HasKey(x => x.Id);
        }
    }
}