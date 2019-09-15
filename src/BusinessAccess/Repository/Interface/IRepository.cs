using DataAccess.Entity.Base;
using System;

namespace BusinessAccess.Repository
{
    public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : BaseEntity
    {
    }
}