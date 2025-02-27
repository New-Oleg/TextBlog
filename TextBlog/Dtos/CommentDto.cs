using System.ComponentModel.DataAnnotations;

namespace TextBlog.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid AuthorId { get; set; } // ID автора

        public Guid PostId { get; set; } // ID поста

        public string Text { get; set; } // Текст комментария

        public DateTime PublishTime { get; set; } // Время публикации комментария
    }
}
