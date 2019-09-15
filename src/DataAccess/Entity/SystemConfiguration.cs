using System;
using DataAccess.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entity.EnumType;

namespace DataAccess.Entity
{
    [Table("SYS_CONFIG")]
    public class SystemConfiguration : BaseEntity
    {
        #region Initial
        public SystemConfiguration()
        {
        }
        #endregion

        #region Primary key
        [Column("KEY")]
        [StringLength(2048)]
        public string KeyStr
        {
            get { return Key.ToString(); }
            set { Key = (SystemConfigEnum)Enum.Parse(typeof(SystemConfigEnum), value, true); }
        }

        [NotMapped]
        public SystemConfigEnum Key { get; set; }
        #endregion

        #region Properties
        [Column("VALUE")]
        [StringLength(2048)]
        public string Value { get; set; }

        [Column("VALUE_UNIT")]
        [StringLength(2048)]
        public string ValueUnit
        {
            get { return Unit.ToString(); }
            set { Unit = (Unit)Enum.Parse(typeof(Unit), value, true); }
        }

        [NotMapped]
        public Unit Unit { get; set; }
        #endregion
    }
}