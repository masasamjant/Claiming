using Masasamjant.Claiming.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Masasamjant.Claiming.SqlServer
{
    /// <summary>
    /// Represents repository to access entity claims stored to database using Entity Framework and SQL server.
    /// </summary>
    internal sealed class EntityDbContext : DbContext, IEntityClaimRepository
    {
        private readonly string connectionString;
        private readonly int? commandTimeoutSeconds;
        private readonly EntityClaimSchema schema;

        /// <summary>
        /// Initializes new instance of the <see cref="EntityDbContext"/> class.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="connectionString"></param>
        /// <param name="commandTimeoutSeconds"></param>
        public EntityDbContext(EntityClaimSchema schema, string connectionString, int? commandTimeoutSeconds)
        {
            this.schema = schema;
            this.connectionString = connectionString;
            this.commandTimeoutSeconds = commandTimeoutSeconds;
        }

        /// <summary>
        /// Insert specified <see cref="EntityClaim"/> to database.
        /// </summary>
        /// <param name="claim">The <see cref="EntityClaim"/> to insert.</param>
        /// <returns>A <see cref="EntityClaim"/> inserted.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public async Task<EntityClaim> AddClaimAsync(EntityClaim claim)
        {
            CheckDisposed();
            var entry = await AddAsync(claim);
            await SaveChangesAsync();
            return entry.Entity;
        }

        /// <summary>
        /// Gets the <see cref="EntityClaim"/> specified by claim key.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> of the claim.</param>
        /// <returns>A <see cref="EntityClaim"/> or <c>null</c>, if not exists.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public async Task<EntityClaim?> GetClaimAsync(ClaimKey claimKey)
        {
            CheckDisposed();
            return await Set<EntityClaim>()
                .Where(claim => claim.InstanceIdentifier == claimKey.InstanceIdentifier
                    && claim.AssemblyQualifiedTypeName == claimKey.AssemblyQualifiedTypeName
                    && claim.Application == claimKey.Application)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the <see cref="EntityClaim"/> specified by claim identifier.
        /// </summary>
        /// <param name="claimIdentifier">The claim identifier.</param>
        /// <returns>A <see cref="EntityClaim"/> or <c>null</c>, if not exists.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public async Task<EntityClaim?> GetClaimAsync(Guid claimIdentifier)
        {
            CheckDisposed();
            return await Set<EntityClaim>()
                .Where(claim => claim.ClaimIdentifier == claimIdentifier)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the <see cref="EntityClaim"/> specified by other claim.
        /// </summary>
        /// <param name="other">The other claim.</param>
        /// <returns>A <see cref="EntityClaim"/> or <c>null</c>, if not exists.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public async Task<EntityClaim?> GetClaimAsync(Claim other)
        {
            CheckDisposed();
            return await Set<EntityClaim>()
                .Where(ec => ec.ClaimIdentifier == other.ClaimIdentifier
                    && ec.OwnerIdentifier == other.OwnerIdentifier
                    && ec.InstanceIdentifier == other.ClaimKey.InstanceIdentifier
                    && ec.AssemblyQualifiedTypeName == other.ClaimKey.AssemblyQualifiedTypeName
                    && ec.Application == other.ClaimKey.Application)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets all claims stored.
        /// </summary>
        /// <returns>A read-only collection of current claims.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public async Task<IReadOnlyCollection<EntityClaim>> GetClaimsAsync()
        {
            CheckDisposed();
            var claims = await Set<EntityClaim>().ToListAsync();
            return claims.AsReadOnly();
        }

        /// <summary>
        /// Check if claim exists that use specified claim key.
        /// </summary>
        /// <param name="claimKey">The <see cref="ClaimKey"/> of the claim.</param>
        /// <returns><c>true</c> if claim exist; <c>false</c> otherwise.</returns>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public async Task<bool> ExistsAsync(ClaimKey claimKey)
        {
            CheckDisposed();
            return await Set<EntityClaim>()
                .Where(claim => claim.InstanceIdentifier == claimKey.InstanceIdentifier
                    && claim.AssemblyQualifiedTypeName == claimKey.AssemblyQualifiedTypeName
                    && claim.Application == claimKey.Application)
                .AnyAsync();
        }

        /// <summary>
        /// Delete specified <see cref="EntityClaim"/> permanently from database.
        /// </summary>
        /// <param name="claim">The <see cref="EntityClaim"/> to delete.</param>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public async Task RemoveClaimAsync(EntityClaim claim)
        {
            CheckDisposed();
            var entry = Entry(claim);
            if (entry.State == EntityState.Detached)
                Attach(claim);
            Remove(claim);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Disposes current instance.
        /// </summary>
        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString, op => op.CommandTimeout(commandTimeoutSeconds));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityClaimConfiguration(schema));
            base.OnModelCreating(modelBuilder);
        }

        private void CheckDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        private bool IsDisposed { get; set; }
    }
}
