using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FIT5032_IbrahimFinalProject.Models;

namespace FIT5032_IbrahimFinalProject.Data
{
    public class ClinicContext : DbContext
    {
        public ClinicContext (DbContextOptions<ClinicContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Booking>().ToTable("Booking");
        }

        public DbSet<FIT5032_IbrahimFinalProject.Models.Documents>? Documents { get; set; }

        public DbSet<FIT5032_IbrahimFinalProject.Models.Email>? Email { get; set; }
    }
}
