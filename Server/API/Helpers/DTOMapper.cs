using API.Models.DTOs;
using API.Models.Entities;

namespace API.Helpers
{
    public static class DTOMapper
    {
        public static NationDTO ToNationDTO(Nation nation)
        {
            return new NationDTO
            {
                Id = nation.Id,
                Name = nation.Name,
                CreatedAt = nation.CreatedAt,
                UpdatedAt = nation.UpdatedAt
            };
        }

        public static TankClassDTO ToTankClassDTO(TankClass tankClass)
        {
            return new TankClassDTO
            {
                Id = tankClass.Id,
                Name = tankClass.Name,
                CreatedAt = tankClass.CreatedAt,
                UpdatedAt = tankClass.UpdatedAt
            };
        }

        public static StatusDTO ToStatusDTO(Status status)
        {
            return new StatusDTO
            {
                Id = status.Id,
                Name = status.Name,
                CreatedAt = status.CreatedAt,
                UpdatedAt = status.UpdatedAt
            };
        }

        public static TankDTO ToTankDTO(Tank tank)
        {
            return new TankDTO
            {
                Id = tank.Id,
                Name = tank.Name,
                Tier = tank.Tier,
                Rating = tank.Rating,
                ImageURL = tank.ImageURL,
                NationId = tank.NationId,
                NationName = tank.Nation.Name,
                TankClassId = tank.TankClassId,
                TankClassName = tank.TankClass.Name,
                StatusId = tank.StatusId,
                StatusName = tank.Status.Name,
                CreatedAt = tank.CreatedAt,
                UpdatedAt = tank.UpdatedAt
            };
        }
    }
}
