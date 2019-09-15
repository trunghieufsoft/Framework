using System;
using DataAccess.Entity.Base;
using DataAccess.Entity.EnumType;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entity
{
    [Table("USER")]
    public class User : BaseAuthEntity
    {
        #region Initial
        public User()
        {
            Id = Guid.NewGuid();
            Code = string.Join("", Id.ToString().Split("-"));
        }
        #endregion

        #region Primary key
        [Required]
        [StringLength(2048)]
        [Column("USERNAME", Order = 5)]
        public string Username { get; set; }
        #endregion

        #region Foreign Key References

        #endregion

        #region Properties
        [Required]
        [StringLength(128)]
        [Column("CODE", Order = 2)]
        public string Code { get; set; }

        [Required]
        [StringLength(2048)]
        [Column("FULL_NAME", Order = 3)]
        public string FullName { get; set; }

        [Required]
        [StringLength(2048)]
        [Column("USER_TYP", Order = 4)]
        public string UserTypeStr
        {
            get { return UserType.ToString(); }
            set { UserType = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), value, true); }
        }

        [NotMapped]
        public UserTypeEnum UserType { get; set; }

        [Required]
        [StringLength(2048)]
        [Column("STATUS")]
        public string StatusStr
        {
            get { return Status.ToString(); }
            set { Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), value, true); }
        }

        [NotMapped]
        public StatusEnum Status { get; set; } = StatusEnum.Active;

        [Required]
        [StringLength(2048)]
        [Column("ADDRESS")]
        public string Address { get; set; }

        [StringLength(2048)]
        [Column("EMAIL")]
        public string Email { get; set; }

        [Required]
        [Column("PHONE")]
        [StringLength(128)]
        public string Phone { get; set; }
        #endregion

        #region Relationship

        #endregion
    }
}
