using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MarininCars.Models
{
    public class BDContext: DbContext
    {
        public BDContext() :
            base("BDConnection")
        { }
        public DbSet<BdMarks> BdMarks { get; set; }
        public DbSet<BdModels> BdModels { get; set; }
        public DbSet<BdModifications> BdModifications { get; set; }
        public DbSet<BdOrders> BdOrders { get; set; }
    }
}