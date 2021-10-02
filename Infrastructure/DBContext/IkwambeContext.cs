using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DBContext
{
    public class IkwambeContext : DbContext // DB Context represents the database
    {
        public DbSet<Donation> Donations { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Story> Stories { get; set; }
        
        public DbSet<WaterPumpProject> WaterpumpProject { get; set; }

        public IkwambeContext(DbContextOptions options) : base(options)
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

            // users
            modelBuilder.Entity<User>()
               .ToContainer(nameof(Users)); // sets the container

            modelBuilder.Entity<User>() // EF Core adds a discriminator value to identify the entity type that a given item represent
                .HasNoDiscriminator(); // HasNoDiscriminator() removes the discriminator since no other entity type will be stored in this container

            modelBuilder.Entity<User>()
                .UseETagConcurrency();

            // story
            modelBuilder.Entity<Story>()
               .ToContainer(nameof(Story));

            modelBuilder.Entity<Story>()
                .HasNoDiscriminator();
            //need partition key
            modelBuilder.Entity<Story>()
                .UseETagConcurrency();

            //waterpump
            modelBuilder.Entity<WaterPumpProject>()
               .ToContainer(nameof(WaterPumpProject));

            modelBuilder.Entity<WaterPumpProject>()
                .HasNoDiscriminator();
            //need partition key
            modelBuilder.Entity<WaterPumpProject>()
                .UseETagConcurrency();

        }
    }
}
