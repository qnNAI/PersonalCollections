using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {


    public class TagConfiguration : IEntityTypeConfiguration<Tag> {


        public void Configure(EntityTypeBuilder<Tag> builder) {
            builder.Property(e => e.Name).HasMaxLength(255);
            builder.HasIndex(e => e.Name).IsUnique();

            builder
                .HasMany(e => e.Items)
                .WithMany(e => e.Tags)
                .UsingEntity(
                    l => l.HasOne(typeof(Item)).WithMany().OnDelete(DeleteBehavior.Cascade),
                    r => r.HasOne(typeof(Tag)).WithMany().OnDelete(DeleteBehavior.Restrict));
        }
    }
}
