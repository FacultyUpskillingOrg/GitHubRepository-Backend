using GitRepositoryTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace GitRepositoryTracker.Interfaces
{
    public interface IJwtService
    {
        AuthenticationResponse CreateToken(IdentityUser user);
    }

}
