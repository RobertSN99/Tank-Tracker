using System.ComponentModel.DataAnnotations;

namespace Server.Models.Entities
{
    public class Tank
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Range(1, 10, ErrorMessage = "Tier must be between 1 and 10.")]
        public int Tier { get; set; }
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double Rating { get; set; } = 0;
        public string? ImageURL { get; set; }
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
