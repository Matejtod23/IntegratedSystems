using BookingApplication.Domain.Domain;
using BookingApplication.Domain.DTO;
using BookingApplication.Repository.Interface;
using BookingApplication.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Service.Implementation
{
    public class BookingListService : IBookingListService
    {
        private readonly IRepository<Reservation> reservationRepository;
        private readonly IRepository<BookingList> bookListRepository;
        private readonly IUserRepository userRepository;

        public BookingListService(IRepository<Reservation> _reservationRepository, IUserRepository _userRepository, IRepository<BookingList> _bookListRepository)
        {
            this.reservationRepository = _reservationRepository;
            this.userRepository = _userRepository;
            this.bookListRepository = _bookListRepository;
        }

        public void BookNow(string userId)
        {
            var loggedInUser = userRepository.GetById(userId);

            //var bookingList = loggedInUser.BookingList;
            //var allBookings = bookingList.BookingReservations.ToList();

            //foreach (var booking in allBookings)
            //{
            //    allBookings.Remove(booking);
            //}
            //bookListRepository.Update(bookingList);

            loggedInUser.BookingList?.BookingReservations?.Clear();
            userRepository.Update(loggedInUser);
        }

        public void CreateBookingList(BookingList bookingList)
        {
            bookListRepository.Insert(bookingList);
        }

        public void DeleteBookingList(BookingList bookingList)
        {
            bookListRepository.Delete(bookingList);
        }

        public ReservationBookingListDTO GetAllBookingListInfo(string userId)
        {
            var loggedInUser = userRepository.GetById(userId);
            var bookingList = loggedInUser?.BookingList;

            var allReservations = bookingList?.BookingReservations?.ToList();

            var totalPrice = allReservations.Select(r => r.Number_of_nights * r.Reservation.Apartment.Price_per_night).Sum();

            ReservationBookingListDTO dto = new ReservationBookingListDTO
            {
                TotalPrice = totalPrice,
                Reservations = allReservations
            };
            return dto;
        }

        public BookingList GetBookingList(Guid? id)
        {
            return bookListRepository.Get(id);
        }

        public void UpdateExistingBookingList(BookingList bookingList)
        {
            bookListRepository.Update(bookingList);
        }
    }
}
