using Masasamjant.Claiming.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Masasamjant.Claiming.SqlServer
{
    /// <summary>
    /// Represents factory to create instance of <see cref="IEntityClaimRepository"/> implementation 
    /// that use Entity Framework with SQL server database.
    /// </summary>
    public sealed class EntityClaimRepositoryFactory : IEntityClaimRepositoryFactory
    {
        /// <summary>
        /// Creates new instance of <see cref="IEntityClaimRepository"/> implementation.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>A new instance of <see cref="IEntityClaimRepository"/> implementation.</returns>
        public IEntityClaimRepository CreateRepository(IConfiguration configuration)
        {
            try
            {
                var claimingSection = ClaimingConfigurationHelper.GetClaimingConfigurationSection(configuration);
                var connectionString = claimingSection["ConnectionString"];
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new ArgumentException("The configuration contains invalid connection string.", nameof(configuration));
                var commandTimeoutString = claimingSection["CommandTimeout"];
                int? commandTimeout = int.TryParse(commandTimeoutString, out var result) && result > 0 ? result : null;
                var schemaSection = claimingSection.GetChildren().SingleOrDefault(x => x.Key == "Schema");
                var schema = schemaSection != null ? GetSchema(schemaSection, EntityClaimSchema.Default) : EntityClaimSchema.Default;
                return new EntityDbContext(schema, connectionString, commandTimeout);
            }
            catch (InvalidOperationException exception)
            {
                throw new ArgumentException("The configuration is missing some key elements or has invalid configuration.", nameof(configuration), exception);
            }
        }

        private static EntityClaimSchema GetSchema(IConfiguration section, EntityClaimSchema defaultSchema)
        {
            var schemaName = section[nameof(EntityClaimSchema.SchemaName)] ?? defaultSchema.SchemaName;
            var tableName = section[nameof(EntityClaimSchema.TableName)] ?? defaultSchema.TableName;
            var ownerIdentifierMaxLength = ParseMaxLength(section[nameof(EntityClaimSchema.OwnerIdentifierMaxLength)], defaultSchema.OwnerIdentifierMaxLength);
            var typeNameMaxLength = ParseMaxLength(section[nameof(EntityClaimSchema.TypeNameMaxLength)], defaultSchema.TypeNameMaxLength);
            var instanceIdentifierMaxLength = ParseMaxLength(section[nameof(EntityClaimSchema.InstanceIdentifierMaxLength)], defaultSchema.InstanceIdentifierMaxLength);
            var applicationMaxLength = ParseMaxLength(section[nameof(EntityClaimSchema.ApplicationMaxLength)], defaultSchema.ApplicationMaxLength);
            var uniqueClaimSHA1MaxLength = ParseMaxLength(section[nameof(EntityClaimSchema.UniqueClaimSHA1MaxLength)], defaultSchema.UniqueClaimSHA1MaxLength);
            var uniqueClaimSHA1Index = section[nameof(EntityClaimSchema.UniqueClaimSHA1Index)] ?? defaultSchema.UniqueClaimSHA1Index;
            return new EntityClaimSchema(schemaName, tableName, ownerIdentifierMaxLength, typeNameMaxLength, instanceIdentifierMaxLength, applicationMaxLength, uniqueClaimSHA1MaxLength, uniqueClaimSHA1Index);
        }

        private static int ParseMaxLength(string? value, int defaultValue)
        {
            return int.TryParse(value, out var result) && result > 0 ? result : defaultValue;
        }
    }
}
