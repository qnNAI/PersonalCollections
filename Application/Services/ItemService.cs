using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Contracts.Contexts;
using Application.Common.Contracts.Services;
using Application.Models.Collection;
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

        public async Task UpdateItemAsync(ItemDto request)
        {
            var existing = await _context.Items.AsNoTracking()
                .Include(x => x.ItemTags)
                .Include(x => x.Tags)
                .FirstAsync(x => x.Id == request.Id);
            var item = request.Adapt<Item>();
            await _PrepareTagsAsync(item);

            var tagsToRemove = existing.ItemTags.Where(x => !item.ItemTags.Any(it => it.TagId == x.TagId)).ToList();

            _context.Tags.AddRange(item.Tags);
            _context.Items.Update(item);
            await _context.SaveChangesAsync();

            _context.Entry(item).State = EntityState.Detached;
            _context.ItemTags.RemoveRange(tagsToRemove);
            await _context.SaveChangesAsync();
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
            var itemsQuery = _context.Items.AsNoTracking().Where(x => x.CollectionId == request.CollectionId);

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

        public Task<int> CountItemsAsync(string collectionId, string filter, CancellationToken cancellationToken) {
            var itemsQuery = _context.Items.Where(x => x.CollectionId == collectionId);

            if(!string.IsNullOrEmpty(filter))
            {
                itemsQuery = itemsQuery.Where(x => EF.Functions.Contains(x.Name, filter));
            }

            return itemsQuery.CountAsync(cancellationToken);
        }

        public async Task<RemoveItemResponse> RemoveAsync(string itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if(item is null)
            {
                return new RemoveItemResponse { Succeeded = false };
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return new RemoveItemResponse { Succeeded = true };
        }

        public async Task<ItemDto?> GetByIdAsync(string itemId)
        {
            var item = await _context.Items.AsNoTracking()
                .Where(x => x.Id == itemId)
                .Include(x => x.Fields)
                    .ThenInclude(x => x.CollectionField)
                        .ThenInclude(x => x.FieldType)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync();

            return item?.Adapt<ItemDto>();
        }

        private async Task _PrepareTagsAsync(Item item)
        {
            var tagNames = item.Tags.Select(x => x.Name);
            var existingTags = await _context.Tags.AsNoTracking()
                .Where(x => tagNames.Contains(x.Name))
                .ToDictionaryAsync(x => x.Name);
            var newTags = new List<Tag>();

            foreach(var tag in item.Tags)
            {
                if(existingTags.ContainsKey(tag.Name))
                {
                    item.ItemTags.Add(new ItemTag
                    {
                        ItemId = item.Id,
                        TagId = existingTags[tag.Name].Id
                    });
                }
                else
                {
                    tag.Id = Guid.NewGuid().ToString();
                    newTags.Add(tag);
                }
            }
            item.Tags = newTags;
        }

        private class TagComparer : IEqualityComparer<Tag>
        {
            public bool Equals(Tag? x, Tag? y)
            {
                if (x == y) return true;
                if (x == null || y == null) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode([DisallowNull] Tag tag)
            {
                return tag.Id.GetHashCode();
            }
        }

    }
}
