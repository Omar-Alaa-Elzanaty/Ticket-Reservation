using Core.Models;

namespace Core.Ports
{
    public interface IAuthServices
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
