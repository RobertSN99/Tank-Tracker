namespace Server.Models.DTOs
{
    public class StatusDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<TankDTO> Tanks { get; set; } = new List<TankDTO>();
    }

    public class StatusCreateDTO
    {
        public string Name { get; set; } = string.Empty;
    }

    public class StatusUpdateDTO
    {
        public string? Name { get; set; } = string.Empty;
    }
}
