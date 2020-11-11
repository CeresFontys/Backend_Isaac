using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_SensorSettingService.Models
{
    [Table("SensorSettings")]
    public class SettingsModel
    {
        public int Id  { get; set; }
        public int Floor_id { get; set; }
        public int RefreshRate { get; set; }
        public bool KeepData { get; set; }
        public int ExpirationTime { get; set; }

    }
}
