using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MTS.ServiceDesk.Server.Models;

namespace MTS.ServiceDesk.Server.Data
{
    public partial class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDBContext()
        {

        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {

        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{

            //    optionsBuilder.UseSqlServer("");
            //}
        }

        //public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<SupportClient> SupportClient { get; set; }
        public virtual DbSet<UserStatus> UserStatus { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region REmoved ASPNetUsers
            //modelBuilder.Entity<AspNetUsers>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedEmail)
            //        .HasName("EmailIndex");

            //    entity.HasIndex(e => e.NormalizedUserName)
            //        .HasName("UserNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedUserName] IS NOT NULL)");

            //    entity.Property(e => e.Email).HasMaxLength(256);

            //    entity.Property(e => e.FirstName)
            //        .IsRequired()
            //        .HasMaxLength(150)
            //        .IsUnicode(false);

            //    entity.Property(e => e.LastName)
            //        .IsRequired()
            //        .HasMaxLength(150)
            //        .IsUnicode(false);

            //    entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

            //    entity.Property(e => e.UserName).HasMaxLength(256);

            //    entity.HasOne(d => d.Client)
            //        .WithMany(p => p.AspNetUsers)
            //        .HasForeignKey(d => d.ClientId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_AspNetUsers_Client");

            //    entity.HasOne(d => d.UserStatus)
            //        .WithMany(p => p.AspNetUsers)
            //        .HasForeignKey(d => d.UserStatusId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_AspNetUsers_UserStatus");
            //}); 
            #endregion

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SupportClient>(entity =>
            {
                entity.Property(e => e.DomainName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.SupportClient)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupportClient_Status");
            });

            modelBuilder.Entity<UserStatus>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            base.OnModelCreating(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
