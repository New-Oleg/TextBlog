using Microsoft.EntityFrameworkCore;
using TextBlog.Data;
using TextBlog.Dtos;
using TextBlog.Models;

namespace TextBlog.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User? GetById(Guid id)
        {

            return _context.Users.Local.FirstOrDefault(u => u.Id == id)
                ?? _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch { return new List<User>(); }
        }

        public void Add(User user)
        {
            if (Exists(user.Login))
            {
                throw new ArgumentException($"User with login '{user.Login}' already exists.");
            }

            _context.Users.Add(user);  
            _context.SaveChanges();   
        }

        public void Update(User user)
        {
            if (!_context.Users.Any(u => u.Id == user.Id))
            {
                throw new ArgumentException($"User with ID '{user.Id}' not found.");
            }

            _context.Users.Update(user);
            _context.SaveChanges();    
        }

        public void Delete(Guid id)
        {
            var userToRemove = _context.Users.Find(id);
            if (userToRemove != null)
            {
                _context.Users.Remove(userToRemove); 
                _context.SaveChanges();             
            }
        }

        public bool Exists(Guid id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public bool Exists(string login)
        {
            return _context.Users.Any(u => u.Login == login);
        }

        public void Subscribe(Guid userId, Guid authorId)
        {
            var user = GetById(userId);
            if (user == null) throw new ArgumentException($"User with ID '{userId}' not found.");

            if (!user.Subscriptions.Contains(authorId))
            {
                user.Subscriptions.Add(authorId);
                _context.SaveChanges(); 
            }
        }

        public void Unsubscribe(Guid userId, Guid authorId)
        {
            var user = GetById(userId);
            if (user == null) throw new ArgumentException($"User with ID '{userId}' not found.");

            user.Subscriptions.Remove(authorId);
            _context.SaveChanges(); 
        }

        public IEnumerable<User> GetSubscribedUsers(Guid userId)
        {
            var user = GetById(userId);
            if (user == null) throw new ArgumentException($"User with ID '{userId}' not found.");

            return _context.Users.Where(u => user.Subscriptions.Contains(u.Id)).ToList();
        }
    }
}
