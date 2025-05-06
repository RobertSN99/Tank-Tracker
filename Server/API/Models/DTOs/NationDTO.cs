namespace API.Models.DTOs
{
    public class NationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class NationCreateDTO
    {
        public string Name { get; set; } = string.Empty;
    }

    public class NationUpdateDTO
    {
        public string? Name { get; set; } = string.Empty;
    }
}
