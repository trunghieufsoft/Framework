using System;

namespace DataAccess.Entity.Base
{
    public interface IHasModificationTime
    {
        DateTime? LastModifiedTime { get; set; }
    }
}