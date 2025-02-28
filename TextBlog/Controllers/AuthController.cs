using Microsoft.AspNetCore.Mvc;
using TextBlog.Dtos;
using TextBlog.Models;
using TextBlog.Repositorys;
using TextBlog.Services;

namespace TextBlog.Controllers
{
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
            return Redirect("/");
        }

        [HttpPost("Login")]
        public IActionResult Login(UserRegAndLogDto request)
        {
            var user = _userRepo.GetByLogin(request.Login);
            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Неверный логин или пароль");

            var token = _authService.GenerateToken(user);

            // Сохранение токена в куки 
            Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true, Secure = true });

            // Генерация refresh токена и сохранение его в куки
            var refreshToken = _authService.GenerateRefreshToken();
            _authService.StoreRefreshToken(user.Id, refreshToken);
            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions { HttpOnly = true, Secure = true });

            // Редирект на Index в контроллере Padge
            var userDto = _userRepo.GetUserDtoFromToken(token); // получение себя(дто) по jwt

            return Redirect(userDto.Id + "");
        }

        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken(string refreshToken)
        {
            // Получаем старый access token из куки
            var currentToken = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(currentToken) || string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Токен не найден");

            var userId = _authService.GetUserIdFromToken(currentToken);
            if (!userId.HasValue || !_authService.ValidateRefreshToken(userId.Value, refreshToken))
                return Unauthorized("Неверный refresh токен");

            // Обновляем токены
            var (newAccessToken, newRefreshToken) = _authService.RefreshToken(currentToken, refreshToken) ?? (null, null);
            if (newAccessToken == null)
                return Unauthorized("Не удалось обновить токены");

            // Сохраняем новые токены в куки
            Response.Cookies.Append("AuthToken", newAccessToken, new CookieOptions { HttpOnly = true, Secure = true });
            Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions { HttpOnly = true, Secure = true });

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }
    }
}