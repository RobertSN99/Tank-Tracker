using API.Data;
using API.Models.DTOs;
using API.Models.Entities;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;

namespace Tests
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly AppDbContext _context;
        private readonly Mock<IConfiguration> _configurationMock;

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _userManagerMock = MockUserManager();
            _signInManagerMock = MockSignInManager(_userManagerMock.Object);
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["SessionOptions:SessionDurationMinutes"]).Returns("30");

            _authService = new AuthService(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _httpContextAccessorMock.Object,
                _context,
                _configurationMock.Object
            );
        }

        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<SignInManager<User>> MockSignInManager(UserManager<User> userManager)
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            return new Mock<SignInManager<User>>(userManager, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
        }

        [Fact]
        public async Task RegisterAsync_Succeeds_With_Valid_Data()
        {
            var dto = new RegisterDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "P@ssw0rd"
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _authService.RegisterAsync(dto);

            Assert.True(result.Success);
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task LoginAsync_Fails_When_User_Not_Found()
        {
            var dto = new LoginDTO
            {
                Email = "notfound@example.com",
                Password = "irrelevant"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync((User)null);

            var result = await _authService.LoginAsync(dto);

            Assert.False(result.Success);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task LogoutAsync_Succeeds_When_User_Is_Authenticated()
        {
            var userId = Guid.NewGuid().ToString();
            var user = new User { Id = userId };

            var claims = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, userId)
            ]));

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = claims });
            await _context.UserSessions.AddAsync(new UserSession
            {
                UserId = userId,
                LoginTime = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            _signInManagerMock.Setup(x => x.SignOutAsync()).Returns(Task.CompletedTask);

            var result = await _authService.LogoutAsync();

            Assert.True(result.Success);
            Assert.Equal(204, result.StatusCode);
        }
    }
}
