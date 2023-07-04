using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Contracts.Contexts;
using Application.Common.Contracts.Services;
using Application.Models.Item;
using Domain.Entities.Items;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    internal class ItemService : IItemService
    {
        private readonly IApplicationDbContext _context;

        public ItemService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemFieldDto>> GetItemFieldsAsync(string collectionId)
        {
            var fields = await _context.CollectionFields
                .Where(x => x.CollectionId == collectionId)
                .Include(x => x.FieldType)
                .ProjectToType<CollectionFieldDto>()
                .Select(x => new ItemFieldDto
                {
                    CollectionField = x
                })
                .ToListAsync();

            return fields;
        } 

        public async Task AddItemAsync(AddItemRequest request)
        {
            var item = request.Adapt<Item>();
            item.Id = Guid.NewGuid().ToString();

            await _PrepareTagsAsync(item);

            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        private async Task _PrepareTagsAsync(Item item)
        {
            var tagNames = item.Tags.Select(x => x.Name);
            var existingTags = await _context.Tags
                .Where(x => tagNames.Contains(x.Name))
                .ToDictionaryAsync(x => x.Name);

            item.Tags = item.Tags.Select(x =>
            {
                if(existingTags.ContainsKey(x.Name))
                {
                    return existingTags[x.Name];
                }
                else
                {
                    x.Id = Guid.NewGuid().ToString();
                    return x;
                }
            })
            .ToList();
        }

        public async Task<IEnumerable<TagDto>> GetTagsByPrefixAsync(string prefix)
        {
            var tags = await _context.Tags
                .Where(x => x.Name.StartsWith(prefix))
                .Take(20)
                .ProjectToType<TagDto>()
                .ToListAsync();

            return tags;
        }
    }
}
