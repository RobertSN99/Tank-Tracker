namespace API.Models.Entities
{
    public class TankClass
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public ICollection<Tank> Tanks { get; set; } = new List<Tank>();
    }
}
