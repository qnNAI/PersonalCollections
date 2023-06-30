using Application.Common.Contracts.Contexts;
using Domain.Entities.Identity;
using Domain.Entities.Items;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
	internal class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionField> CollectionFields { get; set; }
        public DbSet<CollectionTheme> CollectionThemes { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemField> ItemFields { get; set; }
        public DbSet<CollectionFieldType> CollectionFieldTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<CollectionField>()
                .Property(e => e.Name)
                .HasMaxLength(255);

            builder.Entity<Item>()
                .Property(e => e.Name)
                .HasMaxLength(255);

            builder.Entity<CollectionTheme>()
                .Property(e => e.Name)
                .HasMaxLength(255);

            builder.Entity<CollectionTheme>()
                .HasIndex(e => e.Name)
                .IsUnique();

            _SeedData(builder);
        }

        private static void _SeedData(ModelBuilder builder)
        {
            builder.Entity<CollectionTheme>()
                .HasData(
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Books"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Coins"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Comics"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Postcards"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Trading Cards"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Autographs"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Toy Cars"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Dolls"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Model Trains"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Jewelry"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Board Games"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Candles"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Hats"
                    },
                    new CollectionTheme
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Lighters"
                    }
                );
        }
    }
}
