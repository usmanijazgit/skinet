using Core.Entities.identity;

namespace Core.Interfaces
{
    public interface iTokenService
    {
         string CreateToken(AppUser user);
    }
}