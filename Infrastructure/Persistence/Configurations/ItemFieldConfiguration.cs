using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {

    public class ItemFieldConfiguration : IEntityTypeConfiguration<ItemField> {


        public void Configure(EntityTypeBuilder<ItemField> builder) {

            builder
                .HasKey(e => new { e.CollectionFieldId, e.ItemId });

            builder.Property(e => e.Value).HasMaxLength(450);

            builder
                .HasOne(e => e.CollectionField)
                .WithMany(e => e.ItemFields)
                .HasForeignKey(e => e.CollectionFieldId)
                .IsRequired();

            builder
                .HasOne(e => e.Item)
                .WithMany(e => e.Fields)
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
