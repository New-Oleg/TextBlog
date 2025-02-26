using TextBlog.Data;
using TextBlog.Models;

namespace TextBlog.Repositorys
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public Post? GetById(Guid id)
        {
            return _context.Posts.Local.FirstOrDefault(c => c.Id == id)
                ?? _context.Posts.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Post> GetByAuthor(Guid authorId)
        {
            return _context.Posts.Where(p => p.AuthorId == authorId).ToList();
        }

        public IEnumerable<Post> GetBySubscribedUsers(Guid userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return Enumerable.Empty<Post>();

            return _context.Posts
                .Where(p => user.Subscriptions.Contains(p.AuthorId))
                .ToList();
        }

        public void Add(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void Update(Post post)
        {
            _context.Posts.Update(post);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
        }
    }
}
