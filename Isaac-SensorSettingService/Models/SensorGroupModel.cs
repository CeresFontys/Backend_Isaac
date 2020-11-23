using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_SensorSettingService.Models
{
    public class SensorGroupModel
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Floor { get; set; }
        public List<SensorModel> Sensors { get; set; }
        public int uiIndex { get; set; }

    }
}
