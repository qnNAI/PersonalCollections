using System.Security.Claims;
using Application.Common.Contracts.Services;
using Application.Models.Item;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

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
                await Clients.Caller.SendAsync("CommentFailed", "Empty comment text!");
                return;
            }

            comment.UserId = userId;
            var result = await _itemService.AddCommentAsync(comment);

            if (!result.Succeeded)
            {
                await Clients.Caller.SendAsync("CommentFailed", "Failed to comment!");
                return;
            }

            var date = comment.SentTime.ToLocalTime().ToShortDateString() + " " + comment.SentTime.ToLocalTime().ToShortTimeString();
            await Clients.Group(comment.ItemId).SendAsync("NewComment", result.Comment, date);
        }
    }
}
