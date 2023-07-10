using Application.Common.Contracts.Contexts;
using Application.Common.Contracts.Services;
using Application.Models.Collection;
using Domain.Entities.Items;
using Mapster;
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
                var imageUrl = await _cloudStorageService.UploadAsync(request.Image, Guid.NewGuid().ToString());
                collection.ImageUrl = imageUrl;
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
            var collection = await _context.Collections.AsNoTracking()
                .Include(x => x.Fields)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (collection is null || collection.UserId != request.UserId)
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

            request.Fields = request.Fields
                .Where(x => x.Id is null)
                .Select(x =>
                {
                    x.Id = Guid.NewGuid().ToString();
                    return x;
                })
                .ToList();
            var collectionToUpdate = request.Adapt<Collection>();
            collectionToUpdate.CreationDate = collection.CreationDate;

            if(request.Image is not null)
            {
                var imageUrl = await _cloudStorageService.UploadAsync(request.Image, Guid.NewGuid().ToString());
                collectionToUpdate.ImageUrl = imageUrl;
            }

            foreach(var field in collectionToUpdate.Fields)
            {
                field.FieldType = null;
            }

            await _context.CollectionFields.AddRangeAsync(collectionToUpdate.Fields);
            _context.Collections.Update(collectionToUpdate);
            await _context.SaveChangesAsync();

            return new EditCollectionResponse { Succeeded = true };
        }

        public async Task<RemoveCollectionResponse> RemoveAsync(string collectionId)
        {
            var collection = await _context.Collections.FindAsync(collectionId);
            if (collection is null)
            {
                return new RemoveCollectionResponse { Succeeded = false };
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
                .ProjectToType<CollectionDto>()
                .ToListAsync();

            return collections;
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
    }
}
