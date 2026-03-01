using Moq;
using backend.Models;
using backend.Repositories.Interfaces;
using backend.Services;

namespace backend.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _userService = new UserService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@test.com"
                });

            // Act
            var result = await _userService.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("john@test.com", result.Email);
        }

        [Fact]
        public async Task GetUserById_ShouldThrow_WhenUserDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _userService.GetUserByIdAsync(999)
            );
        }
    }
}