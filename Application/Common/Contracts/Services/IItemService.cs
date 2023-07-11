using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Collection;
using Application.Models.Item;

namespace Application.Common.Contracts.Services
{
    public interface IItemService
    {
        Task<List<ItemFieldDto>> GetItemFieldsAsync(string collectionId);
        Task<IEnumerable<TagDto>> GetTagsByPrefixAsync(string prefix);
        Task<AddItemResponse> AddItemAsync(AddItemRequest request);
        Task UpdateItemAsync(ItemDto request);
        Task<IEnumerable<ItemDto>> GetItemsAsync(GetItemsRequest request, CancellationToken cancellationToken);
        Task<int> CountItemsAsync(string collectionId, string filter, IEnumerable<GetItemsRequest.DateEntry> dateEntries, CancellationToken cancellationToken);
        Task<RemoveItemResponse> RemoveAsync(string itemId);
        Task<ItemDto?> GetByIdAsync(string itemId);
    }
}
