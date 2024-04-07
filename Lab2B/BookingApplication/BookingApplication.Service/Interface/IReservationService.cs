using BookingApplication.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Service.Interface
{
    public interface IReservationService
    {
        List<Reservation> GetAllReservations();
        Reservation GetReservation(Guid? id);
        Reservation GetReservationDetails(Guid? id);
        void CreateReservation(Reservation reservation);
        void UpdateExistingReservation(Reservation reservation);
        void DeleteReservation(Reservation reservation);
        bool BookReservationConfirmed(BookReservation bookReservation, string? userId);
        bool RemoveReservation(Guid? reservationId, string? userId);

    }
}
