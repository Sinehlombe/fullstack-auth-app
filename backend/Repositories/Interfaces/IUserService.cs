using backend.DTOs;

namespace backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserByIdAsync(int id);
    }
}