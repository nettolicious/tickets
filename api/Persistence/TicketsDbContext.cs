using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Nettolicious.Tickets.Contracts.Persistence.Entities;

namespace Nettolicious.Tickets.Persistence
{
    public partial class TicketsDbContext : DbContext
    {
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }

        public TicketsDbContext(DbContextOptions<TicketsDbContext> options) : base(options)
          {
          }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.SysCreated).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.SysCreatedBy).HasDefaultValueSql("(suser_name())");

                entity.Property(e => e.SysLastModified).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.SysLastModifiedBy).HasDefaultValueSql("(suser_name())");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasIndex(e => e.TicketNumber)
                    .HasName("UQIX_Ticket_TicketNumber")
                    .IsUnique();

                entity.Property(e => e.SysCreated).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.SysCreatedBy).HasDefaultValueSql("(suser_name())");

                entity.Property(e => e.SysLastModified).HasDefaultValueSql("(sysdatetimeoffset())");

                entity.Property(e => e.SysLastModifiedBy).HasDefaultValueSql("(suser_name())");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ticket_Order");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
