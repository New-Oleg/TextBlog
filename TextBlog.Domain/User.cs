using System.ComponentModel.DataAnnotations;

namespace TextBlog.Domain
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

        public User(Guid id, string name, string login, string passwordHash, List<Guid> subscriptions, string role)
        {
            Id = id;
            Name = name;
            Login = login;
            PasswordHash = passwordHash;
            Subscriptions = subscriptions;
            Role = role;
        }
    }

}
