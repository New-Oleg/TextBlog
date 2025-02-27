namespace TextBlog.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Guid>? Subscriptions { get; set; }
    }
}
