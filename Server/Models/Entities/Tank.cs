namespace Server.Models.Entities
{
    public class Tank
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Tier { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public int NationId { get; set; }
        public Nation? Nation { get; set; } = null;

        public int TankClassId { get; set; }
        public TankClass? TankClass { get; set; } = null;

        public int StatusId { get; set; }
        public Status? Status { get; set; } = null;
    }
}
