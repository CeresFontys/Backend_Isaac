using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Isaac_FloorService.Models
{
    public class FloorRequest
    {
        public Floor Floor { get; set; }
        public IFormFile File { get; set; }
    }
}
