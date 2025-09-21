using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masasamjant.Claiming.SqlServer
{
    /// <summary>
    /// Represents configuration of <see cref="EntityClaim"/> to database table.
    /// </summary>
    internal sealed class EntityClaimConfiguration : IEntityTypeConfiguration<EntityClaim>
    {
        private readonly EntityClaimSchema schema;

        /// <summary>
        /// Initializes new instance of the <see cref="EntityClaimConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The <see cref="EntityClaimSchema"/>.</param>
        public EntityClaimConfiguration(EntityClaimSchema schema)
        {
            this.schema = schema;
        }

        /// <summary>
        /// Configures <see cref="EntityClaim"/> to database table.
        /// </summary>
        /// <param name="builder">The entity type builder.</param>
        public void Configure(EntityTypeBuilder<EntityClaim> builder)
        {
            builder.ToTable(schema.TableName, schema.SchemaName);
            builder.Property(x => x.ClaimIdentifier);
            builder.Property(x => x.OwnerIdentifier).IsRequired().HasMaxLength(schema.OwnerIdentifierMaxLength);
            builder.Property(x => x.ExpiresAt);
            builder.Property(x => x.AssemblyQualifiedTypeName).IsRequired().HasMaxLength(schema.TypeNameMaxLength);
            builder.Property(x => x.InstanceIdentifierSHA1).IsRequired().HasMaxLength(schema.InstanceIdentifierMaxLength);
            builder.Property(x => x.Application).IsRequired().HasMaxLength(schema.ApplicationMaxLength);
            builder.Property(x => x.UniqueClaimSHA1).IsRequired().HasMaxLength(schema.UniqueClaimSHA1MaxLength);
            builder.Ignore(x => x.ClaimKey);
            builder.HasKey(x => new
            {
                x.ClaimIdentifier
            });
            builder.HasIndex(x => new
            {
                x.UniqueClaimSHA1
            }, schema.UniqueClaimSHA1Index).IsUnique();
        }
    }
}
