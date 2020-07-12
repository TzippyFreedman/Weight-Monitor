using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using TrackingService.Data.Entities;

namespace TrackingService.Data
{
    public class RecordDbContext : DbContext
    {

        public RecordDbContext(DbContextOptions<RecordDbContext> options)
         : base(options)
        {
            
        }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Record>()
                       .ToTable("Record");

            modelBuilder.Entity<Record>()
                      .HasIndex(user => user.Id)
                      .IsUnique();

            modelBuilder.Entity<Record>()
                      .Property(record => record.Id)
                      .HasDefaultValueSql("NEWID()")
                      .ValueGeneratedOnAdd();

            modelBuilder.Entity<Record>()
                        .Property(record => record.UserFileId)
                        .IsRequired();


            modelBuilder.Entity<Record>()
                        .Property(record => record.Weight)
                        .IsRequired();

            modelBuilder.Entity<Record>()
                        .Property(record => record.BMI)
                        .IsRequired();

            modelBuilder.Entity<Record>()
                        .Property(record => record.Date)
                        .HasDefaultValueSql("getdate()")
                        .ValueGeneratedOnAdd();


        }

        public DbSet<Record> Records { get; set; }
    }
}
