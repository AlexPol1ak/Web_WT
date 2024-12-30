using Microsoft.AspNetCore.Identity;

namespace Poliak_UI_WT.Data
{
    /// <summary>
    /// Класс пользователя.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public byte[]? Avatar { get; set; }
        public string MimeType { get; set; } = string.Empty;

    }
}
