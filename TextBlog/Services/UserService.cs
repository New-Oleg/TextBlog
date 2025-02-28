using TextBlog.Models;
using TextBlog.Repositorys;
using TextBlog.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task SubscribeAsync(Guid userId, Guid targetUserId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        var targetUser = await _userRepo.GetByIdAsync(targetUserId);

        if (user == null || targetUser == null)
            throw new ArgumentException("Пользователь не найден");

        if (user.Subscriptions == null || !user.Subscriptions.Contains(targetUserId) )
        {
            user.Subscriptions.Add(targetUserId);
            await _userRepo.Update(user);
        }
    }

    public async Task UnsubscribeAsync(Guid userId, Guid targetUserId)
    {
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null) throw new ArgumentException("Пользователь не найден");

        if (user.Subscriptions.Contains(targetUserId))
        {
            user.Subscriptions.Remove(targetUserId);
            await _userRepo.Update(user);
        }
    }
}
