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
        
        public DbSet<WaterpumpProject> WaterpumpProject { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PurchaseUnit> PurchaseUnits { get; set; }


        public IkwambeContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("IkwambeContainer"); // sets the default container

            modelBuilder.Entity<Donation>()
                .ToContainer(nameof(Donations))
                                        
                 // EF Core adds a discriminator value to identify the entity type that a given item represent
                .HasNoDiscriminator() // HasNoDiscriminator() removes the discriminator since no other entity type will be stored in this container
                .HasPartitionKey(d => d.PartitionKey)
                .UseETagConcurrency();

            // users
            modelBuilder.Entity<User>()
               .ToContainer(nameof(Users))
               .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
               .HasNoDiscriminator()
               .HasPartitionKey(u => u.PartitionKey)
               .UseETagConcurrency();

            // story
            modelBuilder.Entity<Story>()
               .ToContainer(nameof(Story))
               .HasNoDiscriminator()
               .HasPartitionKey(s => s.PartitionKey)
               .UseETagConcurrency()
               .OwnsMany(s => s.StoryImages);


            //waterpump and coordinates
            modelBuilder.Entity<WaterpumpProject>()
                .ToContainer(nameof(Project))
                .HasKey(w => w.ProjectId);

            modelBuilder.Entity<WaterpumpProject>()
                .HasNoDiscriminator()
                .HasPartitionKey(d => d.PartitionKey)
                .UseETagConcurrency()
                .OwnsOne(o => o.Coordinates);

            //transaction and its classes
            modelBuilder.Entity<Transaction>()
                .ToContainer(nameof(Transactions))
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasPartitionKey(t => t.PartitionKey)
                .UseETagConcurrency()
                .OwnsMany(t => t.Links);

            modelBuilder.Entity<Transaction>()
                .OwnsMany(t => t.PurchaseUnits, p =>
                {
                    p.OwnsOne(a => a.Amount);
                    p.OwnsOne(b => b.Payee);
                    p.OwnsOne(b => b.Payments)
                    .OwnsMany(c => c.Captures);
                    p.OwnsOne(d => d.Shipping, s =>
                    {
                        s.OwnsOne(a => a.Address);
                        s.OwnsOne(b => b.Name);
                    });
                });

            modelBuilder.Entity<Transaction>()
                .OwnsOne(t => t.Payer, p =>
                 {
                     p.OwnsOne(a => a.Address);
                     p.OwnsOne(b => b.Name);
                 });

            //modelBuilder.Entity<Payments>()
            //    .OwnsMany(p => p.Captures);

            //modelBuilder.Entity<Name>()
            //    .Property(n => n.NameId).ValueGeneratedOnAdd();

            //modelBuilder.Entity<Payee>()
            //    .Property(p => p.PayeeId).ValueGeneratedOnAdd();
        }
    }
}
