using Microsoft.AspNetCore.Mvc;
using TextBlog.Dtos;
using TextBlog.Models;
using TextBlog.Repositorys;
using TextBlog.Services;

namespace TextBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepo;
        private readonly IUserRepository _userRepo;
        private readonly AuthService _authService;

        public PostController(IPostRepository postRepo, IUserRepository userRepo, AuthService authService)
        {
            _postRepo = postRepo;
            _userRepo = userRepo;
            _authService = authService;
        }


        [HttpPost("CretatePost")]
        public IActionResult CreatePost(PostDto request)
        {
            var token = Request.Cookies["AuthToken"];

            var post = new Post(
                Guid.NewGuid(),
                _userRepo.GetUserDtoFromToken(token).Id,
                request.Hider,
                request.Text

            );
            _postRepo.Add(post);
            return RedirectToAction("Index", "Padge");
        }
    }
}
