using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MultiDataBaseProvider.Domain.Entities.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }

    internal class EntityBaseTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
