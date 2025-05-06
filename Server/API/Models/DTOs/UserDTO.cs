namespace API.Models.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class UserUpdateDTO
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }

    public class UserPasswordChangeDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
