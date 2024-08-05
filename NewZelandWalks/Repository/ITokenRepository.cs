using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repository
{
    public interface ITokenRepository
    {
       string CreateJWTToken(IdentityUser user, List<String> roles);
    }
}
