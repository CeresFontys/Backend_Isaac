using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Isaac_SensorSettingService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Isaac_SensorSettingService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
        }
        public DbSet<SettingsModel> Settings { get; set; }
        public DbSet<SensorModel> Sensors { get; set; }
        public DbSet<SensorGroupModel> Group { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorModel>()
                .HasKey(m => new { m.Floor, m.X, m.Y });
        }

    }
}
