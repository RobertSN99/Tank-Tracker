using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Helpers;
using Server.Models.DTOs;
using Server.Models.Entities;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class TankClassService : ITankClassService
    {
        private readonly AppDbContext _context;

        public TankClassService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<object>> CreateClassAsync(TankClassCreateDTO tankClassDto)
        {
            // Validate the input
            var validationResult = MyValidator.AgainstNullOrEmpty(tankClassDto.Name, nameof(tankClassDto.Name));
            if (!validationResult.Succeeded)
                return ServiceResult<object>.FailureResult(validationResult.Errors.First().Description, validationResult.Errors.Select(e => e.Description));

            // Check if the tank class already exists
            var existsCheck = await _context.TankClasses
                .AnyAsync(tc => tc.Name == tankClassDto.Name);
            if (existsCheck)
                return ServiceResult<object>.FailureResult($"Tank class with name '{tankClassDto.Name}' already exists.");

            // Create the new tank class
            var newTankClass = new TankClass
            {
                Name = tankClassDto.Name,
                CreatedAt = DateTime.UtcNow
            };

            // Add the new tank class to the database
            _context.TankClasses.Add(newTankClass);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to create the tank class in the database.");

            return ServiceResult<object>.SuccessResult(newTankClass, "Tank class created successfully.");
        }
        public async Task<ServiceResult<object>> DeleteClassAsync(int classId)
        {
            // Validate the input
            var validationResult = await MyValidator.ValidateTankClassByIdAsync(classId, _context);
            if (!validationResult.Success) return validationResult;

            // Deletion process
            var tankClass = validationResult.Data as TankClass;
            _context.TankClasses.Remove(tankClass!);
            var result = await _context.SaveChangesAsync();

            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to delete the tank class from the database.");

            return ServiceResult<object>.SuccessResult(null, "Tank class deleted successfully.");

        }
        public async Task<ServiceResult<object>> GetAllClassesAsync()
        {
            // Retrieve all tank classes from the database
            var tankClasses = await _context.TankClasses
                .Include(tc => tc.Tanks)
                .ToListAsync();

            if (tankClasses == null || tankClasses.Count == 0)
                return ServiceResult<object>.FailureResult("No tank classes found in the database.");
            return ServiceResult<object>.SuccessResult(tankClasses, "Tank classes retrieved successfully.");
        }
        public async Task<ServiceResult<object>> GetClassByIdAsync(int classId)
        {
            // Validate the input
            var validationResult = await MyValidator.ValidateTankClassByIdAsync(classId, _context);
            if (!validationResult.Success) return validationResult;

            // Retrieve the tank class by ID
            var tankClass = validationResult.Data as TankClass;
            return ServiceResult<object>.SuccessResult(tankClass, "Tank class retrieved successfully.");
        }
        public async Task<ServiceResult<object>> UpdateClassAsync(int classId, TankClassUpdateDTO tankClassDto)
        {
            // Validate the classId
            var validationResult = await MyValidator.ValidateTankClassByIdAsync(classId, _context);
            if (!validationResult.Success) return validationResult;

            // Validate the dto
            var validationResultDto = MyValidator.AgainstNullOrEmpty(tankClassDto.Name, nameof(tankClassDto.Name));
            if (!validationResultDto.Succeeded)
                return ServiceResult<object>.FailureResult(validationResultDto.Errors.First().Description, validationResultDto.Errors.Select(e => e.Description));

            // Check if the tank class already exists
            var existsCheck = await _context.TankClasses
                .AnyAsync(tc => tc.Name == tankClassDto.Name && tc.Id != classId);
            if (existsCheck)
                return ServiceResult<object>.FailureResult($"Tank class with name '{tankClassDto.Name}' already exists.");

            // Update the tank class
            var tankClass = validationResult.Data as TankClass;
            tankClass!.Name = tankClassDto.Name;
            tankClass.UpdatedAt = DateTime.UtcNow;
            _context.TankClasses.Update(tankClass);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to update the tank class in the database.");

            return ServiceResult<object>.SuccessResult(tankClass, "Tank class updated successfully.");
        }
    }
}
