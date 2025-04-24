using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Helpers;
using Server.Models.DTOs;
using Server.Models.Entities;
using Server.Services.Interfaces;

namespace Server.Services
{
    public class NationService : INationService
    {
        private readonly AppDbContext _context;

        public NationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<object>> GetAllNationsAsync()
        {
            // Retrieve all nations from the database
            var nations = await _context.Nations
                .Include(n => n.Tanks)
                .ToListAsync();
            if (nations == null || nations.Count == 0)
                return ServiceResult<object>.FailureResult("No nations found in the database.", null, 404);
            var nationDTOs = nations.Select(DTOMapper.ToNationDTO).ToList();
            return ServiceResult<object>.SuccessResult(nationDTOs, "Nations retrieved successfully.");
        }

        public async Task<ServiceResult<object>> GetNationByIdAsync(int nationId)
        {
            // Validate the input
            var validationResult = await MyValidator.ValidateNationByIdAsync(nationId, _context);
            if (!validationResult.Success) return validationResult;

            // Retrieve the nation by ID
            var nation = validationResult.Data as Nation;
            var dto = DTOMapper.ToNationDTO(nation!);
            return ServiceResult<object>.SuccessResult(dto, "Nation retrieved successfully.");
        }

        public async Task<ServiceResult<object>> GetNationByNameAsync(string nationName)
        {
            // Validate the input
            var validationResult = await MyValidator.ValidateNationByNameAsync(nationName, _context);
            if (!validationResult.Success) return validationResult;

            // Retrieve the nation by name
            var nation = validationResult.Data as Nation;
            var dto = DTOMapper.ToNationDTO(nation!);
            return ServiceResult<object>.SuccessResult(dto, "Nation retrieved successfully.");
        }

        public async Task<ServiceResult<object>> CreateNationAsync(NationCreateDTO nationDto)
        {
            // Validate the input
            var validationResult = MyValidator.AgainstNullOrEmpty(nationDto.Name, nameof(nationDto.Name));
            if (!validationResult.Succeeded)
                return ServiceResult<object>.FailureResult(validationResult.Errors.First().Description, validationResult.Errors.Select(e => e.Description));

            // Check if the nation already exists
            var existsCheck = await _context.Nations
                .AnyAsync(n => n.Name == nationDto.Name);
            if (existsCheck)
                return ServiceResult<object>.FailureResult($"Nation with name '{nationDto.Name}' already exists.", null, 409);

            // Create the new nation
            var newNation = new Nation
            {
                Name = nationDto.Name,
                CreatedAt = DateTime.UtcNow
            };

            // Add the new nation to the database
            _context.Nations.Add(newNation);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to save the new nation to the database");

            return ServiceResult<object>.SuccessResult(DTOMapper.ToNationDTO(newNation), "Nation created successfully", 201);
        }

        public async Task<ServiceResult<object>> DeleteNationAsync(int nationId)
        {
            // Validate the input
            var validationResult = await MyValidator.ValidateNationByIdAsync(nationId, _context);
            if (!validationResult.Success) return validationResult;

            // Deletion process
            var nationToDelete = validationResult.Data as Nation;
            _context.Nations.Remove(nationToDelete!);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to delete the nation from the database");

            return ServiceResult<object>.SuccessResult(null, "Nation deleted successfully", 204);
        }

        public async Task<ServiceResult<object>> UpdateNationAsync(int nationId, NationUpdateDTO nationDto)
        {
            // Validate the nationId
            var validationResult = await MyValidator.ValidateNationByIdAsync(nationId, _context);
            if (!validationResult.Success) return validationResult;

            // Name validation
            var nameValidation = MyValidator.AgainstNullOrEmpty(nationDto.Name, nameof(nationDto.Name));
            if (!nameValidation.Succeeded)
                return ServiceResult<object>.FailureResult(nameValidation.Errors.First().Description, nameValidation.Errors.Select(e => e.Description));

            // Check if the nation already exists
            var existsCheck = await _context.Nations
                .AnyAsync(n => n.Name == nationDto.Name && n.Id != nationId);
            if (existsCheck)
                return ServiceResult<object>.FailureResult($"Nation with name '{nationDto.Name}' already exists.", null, 409);

            // Update the nation
            var nationToUpdate = validationResult.Data as Nation;
            nationToUpdate!.Name = nationDto.Name;
            nationToUpdate.UpdatedAt = DateTime.UtcNow;
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to update the nation in the database");

            return ServiceResult<object>.SuccessResult(DTOMapper.ToNationDTO(nationToUpdate), "Nation updated successfully");
        }
    }
}
