using System.Security.Claims;
using Application.Common.Contracts.Services;
using Application.Models.Item;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;

namespace PersonalCollections.SignalR
{
    public class ItemHub : Hub
    {
        private readonly IItemService _itemService;
        private readonly IStringLocalizer<ItemHub> _localizer;

        public ItemHub(IItemService itemService, IStringLocalizer<ItemHub> localizer)
        {
            _itemService = itemService;
            _localizer = localizer;
        }

        [Authorize]
        public async Task JoinItemGroup(string itemId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, itemId);
        }

        [Authorize]
        public async Task Like(string itemId, string action = "like")
        {
            string userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            bool result;

            if(action == "like")
            {
                result = await _itemService.AddLikeAsync(userId, itemId);
            }
            else if(action == "dislike")
            {
                result = await _itemService.RemoveLikeAsync(userId, itemId);
            }
            else
            {
                return;
            }

            if(result)
            {
                var likes = await _itemService.CountLikesAsync(itemId);
                await Clients.Caller.SendAsync("LikeSuccess");
                await Clients.Group(itemId).SendAsync("LikesUpdate", likes);
            }
        }

        [Authorize]
        public async Task Comment(CommentDto comment)
        {
            string userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if(string.IsNullOrEmpty(comment.Text))
            {
                await Clients.Caller.SendAsync("CommentFailed", _localizer["EmptyText"].Value);
                return;
            }

            comment.UserId = userId;
            var result = await _itemService.AddCommentAsync(comment);

            if (!result.Succeeded)
            {
                await Clients.Caller.SendAsync("CommentFailed", _localizer["Failed"].Value);
                return;
            }

            var date = comment.SentTime.ToLocalTime().ToShortDateString() + " " + comment.SentTime.ToLocalTime().ToShortTimeString();
            await Clients.Group(comment.ItemId).SendAsync("NewComment", result.Comment, date);
        }
    }
}
