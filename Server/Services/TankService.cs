using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Helpers;
using Server.Models.DTOs;
using Server.Models.Entities;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class TankService : ITankService
    {
        private readonly AppDbContext _context;
        public TankService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResult<object>> GetAllTanksAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return ServiceResult<object>.FailureResult("Page number and page size must be greater than zero.");

            var totalCount = await _context.Tanks.CountAsync();
            var tanks = await _context.Tanks
                .Include(t => t.Nation)
                .Include(t => t.TankClass)
                .Include(t => t.Status)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TankDTO
                {
                    Id = t.Id,
                    Name = t.Name!,
                    Tier = t.Tier,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    NationId = t.NationId,
                    NationName = t.Nation.Name,
                    TankClassId = t.TankClassId,
                    TankClassName = t.TankClass.Name,
                    StatusId = t.StatusId,
                    StatusName = t.Status.Name
                })
                .ToListAsync();

            if (tanks == null || tanks.Count == 0)
                return ServiceResult<object>.FailureResult("No tanks found.");

            var result = new
            {
                Items = tanks,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            return ServiceResult<object>.SuccessResult(result);
        }
        public async Task<ServiceResult<object>> GetTankByIdAsync(int tankId)
        {
            // Validate the tankId
            var validationResult = await MyValidator.ValidateTankByIdAsync(tankId, _context);
            if (!validationResult.Success) return validationResult;

            // Fetch and return the tank details
            var tank = validationResult.Data as Tank;
            var tankOut = new TankDTO
            {
                Id = tank!.Id,
                Name = tank.Name!,
                Tier = tank.Tier,
                CreatedAt = tank.CreatedAt,
                UpdatedAt = tank.UpdatedAt,
                NationId = tank.NationId,
                NationName = tank.Nation.Name,
                TankClassId = tank.TankClassId,
                TankClassName = tank.TankClass.Name,
                StatusId = tank.StatusId,
                StatusName = tank.Status.Name
            };
            return ServiceResult<object>.SuccessResult(tankOut);
        }
        public async Task<ServiceResult<object>> CreateTankAsync(TankCreateDTO tankCreateDTO)
        {
            // Validate the input
            foreach (var prop in tankCreateDTO.GetType().GetProperties())
            {
                var value = prop.GetValue(tankCreateDTO);
                if (value is null)
                    return ServiceResult<object>.FailureResult($"{prop.Name} cannot be null or empty.");

                if (value is string strValue)
                {
                    var validationResult = MyValidator.AgainstNullOrEmpty(strValue, prop.Name);
                    if (!validationResult.Succeeded)
                        return ServiceResult<object>.FailureResult(validationResult.Errors.First().Description, validationResult.Errors.Select(e => e.Description));
                }
            }

            // Check if tier prop is in bounds
            if (tankCreateDTO.Tier < 1 || tankCreateDTO.Tier > 10)
                return ServiceResult<object>.FailureResult("Tier must be between 1 and 10.");

            // Check if the nation exists
            var nationExists = await MyValidator.ValidateNationByIdAsync(tankCreateDTO.NationId, _context);
            if (!nationExists.Success) return nationExists;

            // Check if the tank class exists
            var tankClassExists = await MyValidator.ValidateTankClassByIdAsync(tankCreateDTO.TankClassId, _context);
            if (!tankClassExists.Success) return tankClassExists;

            // Check if the status exists
            var statusExists = await MyValidator.ValidateStatusByIdAsync(tankCreateDTO.StatusId, _context);
            if (!statusExists.Success) return statusExists;

            // Check if the tank already exists
            var existsCheck = await _context.Tanks
                .AnyAsync(t => t.Name == tankCreateDTO.Name);
            if (existsCheck)
                return ServiceResult<object>.FailureResult($"Tank with name '{tankCreateDTO.Name}' already exists.");

            // Create the new tank
            var newTank = new Tank
            {
                Name = tankCreateDTO.Name,
                NationId = tankCreateDTO.NationId,
                Nation = (Nation)nationExists.Data!,
                TankClassId = tankCreateDTO.TankClassId,
                TankClass = (TankClass)tankClassExists.Data!,
                StatusId = tankCreateDTO.StatusId,
                Status = (Status)statusExists.Data!,
                Tier = tankCreateDTO.Tier,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tanks.Add(newTank);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to create the tank.");

            return ServiceResult<object>.SuccessResult("Tank created successfully.");
        }
        public async Task<ServiceResult<object>> UpdateTankAsync(int tankId, TankUpdateDTO tankUpdateDTO)
        {
            // Validate the tankId
            var validationResult = await MyValidator.ValidateTankByIdAsync(tankId, _context);
            if (!validationResult.Success) return validationResult;

            // Validate the tankUpdateDTO
            foreach (var prop in tankUpdateDTO.GetType().GetProperties())
            {
                var value = prop.GetValue(tankUpdateDTO);
                if (value is null)
                    return ServiceResult<object>.FailureResult($"{prop.Name} cannot be null or empty.");

                if (value is string strValue)
                {
                    var strCheck = MyValidator.AgainstNullOrEmpty(strValue, prop.Name);
                    if (!strCheck.Succeeded)
                        return ServiceResult<object>.FailureResult(strCheck.Errors.First().Description, strCheck.Errors.Select(e => e.Description));
                }
            }

            var tankToUpdate = (Tank)validationResult.Data!;

            // Check if the name already exists
            if (tankUpdateDTO.Name != null && (tankToUpdate.Name != tankUpdateDTO.Name))
            {
                var nameExists = await _context.Tanks
                    .AnyAsync(t => t.Name == tankUpdateDTO.Name);
                if (nameExists)
                    return ServiceResult<object>.FailureResult($"Tank with name '{tankUpdateDTO.Name}' already exists.");
                tankToUpdate.Name = tankUpdateDTO.Name;
            }

            // Check if the tier prop is in bounds
            if (tankUpdateDTO.Tier != null)
            {
                var isInBounds = MyValidator.AgainstCondition(tankUpdateDTO.Tier < 1 || tankUpdateDTO.Tier > 10, "Tier must be between 1 and 10.");
                if (!isInBounds.Succeeded)
                    return ServiceResult<object>.FailureResult(isInBounds.Errors.First().Description, isInBounds.Errors.Select(e => e.Description));
                tankToUpdate.Tier = (int)tankUpdateDTO.Tier;
            }

            // Check if the nation exists
            if (tankUpdateDTO.NationId != null)
            {
                var nationExists = await MyValidator.ValidateNationByIdAsync(tankUpdateDTO.NationId.Value, _context);
                if (!nationExists.Success) return nationExists;
                tankToUpdate.NationId = (int)tankUpdateDTO.NationId;
                tankToUpdate.Nation = (Nation)nationExists.Data!;
            }

            // Check if the tank class exists
            if (tankUpdateDTO.TankClassId != null)
            {
                var tankClassExists = await MyValidator.ValidateTankClassByIdAsync(tankUpdateDTO.TankClassId.Value, _context);
                if (!tankClassExists.Success) return tankClassExists;
                tankToUpdate.TankClassId = (int)tankUpdateDTO.TankClassId;
                tankToUpdate.TankClass = (TankClass)tankClassExists.Data!;
            }

            // Check if the status exists
            if (tankUpdateDTO.StatusId != null)
            {
                var statusExists = await MyValidator.ValidateStatusByIdAsync(tankUpdateDTO.StatusId.Value, _context);
                if (!statusExists.Success) return statusExists;
                tankToUpdate.StatusId = (int)tankUpdateDTO.StatusId;
                tankToUpdate.Status = (Status)statusExists.Data!;
            }

            _context.Tanks.Update(tankToUpdate!);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to update the tank.");

            return ServiceResult<object>.SuccessResult(tankToUpdate, "Tank updated successfully.");
        }
        public async Task<ServiceResult<object>> DeleteTankAsync(int tankId)
        {
            // Validate the tankId
            var validationResult = await MyValidator.ValidateTankByIdAsync(tankId, _context);
            if (!validationResult.Success) return validationResult;

            // Deletion process
            var tankToDelete = validationResult.Data as Tank;
            if (tankToDelete == null)
                return ServiceResult<object>.FailureResult("Tank not found.");

            _context.Tanks.Remove(tankToDelete);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to delete the tank.");

            return ServiceResult<object>.SuccessResult(tankToDelete, "Tank deleted successfully.");
        }
    }
}
