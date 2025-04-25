using KingJwtAuth.Context;

namespace KingJwtAuth.Services.Users
{
    public interface IUsersService
    {
        GetUsersServiceDto GetUserById(RequestGetUsersServiceDto req);
    }
    /// <summary>
    /// ==================================================
    /// </summary>
    /// 
    public class RequestGetUsersServiceDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class GetUsersServiceDto
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
    public class UsersService : IUsersService
    {
        private readonly IDataBaseContext _context;
        public UsersService(IDataBaseContext context)
        {
            _context = context;
        }
        public GetUsersServiceDto GetUserById(RequestGetUsersServiceDto req)
        {
            var user = _context.Users.Where(x => x.Username == req.Username && x.Password == req.Password);
            if (user == null) return null;
            else
            {
                return user.Select(x => new GetUsersServiceDto
                {
                    Fullname = x.Username,
                    Id = x.Id,
                    Password = x.Password,
                    Role = x.Role,
                    Username = x.Username,
                }).First();
            }
        }
    }
}
