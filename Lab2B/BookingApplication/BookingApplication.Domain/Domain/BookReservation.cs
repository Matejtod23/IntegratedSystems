namespace BookingApplication.Domain.Domain
{
    public class BookReservation : BaseEntity
    {
       
        public int Number_of_nights { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public Guid BookingListId { get; set; }
        public BookingList? BookingList { get; set; }
    }
}
