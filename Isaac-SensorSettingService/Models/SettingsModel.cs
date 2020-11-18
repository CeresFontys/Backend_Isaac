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
        public string Id  { get; set; }
        public string RefreshRate { get; set; }
        public bool KeepData { get; set; }
        public string ExpirationTime { get; set; }

    }
}
