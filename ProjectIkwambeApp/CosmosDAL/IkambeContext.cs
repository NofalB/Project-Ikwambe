using Microsoft.EntityFrameworkCore;
using ProjectIkwambe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIkwambe.CosmosDAL
{
    public class IkambeContext : DbContext // DB Context represents the database
    {
        public DbSet<Donation> Donations { get; set; }

        public IkambeContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("IkambeContainer"); // sets the default container

            modelBuilder.Entity<Donation>()
                .ToContainer(nameof(Donations)); // sets the container

            modelBuilder.Entity<Donation>() // EF Core adds a discriminator value to identify the entity type that a given item represent
                .HasNoDiscriminator(); // HasNoDiscriminator() removes the discriminator since no other entity type will be stored in this container

            modelBuilder.Entity<Donation>()
                .HasPartitionKey(d => d.PartitionKey);  // sets partion key

            modelBuilder.Entity<Donation>()
                .UseETagConcurrency();

        }
    }
}
