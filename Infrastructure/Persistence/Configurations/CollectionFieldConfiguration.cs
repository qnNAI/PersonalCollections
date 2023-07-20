using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CollectionFieldConfiguration : IEntityTypeConfiguration<CollectionField>
    {
        public void Configure(EntityTypeBuilder<CollectionField> builder)
        {
            builder
                .HasMany(x => x.ItemFields)
                .WithOne(x => x.CollectionField)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(x => x.CollectionFieldId);
        }
    }
}
