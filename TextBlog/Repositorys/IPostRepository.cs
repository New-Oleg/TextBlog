using TextBlog.Models;

namespace TextBlog.Repositorys
{
    public interface IPostRepository
    {
        Post? GetById(Guid id);
        IEnumerable<Post> GetByAuthor(Guid authorId);
        IEnumerable<Post> GetBySubscribedUsers(Guid userId);
        void Add(Post post);
        void Update(Post post);
        void Delete(Guid id);
    }
}
