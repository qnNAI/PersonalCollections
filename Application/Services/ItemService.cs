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

            NormalizeTags(item);

            item.Id = Guid.NewGuid().ToString();
            await PrepareTagsAsync(item);

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
            NormalizeTags(item);

            await PrepareTagsAsync(item);

            var tagsToRemove = existing.ItemTags.Where(x => !item.ItemTags.Any(it => it.TagId == x.TagId)).ToList();
            var tagsToAdd = item.ItemTags.Where(x => !existing.ItemTags.Any(it => it.TagId == x.TagId)).ToList();
            
            _context.ItemTags.AddRange(tagsToAdd);
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

        public async Task<IEnumerable<ItemDto>> GetItemsAsync(GetItemsRequest request, CancellationToken cancellationToken)
        {
            var itemsQuery = _context.Items.AsNoTracking().Where(x => x.CollectionId == request.CollectionId);

            itemsQuery = ApplyFilters(request.Filter, request.DateEntries, itemsQuery);

            itemsQuery = request.Order switch
            {
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
                .Include(x => x.Likes)
                .ProjectToType<ItemDto>()
                .ToListAsync(cancellationToken);

            return items;
        }

        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string term, int page, int pageSize, CancellationToken cancellationToken)
        {
            var items = await _context.Items
                .AsNoTracking()
                .Where(x => EF.Functions.Contains(x.Name, term))
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Collection)
                .Include(x => x.Tags)
                .ProjectToType<ItemDto>()
                .ToListAsync(cancellationToken);

            return items;
        }

        private static IQueryable<Item> ApplyFilters(string filter, IEnumerable<GetItemsRequest.DateEntry> dateEntries, IQueryable<Item> itemsQuery)
        {
            if(!string.IsNullOrEmpty(filter))
            {
                itemsQuery = itemsQuery.Where(x => EF.Functions.Contains(x.Name, filter));
            }

            foreach(var dateEntry in dateEntries)
            {
                itemsQuery = itemsQuery.Where(x => x.Fields.First(f => f.CollectionFieldId == dateEntry.Id).Value == dateEntry.Value);
            }

            return itemsQuery;
        }

        public Task<int> CountItemsAsync(string collectionId, string filter, IEnumerable<GetItemsRequest.DateEntry> dateEntries, CancellationToken cancellationToken) {
            var itemsQuery = _context.Items.Where(x => x.CollectionId == collectionId);

            itemsQuery = ApplyFilters(filter, dateEntries, itemsQuery);

            return itemsQuery.CountAsync(cancellationToken);
        }

        public async Task<RemoveItemResponse> RemoveAsync(string itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if(item is null)
            {
                return new RemoveItemResponse { Succeeded = false };
            }

            var likes = await _context.Likes.Where(x => x.ItemId == itemId).ToListAsync();
            _context.Likes.RemoveRange(likes);
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

        public async Task<bool> AddLikeAsync(string userId, string itemId)
        {
            var isExists = (await _context.Likes.FindAsync(userId, itemId)) is not null;
            if(isExists)
            { 
                return false;
            }

            _context.Likes.Add(new Like
            {
                UserId = userId,
                ItemId = itemId
            });
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveLikeAsync(string userId, string itemId)
        {
            var existing = await _context.Likes.FindAsync(userId, itemId);
            if(existing is null)
            {
                return false;
            }

            _context.Likes.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> CountLikesAsync(string itemId)
        {
            var likes = await _context.Likes
                .Where(x => x.ItemId == itemId)
                .CountAsync();

            return likes;
        }

        public async Task<bool> IsLikedAsync(string userId, string itemId)
        {
            return (await _context.Likes.FindAsync(userId, itemId)) is not null;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsAsync(string itemId, int skip, int pageSize)
        {
            var comments = await _context.Comments.AsNoTracking()
                .Where(x => x.ItemId == itemId)
                .OrderByDescending(x => x.SentTime)
                .Skip(skip)
                .Take(pageSize)
                .Include(x => x.User)
                .ProjectToType<CommentDto>()
                .ToListAsync();

            return comments;
        }

        public async Task<AddCommentResponse> AddCommentAsync(CommentDto comment)
        {
            var isExists = await _context.Items.AnyAsync(x => x.Id == comment.ItemId);
            if (!isExists)
            {
                return new AddCommentResponse { Succeeded = false };
            }

            comment.Id = Guid.NewGuid().ToString();
            comment.SentTime = DateTime.UtcNow;
            _context.Comments.Add(comment.Adapt<Comment>());
            await _context.SaveChangesAsync();

            var created = await _context.Comments.AsNoTracking()
                .Include(x => x.User)
                .FirstAsync(x => x.Id == comment.Id);

            return new AddCommentResponse
            {
                Succeeded = true,
                Comment = created.Adapt<CommentDto>()
            };
        }

        private async Task PrepareTagsAsync(Item item)
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

        private void NormalizeTags(Item item)
        {
            foreach(var tag in item.Tags)
            {
                tag.Name = tag.Name.ToLower();
            }
        }
    }
}
