using Microsoft.EntityFrameworkCore;
using Subscriber.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Data
{
   public class UserContext:DbContext

    {
        public UserContext(DbContextOptions<UserContext> options)
          : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<UserFile>().ToTable("UserFile");
            modelBuilder.Entity<User>().ToTable("User");

            modelBuilder.Entity<UserFile>()
                               .Property(user => user.OpenDate)
                                   .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<UserFile>()
                              .Property(user => user.UpdateDate)
                                  .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<UserFile>()
                .Property(u => u.Weight)
                .HasDefaultValue(0);
            modelBuilder.Entity<UserFile>()
               .Property(u => u.BMI)
               .HasDefaultValue(0);
            modelBuilder.Entity<UserFile>()
                .Property(u=>u.Id)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<User>()

                  .HasIndex(u => u.Email)
                  .IsUnique();


            modelBuilder.Entity<User>()
           .Property(u => u.FirstName)
           .IsRequired();
           
            modelBuilder.Entity<User>()
           .Property(u => u.Password)
           .IsRequired();
        }
    }
}
