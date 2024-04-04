namespace BookingApplication.Models.DTO
{
    public class ReservationBookingListDTO
    {
        public List<BookReservation>? Reservations { get; set; }
        public double TotalPrice { get; set; }
    }
}
