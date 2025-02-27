using TextBlog.Dtos;
using TextBlog.Models;

namespace TextBlog.Repositorys
{
    public interface IUserRepository
    {
        User? GetById(Guid id); 
        User? GetByLogin(string login); 
        IEnumerable<UserDto> GetAll();
        void Add(User user);
        void Update(User user);
        void Delete(Guid id);
        bool Exists(Guid id);
        bool Exists(string login);

        void Subscribe(Guid userId, Guid authorId);
        void Unsubscribe(Guid userId, Guid authorId);
        IEnumerable<User> GetSubscribedUsers(Guid userId);
        UserDto? GetUserDtoFromToken(string token);
    }
}
