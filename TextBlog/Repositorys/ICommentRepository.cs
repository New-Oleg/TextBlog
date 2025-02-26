using TextBlog.Models;
using Microsoft.EntityFrameworkCore;
using TextBlog.Data;

namespace TextBlog.Repositorys
{
    public interface ICommentRepository
    {
        Comment? GetById(Guid id);
        IEnumerable<Comment> GetByPostId(Guid postId);
        void Add(Comment comment);
        void Update(Comment comment);
        void Delete(Guid id);
    }
}
