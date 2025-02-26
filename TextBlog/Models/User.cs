using System.ComponentModel.DataAnnotations;

namespace TextBlog.Models
{
    public class User
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Login { get; set; }

        [Required]
        public string PasswordHash { get; set; } // Хранение пароля в хешированном виде

        public List<Guid> Subscriptions { get; set; } = new List<Guid>(); // ID подписок

        [Required]
        public string Role { get; set; } // Роль пользователя


        public User(Guid id, string name, string login, string password, string role)
        {
            Id = id;
            Name = name;
            Login = login;
            PasswordHash = HashPassword(password); // Хешируем пароль при создании пользователя
            Subscriptions = new List<Guid>();
            Role = role;
        }

        public User() { }

        // Метод для хеширования пароля
        public void SetPassword(string password)
        {
            PasswordHash = HashPassword(password);
        }

        // Метод для проверки пароля
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        // Метод для создания хеша пароля
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }

}
