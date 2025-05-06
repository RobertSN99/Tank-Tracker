using API.Data;
using API.Helpers;
using API.Models.DTOs;
using API.Models.Entities;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests
{
    public class TankServiceTests
    {
        private readonly ITestOutputHelper _output;
        private readonly AppDbContext _context;
        private readonly TankService _service;

        public TankServiceTests(ITestOutputHelper output)
        {
            _output = output;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new TankService(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Nations.Add(new Nation { Id = 1, Name = "USSR" });
            _context.Statuses.Add(new Status { Id = 1, Name = "Tech Tree" });
            _context.TankClasses.Add(new TankClass { Id = 1, Name = "HT" });

            _context.Tanks.Add(new Tank
            {
                Id = 1,
                Name = "IS-7",
                Tier = 10,
                Rating = 4.5,
                NationId = 1,
                StatusId = 1,
                TankClassId = 1,
                CreatedAt = DateTime.UtcNow
            });

            _context.SaveChanges();
        }

        private void LogResult<T>(ServiceResult<T> result)
        {
            _output.WriteLine($"StatusCode: {result.StatusCode}\nMessage: {result.Message}\nData: {result.Data}");
        }

        [Fact]
        public async Task GetAllTanksAsync_Returns_Tanks()
        {
            var result = await _service.GetAllTanksAsync(1, 10, null, null, null, null, null, null, null, null);
            LogResult(result);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetAllTanksAsync_ValidationError_When_PageSize_Zero()
        {
            var result = await _service.GetAllTanksAsync(1, 0, null, null, null, null, null, null, null, null);

            Assert.False(result.Success);
            Assert.Equal(400, result.StatusCode);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetTankByIdAsync_Returns_Valid_Tank()
        {
            var result = await _service.GetTankByIdAsync(1);

            Assert.True(result.Success);
            Assert.IsType<TankDTO>(result.Data);
            var tank = result.Data as TankDTO;
            Assert.Equal("IS-7", tank.Name);
        }

        [Fact]
        public async Task GetTankByIdAsync_Returns_NotFound()
        {
            var result = await _service.GetTankByIdAsync(999);

            Assert.False(result.Success);
            Assert.Contains("not exist", result.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task CreateTankAsync_Succeeds_With_Valid_Data()
        {
            var dto = new TankCreateDTO
            {
                Name = "T-62A",
                Tier = 10,
                Rating = 4,
                NationId = 1,
                StatusId = 1,
                TankClassId = 1
            };

            var result = await _service.CreateTankAsync(dto);

            Assert.True(result.Success);
            Assert.Equal(201, result.StatusCode);
            Assert.IsType<TankDTO>(result.Data);
        }

        [Fact]
        public async Task CreateTankAsync_Fails_When_Name_Exists()
        {
            var dto = new TankCreateDTO
            {
                Name = "IS-7",
                Tier = 10,
                Rating = 4,
                NationId = 1,
                StatusId = 1,
                TankClassId = 1
            };

            var result = await _service.CreateTankAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(409, result.StatusCode);
        }

        [Fact]
        public async Task UpdateTankAsync_Succeeds()
        {
            var updateDto = new TankUpdateDTO
            {
                Name = "IS-4",
                Rating = 4.5,
                Tier = 10
            };

            var result = await _service.UpdateTankAsync(1, updateDto);

            Assert.True(result.Success);
            Assert.IsType<TankDTO>(result.Data);
        }

        [Fact]
        public async Task UpdateTankAsync_Fails_When_Name_Exists()
        {
            await _service.CreateTankAsync(new TankCreateDTO
            {
                Name = "Object 140",
                Tier = 10,
                Rating = 4,
                NationId = 1,
                StatusId = 1,
                TankClassId = 1
            });

            var result = await _service.UpdateTankAsync(1, new TankUpdateDTO { Name = "Object 140" });

            Assert.False(result.Success);
            Assert.Equal(409, result.StatusCode);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task UpdateTankAsync_Fails_When_NotFound()
        {
            var result = await _service.UpdateTankAsync(999, new TankUpdateDTO { Name = "Nonexistent" });

            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteTankAsync_Succeeds()
        {
            var deleteResult = await _service.DeleteTankAsync(1);

            Assert.True(deleteResult.Success);
            Assert.Equal(204, deleteResult.StatusCode);
            Assert.Null(deleteResult.Data);

            var checkResult = await _service.GetTankByIdAsync(1);
            Assert.False(checkResult.Success);
            Assert.Equal(404, checkResult.StatusCode);
        }

        [Fact]
        public async Task DeleteTankAsync_Fails_When_NotFound()
        {
            var result = await _service.DeleteTankAsync(999);

            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
        }

        [Theory]
        [InlineData("USSR", null, null)]
        [InlineData(null, "Tech Tree", null)]
        [InlineData(null, null, "IS")]
        public async Task GetAllTanksAsync_Filters_Work(string nation, string status, string searchTerm)
        {
            var nations = nation is null ? null : new List<string> { nation };
            var statuses = status is null ? null : new List<string> { status };

            var result = await _service.GetAllTanksAsync(1, 10, nations, statuses, null, null, null, searchTerm, null, null);
            LogResult(result);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Theory]
        [InlineData("BadSort", "asc")]
        [InlineData("Nation", "badOrder")]
        public async Task GetAllTanksAsync_Fails_With_Invalid_Sort(string sortBy, string sortOrder)
        {
            var result = await _service.GetAllTanksAsync(1, 10, null, null, null, null, null, null, sortBy, sortOrder);

            Assert.False(result.Success);
            Assert.Equal(400, result.StatusCode);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetAllTanksAsync_Sorts_By_Name_Asc()
        {
            await _service.CreateTankAsync(new TankCreateDTO
            {
                Name = "A-20",
                Tier = 5,
                Rating = 2,
                NationId = 1,
                StatusId = 1,
                TankClassId = 1
            });

            var result = await _service.GetAllTanksAsync(1, 10, null, null, null, null, null, null, "Name", "asc");
            LogResult(result);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(null, 6)]
        public async Task GetAllTanksAsync_Fails_With_Invalid_Filter(int? tier, int? rating)
        {
            var tiers = tier.HasValue ? new List<int> { tier.Value } : null;
            var ratings = rating.HasValue ? new List<double> { rating.Value } : null;

            var result = await _service.GetAllTanksAsync(1, 10, null, null, null, tiers, ratings, null, null, null);

            Assert.False(result.Success);
            Assert.Equal(400, result.StatusCode);
            Assert.Null(result.Data);
        }
    }
}
