namespace Server.Models.DTOs
{
    public class TankClassDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<TankDTO> Tanks { get; set; } = new List<TankDTO>();
    }

    public class TankClassCreateDTO
    {
        public string Name { get; set; } = string.Empty;
    }

    public class TankClassUpdateDTO
    {
        public string Name { get; set; } = string.Empty;
    }
}
