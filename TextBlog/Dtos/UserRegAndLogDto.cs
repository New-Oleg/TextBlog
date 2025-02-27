using System.ComponentModel.DataAnnotations;

namespace TextBlog.Dtos
{
    public class UserRegAndLogDto
    {
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(4, ErrorMessage = "Логин должен содержать минимум 4 символа")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string Password { get; set; }
    }
}
