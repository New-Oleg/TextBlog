using TextBlog.Dtos;
using TextBlog.Models;

namespace TextBlog.Repositorys
{
    public interface IUserRepository
    {
        User? GetById(Guid id);
        Task<User?> GetByIdAsync(Guid id);
        User? GetByLogin(string login); 
        IEnumerable<UserDto> GetAll();
        void Add(User user);
        void Delete(Guid id);
        bool Exists(Guid id);
        bool Exists(string login);
        Task Update(User user);

        IEnumerable<UserDto> GetSubscribedUsers(Guid userId);
        UserDto? GetUserDtoFromToken(string token);
    }
}
