using TextBlog.Dtos;
using TextBlog.Models;

namespace TextBlog.Repositorys
{
    public interface IPostRepository
    {
        Post? GetById(Guid id);
        IEnumerable<PostDto> GetByAuthor(Guid authorId);
        void Add(Post post);
        void Update(Post post);
        void Delete(Guid id);
    }
}
