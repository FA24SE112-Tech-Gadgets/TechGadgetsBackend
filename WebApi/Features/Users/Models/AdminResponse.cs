namespace WebApi.Features.Users.Models;

public class AdminResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;
}
