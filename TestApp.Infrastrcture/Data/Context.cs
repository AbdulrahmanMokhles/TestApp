using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Domain.Models;

namespace TestApp.Infrastrcture.Data
{
    public class Context : DbContext
    {
        public DbSet<MacAddress> MacAddresses { get; set; }
        public Context(DbContextOptions<Context> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MacAddress>()
                .HasIndex(m => m.Mac)
                .IsUnique();


            base.OnModelCreating(modelBuilder);
        }
    }
}
