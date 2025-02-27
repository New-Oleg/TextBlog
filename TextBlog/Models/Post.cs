using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBlog.Dtos;

namespace TextBlog.Models
{
    public class Post 
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AuthorId { get; set; } // ID автора

        [Required]
        public string Hider { get; set; }

        [Required] 
        public string Text { get; set; }

        [Required]
        public DateTime PublishTime { get; set; } // Время публикации

        public int Likes { get; set; } // Количество лайков

        public int Dislikes { get; set; } // Количество дизлайков

        public Post(Guid id, Guid authorId, string text, string hider)
        {
            Id = id;
            AuthorId = authorId;
            Text = text;
            Hider = hider;

            PublishTime = DateTime.UtcNow;
            Likes = 0;
            Dislikes = 0;
        }

        public Post() { }

        public PostDto ParsToDto() 
        { 
            return new PostDto{ Id=Id, AuthorId = AuthorId, Hider = Hider,
                Text = Text, PublishTime  = PublishTime, Likes  = Likes, Dislikes = Dislikes }; 
        }

    }
}
