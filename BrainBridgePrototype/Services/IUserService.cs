using BrainBridgePrototype.DTOs;

namespace BrainBridgePrototype.Services
{
    public interface IUserService
    {
        Task Register(UserDto userDto, string password);
        Task<string> Login(string username, string password);
        Task<UserDto> GetUserById(string userId);
    }
}
