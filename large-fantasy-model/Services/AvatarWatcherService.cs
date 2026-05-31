using Microsoft.AspNetCore.SignalR;
using large_fantasy_model.Hubs;

namespace large_fantasy_model.Services
{
    public class AvatarWatcherService : IHostedService, IDisposable
    {
        private FileSystemWatcher _watcher;
        private readonly IHubContext<PrivateMessageHub> _pmHub;
        private readonly IHubContext<GameHub> _gameHub;
        private readonly IHubContext<LobbyHub> _lobbyHub;
        private readonly IWebHostEnvironment _env;
        private readonly IServiceProvider _serviceProvider;

        public AvatarWatcherService(
            IHubContext<PrivateMessageHub> pmHub,
            IHubContext<GameHub> gameHub,
            IHubContext<LobbyHub> lobbyHub,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider)
        {
            _pmHub = pmHub;
            _gameHub = gameHub;
            _lobbyHub = lobbyHub;
            _env = env;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var path = Path.Combine(_env.WebRootPath, "imgs", "avatars");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            _watcher = new FileSystemWatcher(path);
            _watcher.Deleted += OnDeleted;
            _watcher.EnableRaisingEvents = true;

            return Task.CompletedTask;
        }

        private async void OnDeleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<large_fantasy_model.Data.LargeFantasyModelContext>();
                var deletedFileName = e.Name.Replace("\\", "/");
                var user = context.Users.FirstOrDefault(u => u.ProfilePicturePath != null && u.ProfilePicturePath.EndsWith(deletedFileName));
                
                if (user != null)
                {
                    user.ProfilePicturePath = null;
                    context.Update(user);
                    await context.SaveChangesAsync();

                    await _pmHub.Clients.All.SendAsync("UserAvatarChanged", user.Username, "");
                    await _gameHub.Clients.All.SendAsync("UserAvatarChanged", user.Username, "");
                    await _lobbyHub.Clients.All.SendAsync("UserAvatarChanged", user.Username, "");
                }
            }
            catch
            {
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _watcher?.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _watcher?.Dispose();
        }
    }
}
