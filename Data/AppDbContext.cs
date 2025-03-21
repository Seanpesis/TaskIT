using Microsoft.EntityFrameworkCore;
using TaskItClient.Models;
using System.IO;
using System;

namespace TaskItClient.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; } = default!;

        private static string DbFileName = "local.db";

        private static string DbPath =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DbFileName);
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var dbDir = Path.Combine(folder, "TaskItClient");
                Directory.CreateDirectory(dbDir);
                var dbPath = Path.Combine(dbDir, "local.db");

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

    }
}
