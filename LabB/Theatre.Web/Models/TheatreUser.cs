using Microsoft.AspNetCore.Identity;

namespace Theatre.Web.Models
{
    public class TheatreUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
