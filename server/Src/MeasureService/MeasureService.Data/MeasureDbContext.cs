using MeasureService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeasureService.Data
{
    public class MeasureDbContext : DbContext
    {
        public MeasureDbContext(DbContextOptions options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Measure>()
                          .ToTable("Measure");
            modelBuilder.Entity<Measure>()
                        .HasIndex(measure => measure.Id);

            modelBuilder.Entity<Measure>()
                        .Property(measure => measure.Status)
                        .HasConversion<string>();
            modelBuilder.Entity<Measure>()
                        .Property(measure => measure.UserFileId)
                        .IsRequired();
            modelBuilder.Entity<Measure>()
                         .Property(measure => measure.Status)
                        .IsRequired();
            modelBuilder.Entity<Measure>()
                       .Property(measure => measure.Weight)
                        .IsRequired();
            modelBuilder.Entity<Measure>()
                       .Property(measure => measure.Id)
                       .HasDefaultValueSql("NEWID()")
                        .ValueGeneratedOnAdd();
            modelBuilder.Entity<Measure>()
                        .Property(measure => measure.Date)
                        .HasDefaultValueSql("getdate()")
                        .ValueGeneratedOnAdd();

        }

        public DbSet<Measure> Measures { get; set; }
    }
}
