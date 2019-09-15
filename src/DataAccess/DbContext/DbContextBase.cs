using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using DataAccess.Entity.Base;
using System.Threading.Tasks;
using System.Threading;

namespace DataAccess.DbContext
{
    public class DbContextBase : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContextBase(DbContextOptions options) : base(options) { }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                ApplyDatabaseConcepts();
                return base.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DbUpdateException("Exception When Save Change DB {e}", e);
            }
        }

        private void ApplyDatabaseConcepts()
        {
            System.Collections.Generic.List<EntityEntry> entries = ChangeTracker.Entries().ToList();
            foreach (EntityEntry entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetCreationAuditProperties(entry.Entity);
                        break;

                    case EntityState.Modified:
                        SetModificationAuditProperties(entry.Entity);
                        break;

                    case EntityState.Deleted:
                        if (IsSoftDelete(entry.Entity))
                        {
                            CancelDeletionForSoftDelete(entry);
                            SetDeletionAuditProperties(entry.Entity);
                        }

                        break;

                    case EntityState.Detached:
                        break;

                    case EntityState.Unchanged:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool IsSoftDelete(object entityAsObj)
        {
            return entityAsObj is ISoftDelete;
        }

        private void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            entry.State = EntityState.Unchanged;
        }

        private void SetDeletionAuditProperties(object entityAsObj)
        {
            if (!(entityAsObj is ISoftDelete entityWithSoftDelete))
            {
                return;
            }

            entityWithSoftDelete.IsDeleted = true;
        }

        private void SetModificationAuditProperties(object entityAsObj)
        {
            if (!(entityAsObj is IHasModificationTime entityWithModificationTime))
            {
                return;
            }

            entityWithModificationTime.LastModifiedTime = DateTime.Now;
        }

        private void SetCreationAuditProperties(object entityAsObj)
        {
            if (!(entityAsObj is IHasCreationTime entityWithCreationTime))
            {
                return;
            }

            entityWithCreationTime.CreationTime = DateTime.Now;
        }
    }
}
