using System.IdentityModel.Tokens.Jwt;
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

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return _context.Users.Local.FirstOrDefault(u => u.Id == id)
                ?? await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }


        public User? GetByLogin(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public IEnumerable<UserDto> GetAll()
        {
            try
            {
                return _context.Users.ToList().Select(user => user.ParsToDto());
            }
            catch
            {
                return new List<UserDto>();
            }
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


        public void Delete(Guid id)
        {
            var userToRemove = _context.Users.Find(id);
            if (userToRemove != null)
            {
                _context.Users.Remove(userToRemove); 
                _context.SaveChanges();             
            }
        }
        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public bool Exists(Guid id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public bool Exists(string login)
        {
            return _context.Users.Any(u => u.Login == login);
        }


        public IEnumerable<UserDto> GetSubscribedUsers(Guid userId)
        {
            var user = GetById(userId);
            if (user == null)
                throw new ArgumentException($"User with ID '{userId}' not found.");

            return _context.Users
                .Where(u => user.Subscriptions.Contains(u.Id))
                .Select(u => u.ParsToDto()) // Применяем ParsToDto() для каждого пользователя
                .ToList();
        }

        public UserDto? GetUserDtoFromToken(string token)
        {
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null) return null;

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                    return null;
                User u = GetById(Guid.Parse(userIdClaim));

                if (u == null) return null;

                return u.ParsToDto();
            }
            catch
            {
                return null;
            }
        }

    }
}
