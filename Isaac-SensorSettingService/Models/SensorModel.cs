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
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public SensorGroupModel Group { get; set; }
        [Required] public string Name { get; set; }

        [Required] 
        public string Floor { get; set; }
        [Required]
        public int X { get; set; }
        [Required]
        public int Y { get; set; }
        public int uiIndex { get; set; }
    }
}
