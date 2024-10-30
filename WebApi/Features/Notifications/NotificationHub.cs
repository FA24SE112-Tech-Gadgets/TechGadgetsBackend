using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Data;
using WebApi.Features.Notifications.Models;

namespace WebApi.Features.Notifications;

[Authorize]
public class NotificationHub : Hub
{
    private readonly AppDbContext _context;

    public NotificationHub(AppDbContext context)
    {
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {
        //var userInfoJson = Context.User.Claims.FirstOrDefault(c => c.Type == "UserInfo")?.Value;

        //var userInfo = JsonConvert.DeserializeObject<TokenRequest>(userInfoJson!);

        //var user = await _context.Users
        //    .Include(u => u.Manager)
        //    .Include(u => u.Admin)
        //    .Include(u => u.Customer)
        //    .Include(u => u.Seller)
        //    .Include(u => u.Wallet)
        //    .FirstOrDefaultAsync(x => x.Id == userInfo!.Id);
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
    }
}
