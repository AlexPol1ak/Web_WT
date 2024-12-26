using Microsoft.AspNetCore.Identity;

namespace Poliak_UI_WT.Data
{
    public class ApplicationUser: IdentityUser
    {
        public byte[] Avatar { get; set; } 
        public string MimeType { get; set; } = string.Empty;
       
    }
}
