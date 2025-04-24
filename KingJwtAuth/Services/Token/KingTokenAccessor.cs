using KingJwtAuth.Models;

namespace KingJwtAuth.Services.Token
{
    public class KingTokenAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly TokenService _tokenService;

        public KingTokenAccessor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _tokenService = new TokenService();
        }

        public UserTokenDto GetUserFromToken()
        {
            var cookies = _contextAccessor.HttpContext?.Request?.Cookies;
            if (cookies == null) return null;

            if (!cookies.TryGetValue(TokenStatics.AuthCookieName, out string token))
                return null;

            if (_tokenService.ValidateKingToken(token, TokenStatics.TokenKey, out UserTokenDto user))
                return user;

            return null;
        }
    }
}
