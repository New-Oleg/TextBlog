using Microsoft.EntityFrameworkCore;
using TextBlog.Data;
using TextBlog.Dtos;
using TextBlog.Models;

namespace TextBlog.Repositorys
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public Comment? GetById(Guid id)
        {
            return _context.Comments.Local.FirstOrDefault(c => c.Id == id)
            ?? _context.Comments.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<CommentDto> GetByPostId(Guid postId)
        {
            return _context.Comments
                .Where(c => c.PostId == postId)
                .Select(c => c.ParsToDto())
                .ToList();
        }

        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
        }
    }
}
