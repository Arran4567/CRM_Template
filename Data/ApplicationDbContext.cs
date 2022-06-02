using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bicks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bicks.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext() : base()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Link DB Table to Model
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Bookings>().ToTable("Bookings");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Sale>().ToTable("Sales");
            modelBuilder.Entity<InvoiceItem>().ToTable("InvoiceItems");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
    }
}
