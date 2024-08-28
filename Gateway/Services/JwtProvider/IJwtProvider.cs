using Gateway.Persistence;

namespace Gateway.Services.JwtProvider
{
    public interface IJwtProvider
    {
        string GenerateJwt(User user);
    }
}
