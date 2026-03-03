using Moq;
using Microsoft.Extensions.Configuration;
using backend.DTOs;
using backend.Models;
using backend.Repositories.Interfaces;
using backend.Services;

namespace backend.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockConfig = new Mock<IConfiguration>();

            _mockConfig.Setup(c => c["Jwt:Key"])
                .Returns("test-super-secret-key-that-is-long-enough");
            _mockConfig.Setup(c => c["Jwt:Issuer"])
                .Returns("test-issuer");
            _mockConfig.Setup(c => c["Jwt:Audience"])
                .Returns("test-audience");

            _authService = new AuthService(_mockRepo.Object, _mockConfig.Object);
        }

        [Fact]
        public async Task Register_ShouldSucceed_WhenEmailIsNew()
        {
            _mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(new User { Id = 1, Email = "onwaba@gmail.com" });

            var request = new RegisterRequestDto
            {
                FirstName = "Onwaba",
                LastName = "Nobhitywana",
                Email = "onwaba@gmail.com",
                Password = "password123"
            };

            var result = await _authService.RegisterAsync(request);

            Assert.Equal("Registration successful.", result);
        }

        [Fact]
        public async Task Register_ShouldFail_WhenEmailAlreadyExists()
        {
            _mockRepo.Setup(r => r.EmailExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var request = new RegisterRequestDto
            {
                FirstName = "Onwaba",
                LastName = "Nobhitywana",
                Email = "onwaba@gmail.com",
                Password = "password123"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _authService.RegisterAsync(request)
            );
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
            _mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User
                {
                    Id = 1,
                    Email = "onwaba@gmail.com",
                    PasswordHash = hashedPassword
                });

            var request = new LoginRequestDto
            {
                Email = "onwaba@gmail.com",
                Password = "password123"
            };

            var token = await _authService.LoginAsync(request);

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public async Task Login_ShouldFail_WhenPasswordIsWrong()
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
            _mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User
                {
                    Id = 1,
                    Email = "onwaba@gmail.com",
                    PasswordHash = hashedPassword
                });

            var request = new LoginRequestDto
            {
                Email = "onwaba@gmail.com",
                Password = "wrongpassword"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.LoginAsync(request)
            );
        }

        [Fact]
        public async Task Login_ShouldFail_WhenUserDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            var request = new LoginRequestDto
            {
                Email = "nobody@test.com",
                Password = "password123"
            };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.LoginAsync(request)
            );
        }
    }
}