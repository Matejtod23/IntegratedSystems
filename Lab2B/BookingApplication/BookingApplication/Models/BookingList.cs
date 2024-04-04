namespace BookingApplication.Models
{
    public class BookingList
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public virtual BookingApplicationUser? User { get; set; }
        public virtual ICollection<BookReservation>? BookingReservations { get; set; }
    }
}
