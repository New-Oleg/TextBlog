﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TextBlog.Dtos;
using TextBlog.Models;
using TextBlog.Repositorys;
using TextBlog.Services;
using TextBlog.SignalRHub;

namespace TextBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepo;
        private readonly IUserRepository _userRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IHubContext<CommentHub> _hubContext;
        private readonly AuthService _authService;

        public PostController(IPostRepository postRepo, IUserRepository userRepo, AuthService authService,
            ICommentRepository commentRepo, IHubContext<CommentHub> hubContext)
        {
            _postRepo = postRepo;
            _userRepo = userRepo;
            _authService = authService;
            _commentRepo = commentRepo;
            _hubContext = hubContext;
        }


        [HttpPost("CretatePost")]
        public IActionResult CreatePost(PostDto request)
        {
            var token = Request.Cookies["AuthToken"];

            var post = new Post(
                Guid.NewGuid(),
                _userRepo.GetUserDtoFromToken(token).Id,
                request.Text,
                request.Hider

            );

            _postRepo.Add(post);

            var userDto = _userRepo.GetUserDtoFromToken(token); // получение себя(дто) по jwt
            return Redirect(userDto.Id + "");
        }

        [HttpGet("Post/Details/{postId}")]
        public IActionResult Details(Guid postId) {

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token) || !_authService.ValidateToken(token)) //если токен инвалид редирект на страницу логина
            {
                return Redirect("/");
            }

            var commentsDto = _commentRepo.GetByPostId(postId); // если есть коментарии отоброзить их
            if (commentsDto != null)
            {
                ViewData["comments"] = commentsDto;
            }

            return View(_postRepo.GetById(postId).ParsToDto());   
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Guid postId, string commentText)
        {

            var token = Request.Cookies["AuthToken"];

            if (string.IsNullOrWhiteSpace(commentText))
                return BadRequest("Комментарий не может быть пустым.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userDto = _userRepo.GetUserDtoFromToken(token);

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = postId,
                AuthorId = userDto.Id,
                Text = commentText,
                PublishTime = DateTime.UtcNow
            };

            _commentRepo.Add(comment);

            // Отправка сообщения через SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveComment", postId.ToString(), userDto.Id.ToString(), comment.Text);

            return RedirectToAction("Details", "Post", new { postId });
        }
    }
}
