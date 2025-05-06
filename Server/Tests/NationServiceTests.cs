using API.Data;
using API.Helpers;
using API.Models.DTOs;
using API.Models.Entities;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests
{
    public class NationServiceTests
    {
        private readonly ITestOutputHelper _output;
        private readonly AppDbContext _context;
        private readonly NationService _service;

        public NationServiceTests(ITestOutputHelper output)
        {
            _output = output;

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new NationService(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.Nations.Add(new Nation
            {
                Id = 1,
                Name = "Germany",
                CreatedAt = DateTime.UtcNow
            });

            _context.SaveChanges();
        }

        private void LogResult<T>(ServiceResult<T> result)
        {
            _output.WriteLine($"StatusCode: {result.StatusCode}\nMessage: {result.Message}\nData: {result.Data}");
        }

        [Fact]
        public async Task GetAllNationsAsync_Returns_Data()
        {
            var result = await _service.GetAllNationsAsync();
            LogResult(result);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetNationByIdAsync_Returns_Nation()
        {
            var result = await _service.GetNationByIdAsync(1);
            LogResult(result);

            Assert.True(result.Success);
            Assert.IsType<NationDTO>(result.Data);
        }

        [Fact]
        public async Task GetNationByIdAsync_Fails_When_NotFound()
        {
            var result = await _service.GetNationByIdAsync(999);
            LogResult(result);

            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetNationByNameAsync_Returns_Nation()
        {
            var result = await _service.GetNationByNameAsync("Germany");
            LogResult(result);

            Assert.True(result.Success);
            Assert.IsType<NationDTO>(result.Data);
        }

        [Fact]
        public async Task GetNationByNameAsync_Fails_When_NotFound()
        {
            var result = await _service.GetNationByNameAsync("Atlantis");
            LogResult(result);

            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task CreateNationAsync_Succeeds()
        {
            var dto = new NationCreateDTO { Name = "France" };

            var result = await _service.CreateNationAsync(dto);
            LogResult(result);

            Assert.True(result.Success);
            Assert.Equal(201, result.StatusCode);
            Assert.IsType<NationDTO>(result.Data);
        }

        [Fact]
        public async Task CreateNationAsync_Fails_When_Name_Exists()
        {
            var dto = new NationCreateDTO { Name = "Germany" };

            var result = await _service.CreateNationAsync(dto);
            LogResult(result);

            Assert.False(result.Success);
            Assert.Equal(409, result.StatusCode);
        }

        [Fact]
        public async Task CreateNationAsync_Fails_When_Name_Null()
        {
            var dto = new NationCreateDTO { Name = null! };

            var result = await _service.CreateNationAsync(dto);
            LogResult(result);

            Assert.False(result.Success);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task UpdateNationAsync_Succeeds()
        {
            var updateDto = new NationUpdateDTO { Name = "West Germany" };

            var result = await _service.UpdateNationAsync(1, updateDto);
            LogResult(result);

            Assert.True(result.Success);
            Assert.IsType<NationDTO>(result.Data);
        }

        [Fact]
        public async Task UpdateNationAsync_Fails_When_NotFound()
        {
            var dto = new NationUpdateDTO { Name = "Nonexistent" };

            var result = await _service.UpdateNationAsync(999, dto);
            LogResult(result);

            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task UpdateNationAsync_Fails_When_Name_Exists()
        {
            // Create second nation
            await _service.CreateNationAsync(new NationCreateDTO { Name = "France" });

            // Try renaming Germany to France
            var result = await _service.UpdateNationAsync(1, new NationUpdateDTO { Name = "France" });
            LogResult(result);

            Assert.False(result.Success);
            Assert.Equal(409, result.StatusCode);
        }

        [Fact]
        public async Task DeleteNationAsync_Succeeds()
        {
            var result = await _service.DeleteNationAsync(1);
            LogResult(result);

            Assert.True(result.Success);
            Assert.Equal(204, result.StatusCode);

            var check = await _service.GetNationByIdAsync(1);
            Assert.False(check.Success);
            Assert.Equal(404, check.StatusCode);
        }

        [Fact]
        public async Task DeleteNationAsync_Fails_When_NotFound()
        {
            var result = await _service.DeleteNationAsync(999);
            LogResult(result);

            Assert.False(result.Success);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
