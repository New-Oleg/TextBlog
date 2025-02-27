using TextBlog.Models;
using Microsoft.EntityFrameworkCore;
using TextBlog.Data;
using TextBlog.Dtos;

namespace TextBlog.Repositorys
{
    public interface ICommentRepository
    {
        Comment? GetById(Guid id);

        Task SaveChangesAsync();
        IEnumerable<CommentDto> GetByPostId(Guid postId);
        void Add(Comment comment);
        void Update(Comment comment);
        void Delete(Guid id);
    }
}
