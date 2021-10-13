﻿using Domain;
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
        
        public DbSet<WaterpumpProject> WaterpumpProject { get; set; }

        public IkwambeContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("IkambeContainer"); // sets the default container

            modelBuilder.Entity<Donation>()
                .ToContainer(nameof(Donations))
                                        
                 // EF Core adds a discriminator value to identify the entity type that a given item represent
                .HasNoDiscriminator() // HasNoDiscriminator() removes the discriminator since no other entity type will be stored in this container
                .HasPartitionKey(d => d.PartitionKey)
                .UseETagConcurrency();

            // users
            modelBuilder.Entity<User>()
               .ToContainer(nameof(Users))
               .HasNoDiscriminator()
               .HasPartitionKey(u => u.PartitionKey)
               .UseETagConcurrency();

            // story
            modelBuilder.Entity<Story>()
               .ToContainer(nameof(Story))
               .HasNoDiscriminator()
               .HasPartitionKey(s => s.PartitionKey)
               .UseETagConcurrency();


            //waterpump and coordinates
            modelBuilder.Entity<WaterpumpProject>()
                .ToContainer(nameof(Project))
                .HasKey(w => w.ProjectId);

            modelBuilder.Entity<WaterpumpProject>()
                .HasNoDiscriminator()
                .HasPartitionKey(d => d.PartitionKey)
                .UseETagConcurrency()
                .OwnsOne(o => o.Coordinates);
        }
    }
}
