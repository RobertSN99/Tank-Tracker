namespace Server.Models.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public ICollection<Tank> Tanks { get; set; } = new List<Tank>();
    }
}
