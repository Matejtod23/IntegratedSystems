namespace BookingApplication.Models
{
    public class BookReservation
    {
        public Guid Id { get; set; }
        public int Number_of_nights { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public Guid BookingListId { get; set; }
        public BookingList? BookingList { get; set; }
    }
}
