using System;

namespace DataAccess.Entity.Base
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; set; }
    }
}