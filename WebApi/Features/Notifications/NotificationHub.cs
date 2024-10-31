using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Features.Notifications;

[Authorize]
public class NotificationHub : Hub
{
    // Method to add user to a specific group
    [Authorize("RoleRestricted")]
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("GroupMethod", $"{Context.ConnectionId} has joined the group {groupName}");
    }
}
