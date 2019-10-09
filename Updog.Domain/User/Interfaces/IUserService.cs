using System.Threading.Tasks;

namespace Updog.Domain {
    public interface IUserService : IService<User> {
        Task<User> AdminRegisterOrUpdate(IAdminConfig config);
        Task<UserLogin?> Login(UserCredentials credentials);
        Task<UserLogin?> Register(UserRegistration registration);
        Task<User> Update(UserUpdateData data, User user);
        Task<User> UpdatePassword(UserUpdatePasswordData data, User user);
    }
}