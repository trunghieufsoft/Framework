using System;
using System.Text;
using Asset.Common.Timing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entity.Base
{
    public abstract class BaseEntity : IHasCreationTime, IHasModificationTime, ISoftDelete
    {
        [Key]
        [Column("ID", Order = 1)]
        public Guid Id { get; set; }

        [Column("DELETED", Order = 95)]
        public bool IsDeleted { get; set; } = false;

        [Column("CREATED_USER", Order = 96)]
        [StringLength(2048)]
        public string CreatedBy { get; set; }

        [Column("CREATED_TM", Order = 97)]
        public DateTime CreationTime { get; set; } = Clock.Now;
        
        [Column("LAST_MDF_USER", Order = 98)]
        [StringLength(2048)]
        public string LastModifiedBy { get; set; }

        [Column("LAST_MDF_TM", Order = 99)]
        public DateTime? LastModifiedTime { get; set; }
    }
}
