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

        protected override void OnModelCreating(ModelBuilder builder) {
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
        }
    }
}
