using Microsoft.AspNetCore.Mvc;
using TextBlog.Dtos;
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

            var userDto = _userRepos.GetUserDtoFromToken(token); // получение себя(дто) по jwt

            return Redirect( userDto.Id + "");
        }

        [HttpGet("/CreatePost")]
        public IActionResult CreatePost()
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token) || !_authService.ValidateToken(token)) //если токен инвалид редирект на страницу логина
            {
                return Redirect("/");
            }
            return View();
        }



        [HttpGet("/FundUsers")]
        public IActionResult FindUsers(string query)
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token) || !_authService.ValidateToken(token)) //если токен инвалид редирект на страницу логина
            {
                return Redirect("/");
            }
            if (string.IsNullOrEmpty(query))
            {
                return View(_userRepos.GetAll());
            }

            var users = _userRepos.GetAll()
                          .Where(u => u.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                          .ToList();

            return View(users); 
        }




    }
}
