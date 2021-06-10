using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EfSqlServerIdentity.Model
{
    [Index(nameof(ChangeDateTime), Name = "IX_Audits_ChangeDateTime")]
    public partial class Audit
    {
        public Audit()
        {
            AuditDetails = new HashSet<AuditDetail>();
        }

        [Key]
        public long AuditId { get; set; }
        [Required]
        [StringLength(255)]
        public string AuthorUserName { get; set; }
        public DateTimeOffset ChangeDateTime { get; set; }
        public int ChangeType { get; set; }
        public long RecordId { get; set; }
        [Required]
        [StringLength(255)]
        public string ClrTypeName { get; set; }
        [Required]
        [StringLength(255)]
        public string SqlTableName { get; set; }
        [Required]
        [StringLength(255)]
        public string SqlTableSchema { get; set; }

        [InverseProperty(nameof(AuditDetail.Audit))]
        public virtual ICollection<AuditDetail> AuditDetails { get; set; }
    }
}
