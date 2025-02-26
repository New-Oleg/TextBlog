using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBlog.Domain
{
    internal class Post
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AuthorId { get; set; } // ID автора

        [Required] 
        public string Text { get; set; }

        [Required]
        public DateTime PublishTime { get; set; }; // Время публикации

        public int Likes { get; set; } // Количество лайков

        public int Dislikes { get; set; } // Количество дизлайков

        public Post(Guid id, Guid authorId, string text)
        {
            Id = id;
            AuthorId = authorId;
            Text = text;

            PublishTime = DateTime.UtcNow;
            Likes = 0;
            Dislikes = 0;

        }

    }
}
