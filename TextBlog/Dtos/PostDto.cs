using System.ComponentModel.DataAnnotations;

namespace TextBlog.Dtos
{
    public class PostDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid AuthorId { get; set; } // ID автора

        public string Text { get; set; }

        public DateTime PublishTime { get; set; } // Время публикации

        public int Likes { get; set; } // Количество лайков

        public int Dislikes { get; set; } // Количество дизлайков
    }
}
