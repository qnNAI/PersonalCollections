using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Domain.Entities.Items;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Contracts.Contexts
{
	public interface IApplicationDbContext
	{
        DbSet<ApplicationUser> Users { get; set; }

        DbSet<Collection> Collections { get; set; }
        DbSet<CollectionField> CollectionFields { get; set; }
        DbSet<CollectionTheme> CollectionThemes { get; set; }

        DbSet<Item> Items { get; set; }
        DbSet<ItemField> ItemFields { get; set; }
        DbSet<CollectionFieldType> CollectionFieldTypes { get; set; }
        DbSet<Tag> Tags { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
