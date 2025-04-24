namespace Server.Models.DTOs
{
    public class TankDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Tier { get; set; }
        public double Rating { get; set; }
        public string? ImageURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int NationId { get; set; }
        public string NationName { get; set; } = string.Empty;
        public int TankClassId { get; set; }
        public string TankClassName { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }

    public class TankCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Tier { get; set; }
        public double Rating { get; set; }
        public string? ImageURL{ get; set; }
        public int NationId { get; set; }
        public int TankClassId { get; set; }
        public int StatusId { get; set; }
    }

    public class TankUpdateDTO
    {
        public string? Name { get; set; }
        public int? Tier { get; set; }
        public double? Rating { get; set; }
        public string? ImageURL { get; set; }
        public int? NationId { get; set; }
        public int? TankClassId { get; set; }
        public int? StatusId { get; set; }
    }
}
