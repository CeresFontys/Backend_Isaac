using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Isaac_AnomalyService.Models;

namespace Isaac_AnomalyService.Data
{
    public class Isaac_AnomalyServiceContext : DbContext
    {
        public Isaac_AnomalyServiceContext (DbContextOptions<Isaac_AnomalyServiceContext> options)
            : base(options)
        {
        }

    }
}
