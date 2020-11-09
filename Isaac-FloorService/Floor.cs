using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Isaac_FloorService
{
    public class Floor
    {
        [Key]
        public string Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public string Image { get; set; }
    }
}
