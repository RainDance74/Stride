using Stride.Application.Common.Interfaces;
using Stride.Domain.Entities;
using Stride.Infrastructure.Data;
using System.Security.Claims;

namespace Stride.Web.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
