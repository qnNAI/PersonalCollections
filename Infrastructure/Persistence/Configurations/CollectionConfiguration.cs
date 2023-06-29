using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {

    public class CollectionConfiguration : IEntityTypeConfiguration<Collection> {


        public void Configure(EntityTypeBuilder<Collection> builder) {
            builder.Property(e => e.Name).HasMaxLength(255);
            builder.Property(e => e.ImageUrl).HasMaxLength(450);
            
            builder
                .HasOne(e => e.Theme)
                .WithMany(e => e.Collections)
                .HasForeignKey(e => e.CollectionThemeId)
                .IsRequired();

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.Collections)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            builder
                .HasMany(e => e.Items)
                .WithOne(e => e.Collection)
                .HasForeignKey(e => e.CollectionId)
                .IsRequired();

            builder
                .HasMany(e => e.Fields)
                .WithOne(e => e.Collection)
                .HasForeignKey(e => e.CollectionId)
                .IsRequired();
        }
    }
}
