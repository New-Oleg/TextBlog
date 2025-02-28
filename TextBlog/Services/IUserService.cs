public interface IUserService
{
    Task SubscribeAsync(Guid userId, Guid targetUserId);
    Task UnsubscribeAsync(Guid userId, Guid targetUserId);
}
