using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Avatar { get; set; }

    public bool? IsEmailVerified { get; set; }

    public bool? IsPhoneVerified { get; set; }

    public bool? TwoFactorEnabled { get; set; }

    public string? TwoFactorKey { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime? LockoutEndDate { get; set; }

    public int? AccessFailedCount { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryDate { get; set; }

    public bool? IsActive { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CustomerProfile> CustomerProfiles { get; set; } = new List<CustomerProfile>();
}
