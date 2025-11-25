using Microsoft.AspNetCore.Identity;

namespace Restoran.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order>? Orders {  get; set; } 

    }
}
