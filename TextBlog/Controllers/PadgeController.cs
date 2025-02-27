using Microsoft.AspNetCore.Mvc;
using TextBlog.Models;
using TextBlog.Repositorys;
using TextBlog.Services;

namespace TextBlog.Controllers
{

    [Route("[controller]")]
    public class PadgeController : Controller
    {
        private readonly AuthService _authService;

        private readonly IUserRepository _userRepos;
        private readonly IPostRepository _postRepos;

        public PadgeController(AuthService authService, IUserRepository userRepos, IPostRepository postRepos)
        {
            _authService = authService;
            _userRepos = userRepos;
            _postRepos = postRepos;
        }


        [HttpGet("/Register")]
        public IActionResult Register()
        {
            ViewData["HideHeader"] = true;
            return View();
        }

        [HttpGet("/")]
        public IActionResult Login()
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token) || !_authService.ValidateToken(token)) //если токен не инвалид редирект на главную страницу 
            {
            ViewData["HideHeader"] = true;
            return View();
            }

            return RedirectToAction("Index", "Padge");
        }

        [HttpGet("/Home")] 
        public IActionResult Index()
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token) || !_authService.ValidateToken(token)) //если токен инвалид редирект на страницу логина
            {
                return RedirectToAction("Login", "Padge");
            }

            var userDto = _userRepos.GetUserDtoFromToken(token); // получение себя(дто) по jwt
            if (userDto == null) // проверка на существование себя
            {
                return Unauthorized(new { message = "Пользователь не найден" });
            }

            var postDto = _postRepos.GetByAuthor(userDto.Id);
            if (postDto != null)
            {
                ViewData["Posts"] = postDto;
            }

            return View(userDto); 
        }

        [HttpGet("/CreatePost")]
        public IActionResult CreatePost()
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token) || !_authService.ValidateToken(token)) //если токен инвалид редирект на страницу логина
            {
                return RedirectToAction("Login", "Padge");
            }
            return View();
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken"); // Удаляем куку с токеном
            return RedirectToAction("Login", "Padge"); // Редирект на страницу входа
        }
    }
}
