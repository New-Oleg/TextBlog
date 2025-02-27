using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TextBlog.Dtos;
using TextBlog.Models;
using TextBlog.Repositorys;
using TextBlog.Services;

namespace TextBlog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly AuthService _authService;

        public AuthController(IUserRepository userRepo, AuthService authService)
        {
            _userRepo = userRepo;
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegAndLogDto request)
        {
            if (_userRepo.Exists(request.Login))
                return BadRequest("Пользователь с таким логином уже существует");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = request.Login,
                PasswordHash = HashPassword(request.Password),
                Role = "User"
            };

            _userRepo.Add(user);
            return Ok(new { message = "Пользователь зарегистрирован" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserRegAndLogDto request)
        {
            var user = _userRepo.GetByLogin(request.Login);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Неверный логин или пароль");

            var token = _authService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return HashPassword(inputPassword) == storedHash;
        }
    }
}
