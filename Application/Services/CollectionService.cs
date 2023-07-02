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

            if (request.Image is not null)
            {
                var imageUrl = await _cloudStorageService.UploadAsync(request.Image, Guid.NewGuid().ToString());
                collection.ImageUrl = imageUrl;
            }


            var fields = request.Fields.Select(x => new CollectionField
            {
                Id = Guid.NewGuid().ToString(),
                Name = x.Name,
                CollectionFieldTypeId = x.TypeId
            });

            collection.Fields.AddRange(fields);
            _context.Collections.Add(collection);

            await _context.SaveChangesAsync();
        }
    }
}
