using BookingApplication.Domain.Domain;
using BookingApplication.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Service.Interface
{
    public interface IBookingListService
    {
        ReservationBookingListDTO GetAllBookingListInfo(string userId);
        BookingList GetBookingList(Guid? id);
        void CreateBookingList(BookingList bookingList);
        void UpdateExistingBookingList(BookingList bookingList);
        void DeleteBookingList(BookingList bookingList);
        void BookNow(string userId);         
    }
}
