﻿using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Helpers;
using API.Models.DTOs;
using API.Models.Entities;
using API.Services.Interfaces;

namespace API.Services
{
    public class StatusService : IStatusService
    {
        private readonly AppDbContext _context;

        public StatusService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResult<object>> CreateStatusAsync(StatusCreateDTO statusDto)
        {
            // Validate the input
            var validationResult = MyValidator.AgainstNullOrEmpty(statusDto.Name, nameof(statusDto.Name));
            if (!validationResult.Succeeded)
                return ServiceResult<object>.FailureResult(validationResult.Errors.First().Description, validationResult.Errors.Select(e => e.Description));

            // Check if the status already exists
            var existsCheck = await _context.Statuses
                .AnyAsync(s => s.Name == statusDto.Name);
            if (existsCheck)
                return ServiceResult<object>.FailureResult($"Status with name '{statusDto.Name}' already exists.", null, 409);

            // Create the new status
            var newStatus = new Status
            {
                Name = statusDto.Name,
                CreatedAt = DateTime.UtcNow
            };

            // Add the new status to the database
            _context.Statuses.Add(newStatus);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to save the new status to the database");

            return ServiceResult<object>.SuccessResult(DTOMapper.ToStatusDTO(newStatus), "Status created successfully", 201);
        }

        public async Task<ServiceResult<object>> DeleteStatusAsync(int statusId)
        {
            // Validate the input
            var validationResult = await MyValidator.ValidateStatusByIdAsync(statusId, _context);
            if (validationResult != null) return validationResult;

            // Deletion process
            var statusToDelete = validationResult!.Data as Status;
            _context.Statuses.Remove(statusToDelete!);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to delete the status from the database");
            return ServiceResult<object>.SuccessResult(null, "Status deleted successfully", 204);
        }

        public async Task<ServiceResult<object>> GetStatusByIdAsync(int statusId)
        {
            // Validate the input
            var validationResult = await MyValidator.ValidateStatusByIdAsync(statusId, _context);
            if (!validationResult.Success) return validationResult;

            // Get and return the status
            var status = validationResult.Data as Status;
            var dto = DTOMapper.ToStatusDTO(status!);
            return ServiceResult<object>.SuccessResult(dto, "Status retrieved successfully");
        }

        public async Task<ServiceResult<object>> GetAllStatusesAsync()
        {
            // Retrieve all statuses from the database
            var statuses = await _context.Statuses.Include(s => s.Tanks).ToListAsync();
            if (statuses == null || statuses.Count == 0)
                return ServiceResult<object>.FailureResult("No statuses found in the database", null, 404);

            var statusesDTO = statuses.Select(DTOMapper.ToStatusDTO).ToList();
            return ServiceResult<object>.SuccessResult(statusesDTO, "Statuses retrieved successfully");
        }

        public async Task<ServiceResult<object>> UpdateStatusAsync(int statusId, StatusUpdateDTO statusDto)
        {
            // Validate the input
            var validationResult = await MyValidator.ValidateStatusByIdAsync(statusId, _context);
            if (!validationResult.Success) return validationResult;

            // Name validation
            var nameValidation = MyValidator.AgainstNullOrEmpty(statusDto.Name, nameof(statusDto.Name));
            if (!nameValidation.Succeeded)
                return ServiceResult<object>.FailureResult(nameValidation.Errors.First().Description, nameValidation.Errors.Select(e => e.Description));

            // Check if the status already exists
            var existsCheck = await _context.Statuses
                .AnyAsync(s => s.Name == statusDto.Name && s.Id != statusId);
            if (existsCheck)
                return ServiceResult<object>.FailureResult($"Status with name '{statusDto.Name}' already exists.");

            // Update the status
            var statusToUpdate = validationResult.Data as Status;
            statusToUpdate!.Name = statusDto.Name;
            statusToUpdate.UpdatedAt = DateTime.UtcNow;
            _context.Statuses.Update(statusToUpdate);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return ServiceResult<object>.FailureResult("Failed to update the status in the database");
            return ServiceResult<object>.SuccessResult(statusToUpdate, "Status updated successfully");
        }
    }
}
