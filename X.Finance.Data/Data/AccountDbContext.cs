using X.Finance.Component.Entities;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace X.Finance.Data.Data
{
    public partial class AccountDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options) { }

        public virtual Microsoft.EntityFrameworkCore.DbSet<AccountDocument> AccountDocuments { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<AccountLine> AccountLines { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<AccountOutstanding> AccountOutstandings { get; set; }
        public virtual Microsoft.EntityFrameworkCore.DbSet<AccountAdvance> AccountAdvances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Here you map the entity to the physical table name if different
            modelBuilder.Entity<AccountDocument>().ToTable("tt_AccountDocument");
            modelBuilder.Entity<AccountLine>().ToTable("tt_AccountLine");
            modelBuilder.Entity<AccountOutstanding>().ToTable("tt_AccountOutstanding");
            modelBuilder.Entity<AccountAdvance>().ToTable("tt_AccountAdvance");

            modelBuilder.Entity<AccountDocument>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DocSeries).HasMaxLength(100);
                entity.Property(e => e.DocName).HasMaxLength(200);
                entity.Property(e => e.VersionNo).IsRowVersion();
            });

            modelBuilder.Entity<AccountLine>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.AccountDocument)
                    .WithMany(p => p.AccountLines)
                    .HasForeignKey(d => d.DocId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Map relationships if needed
            modelBuilder.Entity<AccountOutstanding>()
                .HasOne(o => o.AccountDocument)
                .WithMany()
                .HasForeignKey(o => o.DocId);

            // Map relationships if needed
            modelBuilder.Entity<AccountAdvance>()
                .HasOne(o => o.AccountDocument)
                .WithMany()
                .HasForeignKey(o => o.DocId);

            //OnModelCreatingPartial(modelBuilder);
        }
    }
}
