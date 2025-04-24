using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Helpers;
using Server.Models.DTOs;
using Server.Models.Entities;
using Server.Services.Interfaces;
using System.Linq.Expressions;

namespace Server.Services
{
    public class TankService : ITankService
    {
        private readonly AppDbContext _context;
        public TankService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResult<object>> GetAllTanksAsync(
            int pageNumber, int pageSize,
            List<string>? nationNames, List<string>? statusNames, List<string>? tankClassNames,
            List<int>? tiers, List<double>? ratings,
            string? searchTerm, string? sortBy, string? sortOrder)
        {
            // Validate the page number and size
            if (pageNumber <= 0 || pageSize <= 0)
                return ServiceResult<object>.FailureResult("Page number and page size must be greater than zero.");

            var query = _context.Tanks
                .Include(t => t.Nation)
                .Include(t => t.TankClass)
                .Include(t => t.Status)
                .AsQueryable();

            // Validate the optional parameters
            if (nationNames?.Count > 0)
            {
                foreach (var nation in nationNames)
                {
                    var validateNation = await MyValidator.ValidateNationByNameAsync(nation, _context);
                    if (!validateNation.Success) return validateNation;
                }
                query = query.Where(t => nationNames.Contains(t.Nation.Name));
            }

            if (statusNames?.Count > 0)
            {
                foreach (var status in statusNames)
                {
                    var validateStatus = await MyValidator.ValidateStatusByNameAsync(status, _context);
                    if (!validateStatus.Success) return validateStatus;
                }
                query = query.Where(t => statusNames.Contains(t.Status.Name));
            }

            if (tankClassNames?.Count > 0)
            {
                foreach (var tankClass in tankClassNames)
                {
                    var validateTankClass = await MyValidator.ValidateTankClassByNameAsync(tankClass, _context);
                    if (!validateTankClass.Success) return validateTankClass;
                }
                query = query.Where(t => tankClassNames.Contains(t.TankClass.Name));
            }

            if (tiers?.Count > 0)
            {
                if (tiers.Any(t => t < 1 || t > 10))
                    return ServiceResult<object>.FailureResult("Tier must be between 1 and 10.");

                query = query.Where(t => tiers.Contains(t.Tier));
            }

            if (ratings?.Count > 0)
            {
                if (ratings.Any(r => r < 0 || r > 5))
                    return ServiceResult<object>.FailureResult("Rating must be between 0 and 5.");

                query = query.Where(t => ratings.Contains(t.Rating));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t => t.Name.Contains(searchTerm));
            }

            // Sort logic with dictionary mapping
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var sortMappings = new Dictionary<string, Expression<Func<Tank, object>>>
                {
                    ["Name"] = t => t.Name,
                    ["Tier"] = t => t.Tier,
                    ["Rating"] = t => t.Rating,
                    ["CreatedAt"] = t => t.CreatedAt,
                    ["UpdatedAt"] = t => t.UpdatedAt,
                    ["Nation.Name"] = t => t.Nation.Name,
                    ["TankClass.Name"] = t => t.TankClass.Name,
                    ["Status.Name"] = t => t.Status.Name
                };

                if (!sortMappings.ContainsKey(sortBy))
                    return ServiceResult<object>.FailureResult($"Invalid sort property: {sortBy}");

                bool isAscending = sortOrder?.ToLower() == "asc";

                var sortExpression = sortMappings[sortBy];

                query = isAscending
                    ? query.OrderBy(sortExpression)
                    : query.OrderByDescending(sortExpression);
            }

            var totalCount = await query.CountAsync();

