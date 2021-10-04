using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

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
                .HasKey(w => w.ProjectId); 

            modelBuilder.Entity<WaterPumpProject>()
                .UseETagConcurrency();


            modelBuilder.Entity<Coordinates>
                (c =>
                {
                    c.HasKey(c => c.CoordinateId);
                    c.ToContainer("Coordinates");
                    c.Property(v => v.LocationName);
                });
                
           /* modelBuilder.Entity<Coordinates>()
                .HasKey(c => c.CoordinateId);*/

            /*modelBuilder.Entity<Coordinates>()
                .Navigation(c => c.LocationName)
                .UsePropertyAccessMode(PropertyAccessMode.Property);*/

            /*modelBuilder.Entity<Coordinates>()
                .ToContainer(nameof(Coordinates));

            modelBuilder.Entity<Coordinates>()
                .HasNoDiscriminator();
            //need partition key
            modelBuilder.Entity<Coordinates>()
                .HasKey(w => w.LocationName);

            modelBuilder.Entity<Coordinates>()
                .UseETagConcurrency();*/

            //stuff
            /*modelBuilder.Entity<Coordinates>()
                .HasKey(c => c.CoordinateId);*/
            
            //modelBuilder.Ignore<Coordinates>();

            /* modelBuilder.Entity<Coordinates>()
                 .HasKey(c => c.ProjectId);*/

            /*modelBuilder.Entity<WaterPumpProject>()
                .Property(s => s.Coordinates).IsRequired();*/



            /*modelBuilder.Entity<Coordinates>(
                c =>
                {
                    c.HasNoKey();
                    c.ToContainer("WaterPumpProject");
                    c.Property(a => a.LocationName);
                });*/

           /* modelBuilder.Entity<WaterPumpProject>()
                .HasKey(w => w.Coordinates);*/
            /* modelBuilder.Entity<WaterPumpProject>()
                 .HasOne(w => w.Coordination);*/

            /*modelBuilder.Entity<GeoCoordinate>(entity =>
                entity
                    .HasNoKey()
                    .ToContainer("WaterPumpProject")
                    .
                );*/

            /* modelBuilder.Entity<GeoCoordinate>()
                 .HasKey(gc => gc.Longitude);*/

            /*modelBuilder.Entity<GeoCoordinate>()
                .HasNoKey();*/

            //modelBuilder.Ignore<GeoCoordinate>();

            /*modelBuilder.Entity<WaterPumpProject>()
                .HasOne(w => w.Coordination);

            modelBuilder.Entity<GeoCoordinate>()
                .HasNoKey();*/

            //modelBuilder.Ignore<GeoCoordinate>();

        }
    }
}
