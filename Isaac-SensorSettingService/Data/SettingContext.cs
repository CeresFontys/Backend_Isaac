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
    public class SettingContext : DbContext
    {
        public SettingContext(DbContextOptions<SettingContext> options):base(options)
        {
        }
        public DbSet<SettingsModel> Settings { get; set; }

    }
}
