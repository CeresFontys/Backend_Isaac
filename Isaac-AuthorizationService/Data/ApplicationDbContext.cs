﻿using Isaac_AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isaac_AuthorizationService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        { 
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        { 
        }

        public DbSet<User> Users { get; set; }
    }
}
