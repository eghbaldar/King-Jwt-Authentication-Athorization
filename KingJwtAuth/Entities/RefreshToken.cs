namespace KingJwtAuth.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Token { get; set; }        
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public bool IsRevoke { get; set; }
        public DateTime Exp { get; set; }
    }
}
