namespace TextBlog.SignalRHub
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class CommentHub : Hub
    {
        public async Task SendComment(string postId, string user, string message)
        {
            await Clients.All.SendAsync("ReceiveComment", postId, user, message);
        }
    }
}
