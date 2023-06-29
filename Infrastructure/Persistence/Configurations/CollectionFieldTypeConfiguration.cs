using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations {

    public class CollectionFieldTypeConfiguration : IEntityTypeConfiguration<CollectionFieldType> {


        public void Configure(EntityTypeBuilder<CollectionFieldType> builder) {
            builder.Property(e => e.Name).HasMaxLength(255);

            builder
                .HasMany(e => e.Fields)
                .WithOne(e => e.FieldType)
                .HasForeignKey(e => e.CollectionFieldTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

        }
    }
}
