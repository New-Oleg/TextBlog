using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TextBlog.Dtos;
using TextBlog.Models;
using TextBlog.Repositorys;
using TextBlog.Services;
using TextBlog.SignalRHub;

namespace TextBlog.Controllers
{

    public class UserController : Controller
    {


        private readonly IPostRepository _postRepo;
        private readonly IUserRepository _userRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IHubContext<CommentHub> _hubContext;
        private readonly IUserService _userService;
        private readonly AuthService _authService;

        public UserController(IPostRepository postRepo, IUserRepository userRepo, AuthService authService,
            ICommentRepository commentRepo, IHubContext<CommentHub> hubContext, IUserService userService)
        {
            _postRepo = postRepo;
            _userRepo = userRepo;
            _authService = authService;
            _commentRepo = commentRepo;
            _hubContext = hubContext;
            _userService = userService;
        }


        [HttpGet("/{userId}")]
        public IActionResult Users(Guid userId)
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token) || !_authService.ValidateToken(token)) //если токен инвалид редирект на страницу логина
            {
                return RedirectToAction("Login", "Padge");
            }

            var userDto = _userRepo.GetById(userId); // получение чела(дто) по ID
            if (userDto == null) // проверка на существование чела
            {
                return RedirectToAction("MyProfile", "User");// если его не существует идем домой
            }

            var postDto = _postRepo.GetByAuthor(userDto.Id); // если есть посты отоброзить их
            if (postDto != null)
            {
                ViewData["Posts"] = postDto;
            }

            var meId = _authService.GetUserIdFromToken(token);
            ViewData["meId"] = meId; // Передаем в представление

            return View("Index", userDto.ParsToDto());
        }

        [HttpGet("/subscribes")]
        public IActionResult SubList(Guid userId)
        {
            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token) || !_authService.ValidateToken(token)) //если токен инвалид редирект на страницу логина
            {
                return RedirectToAction("Login", "Padge");
            }
            UserDto user = _userRepo.GetUserDtoFromToken(token);

            var subscribes = _userRepo.GetSubscribedUsers(user.Id);
            ViewData["subscribes"] = subscribes;

            return View(user);
        }

        [HttpPost("/Sub")]
        public async Task<IActionResult> Sub(Guid targetUserId)
        {
            var token = Request.Cookies["AuthToken"];

            var userDto = _userRepo.GetUserDtoFromToken(token);

            await _userService.SubscribeAsync(userDto.Id, targetUserId);
            return Ok(new { message = "Подписка успешна" });

        }

        [HttpPost("/Unsub")]
        public async Task<IActionResult> Unsub(Guid targetUserId)
        {

            var token = Request.Cookies["AuthToken"];

            var userDto = _userRepo.GetUserDtoFromToken(token);

            await _userService.UnsubscribeAsync(userDto.Id, targetUserId);
            return Ok(new { message = "Отписка успешна" });
        }


        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken"); // Удаляем куку с токеном
            return RedirectToAction("Login", "Padge"); // Редирект на страницу входа
        }


    }
}
