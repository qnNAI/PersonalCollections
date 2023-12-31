﻿using Application.Common.Contracts.Contexts;
using Application.Common.Contracts.Services;
using Application.Models.Collection;
using Application.Models.Item;
using Domain.Entities.Items;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    internal class CollectionService : ICollectionService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IApplicationDbContext _context;

        public CollectionService(ICloudStorageService cloudStorageService, IApplicationDbContext context)
        {
            _cloudStorageService = cloudStorageService;
            _context = context;
        }

        public async Task AddAsync(AddCollectionRequest request)
        {
            var collection = request.Adapt<Collection>();
            collection.Id = Guid.NewGuid().ToString();
            collection.CreationDate = DateTime.UtcNow;

            if (request.Image is not null)
            {
                await UploadImageAsync(request.Image, collection);
            }

            var fields = request.Fields.Select(x => new CollectionField
            {
                Id = Guid.NewGuid().ToString(),
                Name = x.Name,
                CollectionFieldTypeId = x.TypeId,
                Order = DateTime.Now.Ticks
            });

            collection.Fields.AddRange(fields);
            _context.Collections.Add(collection);

            await _context.SaveChangesAsync();
        }

        public async Task<EditCollectionResponse> UpdateAsync(EditCollectionRequest request)
        {
            var existing = await _context.Collections.AsNoTracking()
                .Include(x => x.Fields)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if(existing is null || existing.UserId != request.UserId)
            {
                return new EditCollectionResponse
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        "Collection not found!"
                    }
                };
            }

            var newFields = new List<CollectionField>();
            request.Fields = request.Fields
                .Where(x => x.Id is null)
                .Select(x =>
                {
                    x.Id = Guid.NewGuid().ToString();
                    newFields.Add(x.Adapt<CollectionField>());
                    return x;
                })
                .ToList();

            var collectionToUpdate = request.Adapt<Collection>();
            await UpdateCollectionAsync(request, existing, collectionToUpdate);

            await UpdateCollectionItems(existing, newFields);

            return new EditCollectionResponse { Succeeded = true };
        }

        private async Task UpdateCollectionItems(Collection existing, List<CollectionField> newFields)
        {
            var itemIds = _context.Items.Where(x => x.CollectionId == existing.Id).Select(x => x.Id);
            var newEntries = new List<ItemField>();
            foreach(var itemId in itemIds)
            {
                newEntries.AddRange(
                    newFields.Select(x => new ItemField
                    {
                        CollectionFieldId = x.Id,
                        ItemId = itemId
                    }));
            }

            await _context.ItemFields.BulkInsertAsync(newEntries);
        }

        private async Task UpdateCollectionAsync(EditCollectionRequest request, Collection? existing, Collection collectionToUpdate)
        {
            collectionToUpdate.CreationDate = existing.CreationDate;

            if(request.Image is not null)
            {
                if(!string.IsNullOrEmpty(existing.ImageName))
                {
                    await _cloudStorageService.DeleteAsync(existing.ImageName);
                }

                await UploadImageAsync(request.Image, collectionToUpdate);
            } 
            else
            {
                collectionToUpdate.ImageUrl = existing.ImageUrl;
                collectionToUpdate.ImageName = existing.ImageName;
            }

            foreach(var field in collectionToUpdate.Fields)
            {
                field.FieldType = null;
            }

            await _context.CollectionFields.AddRangeAsync(collectionToUpdate.Fields);
            _context.Collections.Update(collectionToUpdate);
            await _context.SaveChangesAsync();
        }

        private async Task UploadImageAsync(IFormFile image, Collection collection)
        {
            var imageName = Guid.NewGuid().ToString();
            var imageUrl = await _cloudStorageService.UploadAsync(image, imageName);
            collection.ImageUrl = imageUrl;
            collection.ImageName = imageName;
        }

        public async Task<RemoveCollectionResponse> RemoveAsync(string collectionId)
        {
            var collection = await _context.Collections.FindAsync(collectionId);
            if (collection is null)
            {
                return new RemoveCollectionResponse { Succeeded = false };
            }

            if(!string.IsNullOrEmpty(collection.ImageName))
            {
                await _cloudStorageService.DeleteAsync(collection.ImageName);
            }

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();

            return new RemoveCollectionResponse { Succeeded = true };
        }

        public async Task<IEnumerable<CollectionDto>> GetCollectionsAsync(string userId)
        {
            var collections = await _context.Collections
                .Where(x => x.UserId == userId)
                .Include(x => x.Theme)
                .OrderByDescending(x => x.CreationDate)
                .ToListAsync();

            return collections.Adapt<List<CollectionDto>>();
        }

        public async Task<IEnumerable<CollectionDto>> GetCollectionsAsync(string term, int page, int pageSize, CancellationToken cancellationToken) 
        {
            using var context = _context.CreateContext();

            var collections = await context.Collections.AsNoTracking()
                .Where(x => EF.Functions.Contains(x.Name, term) || EF.Functions.Contains(x.Description, term))
                .Include(x => x.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return collections.Adapt<List<CollectionDto>>();
        }

        public async Task<CollectionDto?> GetByIdAsync(string id) {
            var collection = (await _context.Collections
                .Include(x => x.Fields)
                    .ThenInclude(x => x.FieldType)
                .Include(x => x.Theme)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id))
                ?.Adapt<CollectionDto>();
            return collection;
        }

        public async Task<IEnumerable<CollectionDto>> GetLargestCollectionsAsync(int pageSize, CancellationToken cancellationToken)
        {
            using var context = _context.CreateContext();

            var collections = await context.Collections.AsNoTracking()
                .OrderByDescending(x => x.Items.Count)
                .Take(pageSize)
                .Include(x => x.User)
                .ToListAsync(cancellationToken);

            return collections.Adapt<List<CollectionDto>>();
        }
    }
}
