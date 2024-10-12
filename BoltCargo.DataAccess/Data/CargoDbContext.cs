﻿using BoltCargo.Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.DataAccess.Data
{
    public class CargoDbContext : IdentityDbContext<CustomIdentityUser, CustomIdentityRole, string>
    {
        public CargoDbContext(DbContextOptions<CargoDbContext> options) : base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }

        //public DbSet<Driver> Drivers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BoltCargoDB;Integrated Security=True;", b => b.MigrationsAssembly("BoltCargo.WebUI"));
        }
    }
}
