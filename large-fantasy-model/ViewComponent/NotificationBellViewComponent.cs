using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using large_fantasy_model.Data;
using System.Security.Claims;

namespace large_fantasy_model.ViewComponents
{
    public class NotificationBellViewComponent : ViewComponent
    {
        private readonly LargeFantasyModelContext _context;

        public NotificationBellViewComponent(LargeFantasyModelContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var claimsPrincipal = User as ClaimsPrincipal;
            var userIdClaim = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Content(""); 
            }

            int userId = int.Parse(userIdClaim);

            
            var notifications = await _context.Notifications
                .Include(n => n.Sender)
                .Where(n => n.ReceiverId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notifications); 
        }
    }
}