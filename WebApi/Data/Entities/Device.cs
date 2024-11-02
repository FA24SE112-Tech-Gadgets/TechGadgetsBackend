namespace WebApi.Data.Entities;

public class Device
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = default!;

    public User User { get; set; } = default!;
}
