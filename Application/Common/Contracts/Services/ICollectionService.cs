using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Collection;

namespace Application.Common.Contracts.Services
{
    public interface ICollectionService
    {
        Task AddAsync(AddCollectionRequest request);
        Task<EditCollectionResponse> UpdateAsync(EditCollectionRequest request);
        Task<RemoveCollectionResponse> RemoveAsync(string collectionId);
        Task<IEnumerable<CollectionDto>> GetCollectionsAsync(string userId);
        Task<CollectionDto?> GetByIdAsync(string id);
    }
}
