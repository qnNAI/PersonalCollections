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
using Microsoft.AspNetCore.Identity;
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

        public async Task<AddItemResponse> AddItemAsync(AddItemRequest request)
        {
            var item = request.Adapt<Item>();
            var isExists = await _context.Items.AnyAsync(x => x.CollectionId == item.CollectionId && x.Name == item.Name);
            if (isExists) {
                return new AddItemResponse {
                    Succeeded = false,
                    Errors = new[] {
                        "Item already exists!"
                    }
                };
            }
            

            item.Id = Guid.NewGuid().ToString();

            await _PrepareTagsAsync(item);

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return new AddItemResponse { Succeeded = true };
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

        public async Task<IEnumerable<ItemDto>> GetItemsAsync(GetItemsRequest request, CancellationToken cancellationToken) {
            var itemsQuery = _context.Items.Where(x => x.CollectionId == request.CollectionId);

            if (!string.IsNullOrEmpty(request.Filter)) {
                itemsQuery = itemsQuery.Where(x => EF.Functions.Contains(x.Name, request.Filter));
            }

            itemsQuery = request.Order switch {
                "asc" => itemsQuery.OrderBy(x => x.Name),
                "desc" => itemsQuery.OrderByDescending(x => x.Name),
                _ => itemsQuery.OrderBy(x => x.Name),
            };

            var items = await itemsQuery
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(x => x.Fields)
                    .ThenInclude(x => x.CollectionField)
                        .ThenInclude(x => x.FieldType)
                .ProjectToType<ItemDto>()
                .ToListAsync(cancellationToken);

            return items;
        }

        public Task<int> CountItemsAsync(string collectionId, CancellationToken cancellationToken = default) {
            return _context.Items.Where(x => x.CollectionId == collectionId).CountAsync(cancellationToken);
        }
    }
}
