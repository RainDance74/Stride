using Microsoft.AspNetCore.Identity;
using Stride.Domain.Entities;

namespace Stride.Infrastucture.Identity;

public class ApplicationUser : IdentityUser, IUser<string> { }