            var tanks = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => DTOMapper.ToTankDTO(t))
                .ToListAsync();

            if (tanks == null || tanks.Count == 0)
                return ServiceResult<object>.FailureResult("No tanks found.", null, 404);

            var result = new
            {
                Items = tanks,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            return ServiceResult<object>.SuccessResult(result, "Tanks retrieved successfully");
        }

        public async Task<ServiceResult<object>> GetTankByIdAsync(int tankId)
        {
            // Validate the tankId
            var validationResult = await MyValidator.ValidateTankByIdAsync(tankId, _context);
            if (!validationResult.Success) return validationResult;

            // Fetch the tank
            var tank = (Tank)validationResult.Data!;
            var dto = DTOMapper.ToTankDTO(tank);
            return ServiceResult<object>.SuccessResult(dto, "Tank retrieved successfully.");
        }

        public async Task<ServiceResult<object>> CreateTankAsync(TankCreateDTO dto)
        {
            // Null or empty property validation
            foreach (var prop in dto.GetType().GetProperties())
            {
                var value = prop.GetValue(dto);
                if (value == null)
                    return ServiceResult<object>.FailureResult($"{prop.Name} cannot be null.");

                if (value is string str)
                {
                    var strValidation = MyValidator.AgainstNullOrEmpty(str, prop.Name);
                    if (!strValidation.Succeeded)
                        return ServiceResult<object>.FailureResult(strValidation.Errors.First().Description);
                }
            }

            if (dto.Tier < 1 || dto.Tier > 10)
                return ServiceResult<object>.FailureResult("Tier must be between 1 and 10.");

            if (dto.Rating < 0 || dto.Rating > 5)
                return ServiceResult<object>.FailureResult("Rating must be between 0 and 5.");

            // Check if tank name already exists
            var tankExists = await _context.Tanks.AnyAsync(t => t.Name == dto.Name);
            if (tankExists)
                return ServiceResult<object>.FailureResult($"Tank with name '{dto.Name}' already exists.", null, 409);

            // Validate foreign keys and get data
            var nationResult = await MyValidator.ValidateNationByIdAsync(dto.NationId, _context);
            if (!nationResult.Success) return nationResult;

            var classResult = await MyValidator.ValidateTankClassByIdAsync(dto.TankClassId, _context);
            if (!classResult.Success) return classResult;

            var statusResult = await MyValidator.ValidateStatusByIdAsync(dto.StatusId, _context);
            if (!statusResult.Success) return statusResult;

            var newTank = new Tank
            {
                Name = dto.Name,
                Tier = dto.Tier,
                Rating = dto.Rating,
                ImageURL = dto.ImageURL,
                NationId = dto.NationId,
                TankClassId = dto.TankClassId,
                StatusId = dto.StatusId,
                Nation = (Nation)nationResult.Data!,
                TankClass = (TankClass)classResult.Data!,
                Status = (Status)statusResult.Data!,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tanks.Add(newTank);
            var result = await _context.SaveChangesAsync();

            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to create the tank.");

            return ServiceResult<object>.SuccessResult(DTOMapper.ToTankDTO(newTank), "Tank created successfully.", 201);
        }

        public async Task<ServiceResult<object>> UpdateTankAsync(int tankId, TankUpdateDTO tankUpdateDTO)
        {
            // Validate the tankId
            var validationResult = await MyValidator.ValidateTankByIdAsync(tankId, _context);
            if (!validationResult.Success) return validationResult;

            var tank = (Tank)validationResult.Data!;

            // Validate the tankUpdateDTO

            // If name provided, check if exists
            if (!string.IsNullOrWhiteSpace(tankUpdateDTO.Name))
            {
                if (tankUpdateDTO.Name != tank.Name)
                {
                    var nameExists = await MyValidator.ValidateTankByNameAsync(tankUpdateDTO.Name, _context);
                    if (nameExists.Success)
                        return ServiceResult<object>.FailureResult($"Tank with name '{tankUpdateDTO.Name}' already exists.", null, 409);
                    tank.Name = tankUpdateDTO.Name;
                }
            }

            // If tier provided, check if in bounds
            if (tankUpdateDTO.Tier.HasValue)
            {
                var tierInBounds = MyValidator.AgainstCondition(tankUpdateDTO.Tier.Value < 1 || tankUpdateDTO.Tier.Value > 10, "Tier must be between 1 and 10.");
                if (!tierInBounds.Succeeded)
                    return ServiceResult<object>.FailureResult(tierInBounds.Errors.First().Description, tierInBounds.Errors.Select(e => e.Description));
                tank.Tier = tankUpdateDTO.Tier.Value;
            }

            // If rating provided, check if in bounds
            if (tankUpdateDTO.Rating.HasValue)
            {
                var ratingInBounds = MyValidator.AgainstCondition(tankUpdateDTO.Rating.Value < 0 || tankUpdateDTO.Rating.Value > 5, "Rating must be between 0 and 5.");
                if (!ratingInBounds.Succeeded)
                    return ServiceResult<object>.FailureResult(ratingInBounds.Errors.First().Description, ratingInBounds.Errors.Select(e => e.Description));
                tank.Rating = tankUpdateDTO.Rating.Value;
            }

            // If imageurl provided, update it
            if (!string.IsNullOrWhiteSpace(tankUpdateDTO.ImageURL))
                tank.ImageURL = tankUpdateDTO.ImageURL;

            // If nation provided, check if exists
            if (tankUpdateDTO.NationId.HasValue)
            {
                var nationCheck = await MyValidator.ValidateNationByIdAsync(tankUpdateDTO.NationId.Value, _context);
                if (!nationCheck.Success) return nationCheck;
                tank.NationId = tankUpdateDTO.NationId.Value;
                tank.Nation = (Nation)nationCheck.Data!;
            }

            // If tank class provided, check if exists
            if (tankUpdateDTO.TankClassId.HasValue)
            {
                var tankClassCheck = await MyValidator.ValidateTankClassByIdAsync(tankUpdateDTO.TankClassId.Value, _context);
                if (!tankClassCheck.Success) return tankClassCheck;
                tank.TankClassId = tankUpdateDTO.TankClassId.Value;
                tank.TankClass = (TankClass)tankClassCheck.Data!;
            }

            // If status provided, check if exists
            if (tankUpdateDTO.StatusId.HasValue)
            {
                var statusCheck = await MyValidator.ValidateStatusByIdAsync(tankUpdateDTO.StatusId.Value, _context);
                if (!statusCheck.Success) return statusCheck;
                tank.StatusId = tankUpdateDTO.StatusId.Value;
                tank.Status = (Status)statusCheck.Data!;
            }

            tank.UpdatedAt = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to update the tank.");
            return ServiceResult<object>.SuccessResult(DTOMapper.ToTankDTO(tank), "Tank updated successfully.");
        }
        public async Task<ServiceResult<object>> DeleteTankAsync(int tankId)
        {
            // Validate the tankId
            var validationResult = await MyValidator.ValidateTankByIdAsync(tankId, _context);
            if (!validationResult.Success) return validationResult;

            var tankToDelete = (Tank)validationResult.Data!;
            _context.Tanks.Remove(tankToDelete);

            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to delete the tank.");

            return ServiceResult<object>.SuccessResult(null, "Tank deleted successfully.", 204);
        }

    }
}
