using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace Isaac_SensorSettingService.Models
{
    [Table("SensorSettings")]
    public class SettingsModel
    {
        [Key]
        public int Id  { get; set; }
        public int RefreshRate { get; set; }
        public bool KeepData { get; set; }
        public int ExpirationTime { get; set; }

    }
}
