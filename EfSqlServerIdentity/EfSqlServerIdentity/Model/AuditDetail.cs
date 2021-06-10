using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EfSqlServerIdentity.Model
{
    [Index(nameof(AuditId), Name = "IX_AuditDetails_AuditId_Include")]
    public partial class AuditDetail
    {
        [Key]
        public long AuditDetailId { get; set; }
        [Required]
        [StringLength(255)]
        public string PropertyName { get; set; }
        [Required]
        [StringLength(255)]
        public string ClrTypeName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public long AuditId { get; set; }

        [ForeignKey(nameof(AuditId))]
        [InverseProperty("AuditDetails")]
        public virtual Audit Audit { get; set; }
    }
}
