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
                .UsingEntity<ItemTag>(
                    r => r.HasOne(x => x.Item).WithMany(x => x.ItemTags).OnDelete(DeleteBehavior.Cascade),
                    l => l.HasOne(x => x.Tag).WithMany(x => x.ItemTags).OnDelete(DeleteBehavior.Restrict),
                    j => 
                    {
                        j.HasKey(e => new { e.ItemId, e.TagId });
                        j.ToTable("ItemTag");
                    });
        }
    }
}
