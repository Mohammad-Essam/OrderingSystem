
using Task.Models;

namespace Task.Services
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        User User { get; }
        IEnumerable<string> Roles { get; }
    }

}
