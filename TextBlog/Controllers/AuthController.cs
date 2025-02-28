using Microsoft.AspNetCore.Mvc;
using TextBlog.Dtos;
using TextBlog.Models;
using TextBlog.Repositorys;
using TextBlog.Services;

namespace TextBlog.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly AuthService _authService;

        public AuthController(IUserRepository userRepo, AuthService authService)
        {
            _userRepo = userRepo;
            _authService = authService;
        }

        [HttpPost("Register")]
        public IActionResult Register(UserRegAndLogDto request)
        {
            if (_userRepo.Exists(request.Login))
                return Json(new { success = false, message = "Пользователь с таким логином уже существует" });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Login = request.Login,
                PasswordHash = _authService.HashPassword(request.Password),
            };
            _userRepo.Add(user);
            return RedirectToAction("Login", "Padge");
        }

        [HttpPost("login")]
        public IActionResult Login(UserRegAndLogDto request)
        {
            var user = _userRepo.GetByLogin(request.Login);
            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Неверный логин или пароль");

            var token = _authService.GenerateToken(user);

            // Сохранение токена в куки 
            Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true });

            // Редирект на Index в контроллере Padge
            var userDto = _userRepo.GetUserDtoFromToken(token); // получение себя(дто) по jwt

            return Redirect(userDto.Id + "");
        }


    }
}
