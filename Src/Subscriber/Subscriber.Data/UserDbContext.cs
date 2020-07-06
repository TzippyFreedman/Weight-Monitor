using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Subscriber.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Data
{

    public class UserDbContext : DbContext
    {
        private readonly IEncryptionProvider _provider;
        private readonly AesKeyInfo _encryptionInfo;

        public UserDbContext(DbContextOptions options) : base(options)
        {

            _encryptionInfo = AesProvider.GenerateKey(AesKeySize.AES128Bits);
            _provider = new AesProvider(_encryptionInfo.Key, _encryptionInfo.IV) ;

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        modelBuilder
                .UseEncryption(_provider);
            modelBuilder.Entity<User>()
                .ToTable("User");
            modelBuilder.Entity<User>()
                        .Property(user => user.Email)
                        .IsRequired();
            modelBuilder.Entity<User>()
                       .Property(user => user.Id)
                       .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<User>()
                        .HasIndex(user => user.Email)
                        .IsUnique();
            modelBuilder.Entity<User>()
                         .Property(u => u.FirstName)
                           .IsRequired();

            modelBuilder.Entity<User>()
                        .Property(u => u.Password)
                        .IsRequired();
            modelBuilder.Entity<UserFile>()
                      .Property(userfile => userfile.Id)
                      .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<UserFile>()
                         .Property(userfile => userfile.OpenDate)
                         .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<UserFile>()
                        .Property(userfile => userfile.Weight)
                        .HasDefaultValue(0);
            modelBuilder.Entity<UserFile>()
                       .Property(userfile => userfile.Height)
                       .HasDefaultValue(0);
            modelBuilder.Entity<UserFile>()
                        .Property(userfile => userfile.BMI)
                        .HasDefaultValue(0);

            modelBuilder.Entity<UserFile>()
                        .Property(userfile => userfile.UpdateDate)
                        .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<UserFile>()
                        .ToTable("UserFile");

        }

       public DbSet<User> Users { get; set; }
       public DbSet<UserFile> UserFiles { get; set; }

    }
}

