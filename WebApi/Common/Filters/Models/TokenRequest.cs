﻿using WebApi.Data.Entities;

namespace WebApi.Common.Filters.Models;

public record TokenRequest
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public LoginMethod LoginMethod { get; set; }
    public Role Role { get; set; }
    public UserStatus Status { get; set; }
}