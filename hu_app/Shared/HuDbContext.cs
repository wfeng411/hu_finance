using hu_app.Models;
using hu_app.Models.Entities.Finance;
using hu_app.Models.Lookups.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace hu_app.Shared
{
    public class HuDbContext : DbContext
    {
        public HuDbContext() : base() { }

        public HuDbContext(DbContextOptions<HuDbContext> options) : base(options) { }

        public DbSet<Logs> Logs { get; set; }

        public DbSet<HuAppUser> HuAppUser { get; set; }

        public DbSet<FinanceTransaction> FinanceTransaction { get; set; }
        public DbSet<FinanceTransactionType> FinanceTransactionType { get; set; }
        public DbSet<FinanceItem> FinanceItem { get; set; }
        public DbSet<FinanceItemIndex> FinanceItemIndex { get; set; }
        public DbSet<FinanceTransaction> FinanceMerchant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinanceTransaction>()
                .HasOne(x => x.Item)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.ItemId);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            FilterSoftDeletedEntries(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ApplySoftDelete();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        public override int SaveChanges()
        {
            ApplySoftDelete();
            var result = base.SaveChanges();
            return result;
        }

        private void ApplySoftDelete()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker
                .Entries()
                .Where(x => x.State == EntityState.Deleted && x.Entity is BaseModel);

            foreach (EntityEntry entry in entries)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    entity.IsDeleted = true;
                    entry.State = EntityState.Modified;
                }
                else
                {
                    throw new Exception("Lookup data cannot be deleted.");
                }
            }
        }
        private void FilterSoftDeletedEntries(ModelBuilder builder)
        {
            IEnumerable<Type> entityTypes = builder
                .Model
                .GetEntityTypes()
                .Where(t => t.BaseType == null && typeof(BaseEntity).IsAssignableFrom(t.ClrType))
                .Select(t => t.ClrType);

            foreach (Type entityType in entityTypes)
            {
                _setEntityQueryFilterMethod
                    .MakeGenericMethod(entityType)
                    .Invoke(null, new object[] { builder });
            }
        }

        private readonly MethodInfo _setEntityQueryFilterMethod = typeof(HuDbContext)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetEntityQueryFilter));

        private static void SetEntityQueryFilter<T>(ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
