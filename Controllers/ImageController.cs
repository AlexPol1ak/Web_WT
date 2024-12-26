using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Poliak_UI_WT.Data;
using System.Security.Claims;

namespace Poliak_UI_WT.Controllers
{
    public class ImageController : Controller
    {
        UserManager<ApplicationUser> _userManager;

        public ImageController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> GetAvatar()
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                return Unauthorized();
            }
            var email = emailClaim.Value;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Avatar != null)
                return File(user.Avatar, user.MimeType);
            var imagePath = Path.Combine("Images", "default-profilepicture.png");

            return File(imagePath, "image/png");
        }
    }
}
