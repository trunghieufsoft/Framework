using System;
using BusinessAccess.UnitOfWork;
using DataAccess.DbContext;
using DataAccess.Entity.Base;
using Microsoft.EntityFrameworkCore.Design;

namespace BusinessAccess.Repository
{
    public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity> where TEntity : BaseEntity
    {
        public Repository(IDesignTimeDbContextFactory<DataDbContext> dbContextFactory, IUnitOfWork unitOfWork)
            : base(dbContextFactory, unitOfWork)
        {
        }
    }
}