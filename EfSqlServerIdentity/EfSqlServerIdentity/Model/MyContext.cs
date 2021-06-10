using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EfSqlServerIdentity.Model
{
    public partial class MyContext : DbContext
    {
        public MyContext()
        {
        }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<AuditDetail> AuditDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=TestDb;Integrated Security=True;Connection Timeout=10");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Audit>(entity =>
            {
                entity.Property(e => e.AuthorUserName).IsUnicode(false);

                entity.Property(e => e.ClrTypeName).IsUnicode(false);

                entity.Property(e => e.SqlTableName).IsUnicode(false);

                entity.Property(e => e.SqlTableSchema).IsUnicode(false);
            });

            modelBuilder.Entity<AuditDetail>(entity =>
            {
                entity.Property(e => e.ClrTypeName).IsUnicode(false);

                entity.Property(e => e.PropertyName).IsUnicode(false);

                entity.HasOne(d => d.Audit)
                    .WithMany(p => p.AuditDetails)
                    .HasForeignKey(d => d.AuditId)
                    .HasConstraintName("FK_dbo.AuditDetails_dbo.Audits");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
