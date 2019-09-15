using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entity.Base
{
    public abstract class BaseAuthEntity : BaseEntity
    {
        [Column("LOGIN_FAILED_NR", Order = 90)]
        public int? LoginFailedNumber { get; set; }

        [StringLength(2048)]
        [Column("TOKEN", Order = 91)]
        public string Token { get; set; }

        [StringLength(2048)]
        [Column("SUBCRISE_TOKEN", Order = 92)]
        public string SubcriseToken { get; set; }

        [Column("TOKEN_EXPIRED_DT", Order = 93)]
        public DateTime? TokenExpiredDate { get; set; }

        [Column("LOGIN_TM", Order = 94)]
        public DateTime? LoginTime { get; set; }

        [Required]
        [Column("PASSWORD", Order = 6)]
        [StringLength(1024)]
        public string Password { get; set; }

        [Column("PASSWORD_LAST_UDT", Order = 7)]
        public DateTime? PasswordLastUdt { get; set; } = DateTime.Now;
    }
}
