namespace KingJwtAuth.Entities
{
    public class UserToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime Exp { get; set; }
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
    }
}
