using Microsoft.AspNetCore.Identity;
using Stride.Domain.Entities;

namespace Stride.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public StrideUser StrideUser { get; init; } = null!;
}
