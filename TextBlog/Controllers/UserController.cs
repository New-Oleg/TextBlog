using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TextBlog.Dtos;
using TextBlog.Repositorys;

namespace TextBlog.Controllers
{
    [Route("User/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<List<UserDto>> Users()
        {
                var users = _userRepository.GetAll();
                // Преобразуйте список User в список UserDto
                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name
                }).ToList();

                return userDtos; 
            
        }
    }
}
