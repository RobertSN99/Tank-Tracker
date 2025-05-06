namespace API.Models.DTOs
{
    public class UserSessionDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime? LogoutTime { get; set; }
    }
}