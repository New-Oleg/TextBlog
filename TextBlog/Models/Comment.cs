﻿using System.ComponentModel.DataAnnotations;
using TextBlog.Dtos;

namespace TextBlog.Models
{
    public class Comment
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AuthorId { get; set; } // ID автора

        [Required]
        public Guid PostId { get; set; } // ID поста

        [Required]
        [MaxLength(100)]
        public string Text { get; set; } // Текст комментария

        [Required]
        public DateTime PublishTime { get; set; } // Время публикации комментария

        public Comment(Guid id, Guid authorId, Guid postId, string text)
        {
            Id = id;
            AuthorId = authorId;
            PostId = postId;
            Text = text;

            PublishTime = DateTime.UtcNow;
        }

        public Comment() { }

        public CommentDto ParsToDto()
        {
            return new CommentDto { Id = Id, AuthorId = AuthorId, 
                                    PostId = PostId, Text = Text };
        }
    }
}

