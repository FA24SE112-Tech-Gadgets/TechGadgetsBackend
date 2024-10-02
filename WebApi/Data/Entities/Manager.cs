﻿namespace WebApi.Data.Entities;

public class Manager
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = default!;

    public User User { get; set; } = default!;
}
