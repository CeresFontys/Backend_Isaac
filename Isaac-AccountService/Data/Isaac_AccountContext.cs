using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Isaac_AccountService;

namespace Isaac_AccountService.Data
{
    public class Isaac_AccountContext : DbContext
    {
        public Isaac_AccountContext (DbContextOptions<Isaac_AccountContext> options)
            : base(options)
        {
        }

        public DbSet<Isaac_AccountService.User> User { get; set; }

        public DbSet<Isaac_AccountService.Whitelist> Whitelist { get; set; }
    }
}
