﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.Models
{
    class RepricerContext : DbContext
    {
        public DbSet<ListedItem> ListedItems { get; set; }
        public DbSet<FBAInventoryItem> FBAInventoryItems { get; set; }

    }
}
