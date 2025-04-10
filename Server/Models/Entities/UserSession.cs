namespace Server.Models.Entities
{
    public class UserSession
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;
        public DateTime? LogoutTime { get; set; } = null;
        public DateTime ExpirationTime { get; set; }
        public string? IPAddress { get; set; } = null;
        public string? UserAgent { get; set; } = null;
    }
}
