using large_fantasy_model.Data;
using large_fantasy_model.Hubs;
using large_fantasy_model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace large_fantasy_model.Controllers
{
    [Authorize]
    public class PrivateMessageController : Controller
    {
        private readonly LargeFantasyModelContext _context;
        private readonly IHubContext<PrivateMessageHub> _hubContext;

        public PrivateMessageController(LargeFantasyModelContext context, IHubContext<PrivateMessageHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartPrivateChat(int friendId)
        {
            int myId = GetCurrentUserId();

            
            var existingChat = await _context.Conversations
                .Include(c => c.Users)
                .Where(c => c.Game == null && c.Users.Count == 2)
                .FirstOrDefaultAsync(c => c.Users.Any(u => u.Id == myId) && c.Users.Any(u => u.Id == friendId));

            if (existingChat != null)
            {
                
                return RedirectToAction(nameof(Index), new { conversationId = existingChat.Id });
            }

            
            var me = await _context.Users.FindAsync(myId);
            var friend = await _context.Users.FindAsync(friendId);

            if (me == null || friend == null) return NotFound();

            var newConversation = new Conversation
            {
                Title = "Private Chat", 
                Users = new List<User> { me, friend }
            };

            _context.Conversations.Add(newConversation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { conversationId = newConversation.Id });
        }

       
        [HttpGet]
        public async Task<IActionResult> Index(int? conversationId)
        {
            int myId = GetCurrentUserId();

           
            var myConversations = await _context.Conversations
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .Where(c => c.Users.Any(u => u.Id == myId) && c.Game == null)
                .ToListAsync();

            ViewBag.ActiveConversationId = conversationId;

            
            var unreadCounts = new Dictionary<int, int>();
            foreach (var c in myConversations)
            {
                unreadCounts[c.Id] = c.Messages.Count(m => !m.IsRead && m.UserId != myId);
            }
            ViewBag.UnreadCounts = unreadCounts;

            if (conversationId.HasValue)
            {
                
                var unreadMsgs = await _context.Messages
                    .Where(m => m.ConversationId == conversationId.Value && !m.IsRead && m.UserId != myId)
                    .ToListAsync();

                if (unreadMsgs.Any())
                {
                    unreadMsgs.ForEach(m => m.IsRead = true);
                    await _context.SaveChangesAsync();
                }

                var activeChat = await _context.Conversations
                    .Include(c => c.Users)
                    .Include(c => c.Messages)
                        .ThenInclude(m => m.User)
                    .FirstOrDefaultAsync(c => c.Id == conversationId.Value && c.Users.Any(u => u.Id == myId));

                ViewBag.ActiveChat = activeChat;
            }

            return View("~/Views/Profile/PrivateMessage.cshtml", myConversations);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int conversationId, string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return BadRequest();

            int myId = GetCurrentUserId();

            var chat = await _context.Conversations
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Id == conversationId && c.Users.Any(u => u.Id == myId));

            if (chat == null) return Unauthorized();

            var message = new Message
            {
                Content = content,
                SendingTime = DateTime.Now,
                ConversationId = conversationId,
                UserId = myId,
                IsRead = false 
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            
            var friendId = chat.Users.FirstOrDefault(u => u.Id != myId)?.Id;
            string timeString = message.SendingTime.ToString("HH:mm");

            await _hubContext.Clients.Group($"User_{myId}").SendAsync("ReceiveMessage", conversationId, content, timeString, myId);
            if (friendId != null)
            {
                await _hubContext.Clients.Group($"User_{friendId}").SendAsync("ReceiveMessage", conversationId, content, timeString, myId);
            }

            return Ok();
        }
    }
}