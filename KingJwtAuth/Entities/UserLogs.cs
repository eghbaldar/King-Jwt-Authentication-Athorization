namespace KingJwtAuth.Entities
{
    public class UserLogs
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RequestPath { get; set; }
        public string Method { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public bool Auth { get; set; } // {true}: the user is authenticated  {false}: the user is unautheticated
    }
}
