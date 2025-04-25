namespace KingJwtAuth.Entities
{
    public class Users
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
