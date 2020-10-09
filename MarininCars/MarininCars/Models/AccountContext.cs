using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MarininCars.Models
{
    public class AccountContext : DbContext
    {
        public AccountContext() :
        base("BDConnection")
        { }
        public DbSet<BdUsers> BdUsers { get; set; }
        public DbSet<BdRole> Roles { get; set; }
    }
}