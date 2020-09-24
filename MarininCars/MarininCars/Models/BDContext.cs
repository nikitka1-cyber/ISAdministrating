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
        public DbSet<BdMarks> Marks { get; set; }
        public DbSet<BdModels> Models { get; set; }
        public DbSet<BdModifications> Modifications { get; set; }
        public DbSet<BdOrders> Orders { get; set; }
    }
}