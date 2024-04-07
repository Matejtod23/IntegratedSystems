using BookingApplication.Domain.Domain;

namespace BookingApplication.Domain.DTO
{
    public class ReservationBookingListDTO
    {
        public List<BookReservation>? Reservations { get; set; }
        public double TotalPrice { get; set; }
    }
}
