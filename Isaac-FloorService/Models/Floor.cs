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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        [Range(1, 50, ErrorMessage = "Value must fall between 1 and 50.")]
        public int Length { get; set; }
        [Range(1, 50, ErrorMessage = "Value must fall between 1 and 50.")]
        public int Width { get; set; }
        public string Image { get; set; }
    }
}
