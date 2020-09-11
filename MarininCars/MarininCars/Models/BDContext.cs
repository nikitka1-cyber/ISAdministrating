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
        public DbSet<Marks> Marks { get; set; }
        public DbSet<Models> Models { get; set; }
        public DbSet<Modifications> Modifications { get; set; }
        public DbSet<Orders> Orders { get; set; }
    }
}