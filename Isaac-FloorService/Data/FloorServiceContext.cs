using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Isaac_FloorService;

namespace Isaac_FloorService.Data
{
    public class FloorServiceContext : DbContext
    {
        public FloorServiceContext()
        {
            
        }
        public FloorServiceContext (DbContextOptions<FloorServiceContext> options)
            : base(options)
        {
        }

        public DbSet<Isaac_FloorService.Floor> Floor { get; set; }
    }
}
