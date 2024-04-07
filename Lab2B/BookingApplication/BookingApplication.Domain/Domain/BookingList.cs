namespace BookingApplication.Domain.Domain
{
    public class BookingList : BaseEntity
    {
      
        public string? UserId { get; set; }
        public virtual BookingApplicationUser? User { get; set; }
        public virtual ICollection<BookReservation>? BookingReservations { get; set; }
    }
}
