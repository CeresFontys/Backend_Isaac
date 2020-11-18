using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_SensorSettingService.Models
{
    [Table("Sensors")]
    public class SensorModel
    {
        public int GroupId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Key, Column(Order = 0)]
        public string Floor { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        public int X { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        public int Y { get; set; }
    }
}
