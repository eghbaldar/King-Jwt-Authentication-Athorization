using KingJwtAuth.Context;
using Microsoft.EntityFrameworkCore;

namespace KingJwtAuth.Services.UserRefreshToken
{
    public class RequestUserRefreshTokenServiceDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public bool IsRevoke { get; set; }
        public DateTime Exp { get; set; }
    }
    public class ResultUserRefreshTokenServiceDto
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
    }
    public class UserRefreshTokenService
    {
        private readonly IDataBaseContext _context;
        public UserRefreshTokenService(IDataBaseContext context)
        {
            _context = context;
        }
        public bool PostUserRefreshToken(RequestUserRefreshTokenServiceDto req)
        {
            // Mark all previous user's token as PROVOKED
            var previousUserToken = _context.UserRefreshToken.Where(x => x.UserId == req.UserId).ToList();
            foreach (var token in previousUserToken) token.IsRevoke = true;

            // insert the new token
            KingJwtAuth.Entities.UserRefreshToken userRefreshToken = new Entities.UserRefreshToken()
            {
                Exp = req.Exp,
                IP = req.IP,
                Token = req.Token,
                UserAgent = req.UserAgent,
                UserId = req.UserId,
            };
            _context.UserRefreshToken.Add(userRefreshToken);
            _context.SaveChanges();
            return true;
        }
        public ResultUserRefreshTokenServiceDto GetRefreshTokenByToken(string token)
        {
            //TODO: compare x.exp with current datatime as well!
            var result = _context.UserRefreshToken
                .Include(x=>x.User)
                .Where(x => x.Token == token && !x.IsRevoke)
                .Select(x => new ResultUserRefreshTokenServiceDto
                {
                    Token = x.Token,
                    UserId = x.UserId,
                    Role = x.User.Role,
                })
                .FirstOrDefault();
            return result;
        }
    }
}