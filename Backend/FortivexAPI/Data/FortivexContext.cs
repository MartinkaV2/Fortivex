using FortivexAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FortivexAPI.Data
{
    public class FortivexContext : DbContext
    {
        public FortivexContext(DbContextOptions<FortivexContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<PlayerProgress> PlayerProgress { get; set; }
        public DbSet<PlayerStat> PlayerStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- TÁBLA KONFIGURÁCIÓK ---
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.Username)
                .IsUnique();

            modelBuilder.Entity<Admin>()
                .HasOne(a => a.Account)
                .WithOne(b => b.Admin)
                .HasForeignKey<Admin>(a => a.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerProgress>()
                .HasOne(p => p.Account)
                .WithOne(a => a.PlayerProgress)
                .HasForeignKey<PlayerProgress>(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerStat>()
                .HasOne(p => p.Account)
                .WithOne(a => a.PlayerStat)
                .HasForeignKey<PlayerStat>(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- SEED ADATOK ---
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    Username = "testplayer",
                    PasswordHash = "test123", // csak példa, valójában hashelt jelszót használj!
                    Email = "test@fortivex.com",
                    CreatedAt = DateTime.Now.AddDays(-10),
                    LastLogin = DateTime.Now
                },
                new Account
                {
                    Id = 2,
                    Username = "adminuser",
                    PasswordHash = "admin123",
                    Email = "admin@fortivex.com",
                    CreatedAt = DateTime.Now.AddDays(-20),
                    LastLogin = DateTime.Now
                },
                new Account
                {
                    Id = 3,
                    Username = "player2",
                    PasswordHash = "pass123",
                    Email = "player2@fortivex.com",
                    CreatedAt = DateTime.Now.AddDays(-5),
                    LastLogin = DateTime.Now
                }
            );

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    AccountId = 2, // az adminuser
                    Role = "Admin",
                    AssignedAt = DateTime.Now.AddDays(-15)
                }
            );

            modelBuilder.Entity<PlayerProgress>().HasData(
                new PlayerProgress
                {
                    Id = 1,
                    AccountId = 1,
                    BestTime = 250
                },
                new PlayerProgress
                {
                    Id = 2,
                    AccountId = 3,
                    BestTime = 180
                }
            );

            modelBuilder.Entity<PlayerStat>().HasData(
                new PlayerStat
                {
                    Id = 1,
                    AccountId = 1,
                    EnemiesKilled = 45,
                    TimePlayed = 3600
                },
                new PlayerStat
                {
                    Id = 2,
                    AccountId = 3,
                    EnemiesKilled = 62,
                    TimePlayed = 5400
                }
            );
        }
    }
}
