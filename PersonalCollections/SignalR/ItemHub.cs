using Application.Common.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PersonalCollections.SignalR
{
    public class ItemHub : Hub
    {
        private readonly IItemService _itemService;

        public ItemHub(IItemService itemService)
        {
            _itemService = itemService;
        }

        [Authorize]
        public async Task JoinItemGroup(string itemId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, itemId);
        }
    }
}
