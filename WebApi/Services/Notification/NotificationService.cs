using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebApi.Services.Notification.Models;

namespace WebApi.Services.Notification;

public class NotificationService : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        // Lấy UserId từ Claim "UserInfo" (hoặc bất kỳ claim nào bạn muốn)
        var userInfoJson = connection.User?.Claims.FirstOrDefault(c => c.Type == "UserInfo")?.Value;
        var userInfo = JsonConvert.DeserializeObject<TokenRequest>(userInfoJson!);

        // Trả về UserId làm UserIdentifier
        return userInfo?.Id.ToString()!;
    }
}
