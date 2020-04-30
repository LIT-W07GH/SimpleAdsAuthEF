using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SimpleAdsNew.Data
{
    public class SimpleAdsContext : DbContext
    {
        private readonly string _connectionString;

        public SimpleAdsContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<SimpleAd> Ads { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
