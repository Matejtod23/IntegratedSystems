using System.ComponentModel.DataAnnotations;

namespace Theatre.Web.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        public int Price { get; set; }
        public Guid TheatreShowId { get; set; }
        public TheatreShow? TheatreShow { get; set; }
        public string? UserId { get; set; }
        public TheatreUser? TheatreUser { get; set; }    

    }
}
