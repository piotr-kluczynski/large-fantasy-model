using large_fantasy_model.Data;
using large_fantasy_model.Hubs;
using large_fantasy_model.Models;
using large_fantasy_model.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace large_fantasy_model.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly LargeFantasyModelContext _context;
        private readonly IHubContext<PrivateMessageHub> _hubContext;

        public ProfileController(LargeFantasyModelContext context, IHubContext<PrivateMessageHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public async Task<IActionResult> ProfilePage()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Friends(string searchQuery)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var user = await _context.Users
                .Include(u => u.Friends)
                .Include(u => u.FriendOf)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            var mutualFriends = user.Friends.Where(f => user.FriendOf.Any(fo => fo.Id == f.Id)).ToList();
            var sentRequests = user.Friends.Where(f => !user.FriendOf.Any(fo => fo.Id == f.Id)).ToList();
            var receivedRequests = user.FriendOf.Where(fo => !user.Friends.Any(f => f.Id == fo.Id)).ToList();

            var viewModel = new FriendsViewModel
            {
                SearchQuery = searchQuery,
                MutualFriends = mutualFriends.Select(f => new UserViewModel { Id = f.Id, Username = f.Username }).ToList(),
                SentRequests = sentRequests.Select(f => new UserViewModel { Id = f.Id, Username = f.Username }).ToList(),
                ReceivedRequests = receivedRequests.Select(fo => new UserViewModel { Id = fo.Id, Username = fo.Username }).ToList()
            };

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                viewModel.SearchResults = await _context.Users
                    .Where(u => u.Username.Contains(searchQuery) && u.Id != userId)
                    .Select(u => new UserViewModel { Id = u.Id, Username = u.Username })
                    .ToListAsync();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFriend(int friendId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var user = await _context.Users
                .Include(u => u.Friends)
                .Include(u => u.FriendOf)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var newFriend = await _context.Users.FindAsync(friendId);

            if (user != null && newFriend != null && !user.Friends.Any(f => f.Id == friendId))
            {
                user.Friends.Add(newFriend);
                await _context.SaveChangesAsync();

                if (user.FriendOf.Any(fo => fo.Id == friendId))
                {
                    TempData["SuccessMessage"] = $"You and {newFriend.Username} are now friends!";

                    await _hubContext.Clients.Group($"User_{friendId}")
                         .SendAsync("ReceiveFriendAccept", user.Id, user.Username);
                }
                else
                {
                    TempData["SuccessMessage"] = $"Friend request sent to {newFriend.Username}!";
                    var notification = new Notification
                    {
                        ReceiverId = friendId,
                        SenderId = user.Id,
                        Type = "FriendRequest",
                        Message = "wants to add you to their friends list.",
                        RelatedEntityId = user.Id
                    };
                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();

                    await _hubContext.Clients.Group($"User_{friendId}")
                        .SendAsync("ReceiveFriendRequest", user.Id, user.Username);

                    await _hubContext.Clients.Group($"User_{friendId}").SendAsync("UpdateNotifications");
                }

                var notificationToRemove = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.ReceiverId == userId && n.SenderId == friendId && n.Type == "FriendRequest");

                if (notificationToRemove != null)
                {
                    _context.Notifications.Remove(notificationToRemove);
                    await _context.SaveChangesAsync();

                    await _hubContext.Clients.Group($"User_{userId}").SendAsync("UpdateNotifications");
                }
            }

            return RedirectToAction(nameof(Friends));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFriend(int friendId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var user = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == userId);
            var friend = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == friendId);

            if (user != null && friend != null)
            {
                if (user.Friends.Any(f => f.Id == friendId)) user.Friends.Remove(friend);
                if (friend.Friends.Any(f => f.Id == userId)) friend.Friends.Remove(user);

                await _context.SaveChangesAsync();

                TempData["DangerMessage"] = $"Traveler {friend.Username} has been removed.";
                await _hubContext.Clients.Group($"User_{friendId}")
                    .SendAsync("ReceiveFriendRemove", user.Id);
            }

            var sentNotification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.SenderId == userId && n.ReceiverId == friendId && n.Type == "FriendRequest");

            if (sentNotification != null)
            {
                _context.Notifications.Remove(sentNotification);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.Group($"User_{friendId}").SendAsync("UpdateNotifications");
            }

            var receivedNotification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.ReceiverId == userId && n.SenderId == friendId && n.Type == "FriendRequest");

            if (receivedNotification != null)
            {
                _context.Notifications.Remove(receivedNotification);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.Group($"User_{userId}").SendAsync("UpdateNotifications");
            }

            return RedirectToAction(nameof(Friends));
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var viewModel = new EditProfileViewModel
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var userInDb = await _context.Users.FindAsync(userId);
            if (userInDb == null) return NotFound();

            if (userInDb.Username != model.Username)
            {
                var usernameExists = await _context.Users.AnyAsync(u => u.Username == model.Username && u.Id != userId);
                if (usernameExists)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(model);
                }
            }

            userInDb.Username = model.Username;
            userInDb.FirstName = model.FirstName;
            userInDb.LastName = model.LastName ?? "";
            userInDb.Bio = model.Bio ?? "";

            _context.Update(userInDb);
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userInDb.Id.ToString()),
                new Claim(ClaimTypes.Name, userInDb.Username),
                new Claim(ClaimTypes.Email, userInDb.Email),
                new Claim("AdminPermissions", userInDb.AdminPermissions.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction(nameof(ProfilePage));
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Incorrect current password.");
                return View(model);
            }

            user.Password = model.NewPassword;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProfilePage));
        }

        [HttpGet]
        public IActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Incorrect current password.");
                return View(model);
            }

            var emailExists = await _context.Users.AnyAsync(u => u.Email == model.NewEmail && u.Id != userId);
            if (emailExists)
            {
                ModelState.AddModelError("NewEmail", "This email is already taken by another traveler.");
                return View(model);
            }

            user.Email = model.NewEmail;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProfilePage));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptFriendInvite(int notificationId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int myId = int.Parse(userIdString!);

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.ReceiverId == myId && n.Type == "FriendRequest");

            if (notification != null && notification.SenderId.HasValue)
            {
                int friendId = notification.SenderId.Value;

                var user = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == myId);
                var newFriend = await _context.Users.FindAsync(friendId);

                if (user != null && newFriend != null && !user.Friends.Any(f => f.Id == friendId))
                {
                    user.Friends.Add(newFriend);
                    TempData["SuccessMessage"] = $"You and {newFriend.Username} are now friends!";

                    await _hubContext.Clients.Group($"User_{friendId}")
                         .SendAsync("ReceiveFriendAccept", user.Id, user.Username);
                }

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Friends");
        }

        [HttpPost]
        public async Task<IActionResult> MarkNotificationsAsRead()
        {
            int myId = GetCurrentUserId();
            var notifications = await _context.Notifications
                .Where(n => n.ReceiverId == myId && !n.IsRead)
                .ToListAsync();

            foreach (var n in notifications)
            {
                n.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineFriendInvite(int notificationId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int myId = int.Parse(userIdString!);

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.ReceiverId == myId && n.Type == "FriendRequest");

            if (notification != null && notification.SenderId.HasValue)
            {
                int senderId = notification.SenderId.Value;

                var sender = await _context.Users.Include(u => u.Friends).FirstOrDefaultAsync(u => u.Id == senderId);
                var me = await _context.Users.FindAsync(myId);

                if (sender != null && me != null && sender.Friends.Any(f => f.Id == myId))
                {
                    sender.Friends.Remove(me);
                }

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.Group($"User_{myId}").SendAsync("UpdateNotifications");
                await _hubContext.Clients.Group($"User_{senderId}").SendAsync("FriendRequestDeclined", myId);
            }

            string referer = Request.Headers["Referer"].ToString();
            return !string.IsNullOrEmpty(referer) ? Redirect(referer) : RedirectToAction("Friends");
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationCounts()
        {
            int myId = GetCurrentUserId();

            var friendRequestsCount = await _context.Notifications
                .CountAsync(n => n.ReceiverId == myId && !n.IsRead && n.Type == "FriendRequest");

            var unreadMessagesCount = await _context.Messages
                .CountAsync(m => !m.IsRead && m.UserId != myId && m.Conversation.Game == null && m.Conversation.Users.Any(u => u.Id == myId));

            return Json(new { friendRequestsCount, unreadMessagesCount });
        }
    }
}