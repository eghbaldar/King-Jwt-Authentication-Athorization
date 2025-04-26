using KingJwtAuth.Context;

namespace KingJwtAuth.Services.UserLogs
{
    public class RequestUserLogsServiceDto
    {
        public string RequestPath { get; set; }
        public string Method { get; set; }
        public string IP { get; set; }
        public string MethodName { get; set; }
        public string UserAgent { get; set; }
        public bool Auth { get; set; }
    }
    public class UserLogsService
    {
        private readonly IDataBaseContext _context;
        public UserLogsService(IDataBaseContext context)
        {
            _context = context;
        }
        public void PostUserLog(RequestUserLogsServiceDto req)
        {
            KingJwtAuth.Entities.UserLogs userLogs = new Entities.UserLogs()
            {
                IP = req.IP,
                Method = req.Method,
                UserAgent = req.UserAgent,
                RequestPath = req.RequestPath,
                Auth = req.Auth,
                MethodName = req.MethodName,
            };
            _context.UserLogs.Add(userLogs);
            _context.SaveChanges();
        }
    }
}
