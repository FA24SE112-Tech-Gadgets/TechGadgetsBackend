namespace WebApi.Data.Entities;

public class UserVerify
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string VerifyCode { get; set; } = default!;
    public VerifyStatus VerifyStatus { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = default!;
}

public enum VerifyStatus
{
    Pending, Verified, Expired
}