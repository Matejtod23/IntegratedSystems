using BookingApplication.Domain.Domain;
using BookingApplication.Repository.Interface;
using BookingApplication.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Service.Implementation
{
    public class ReservationService : IReservationService
    {

        private readonly IRepository<Reservation> reservationRepository;
        private readonly IRepository<BookingList> bookListRepository;
        private readonly IUserRepository userRepository;

        public ReservationService(IRepository<Reservation> _reservationRepository, IUserRepository _userRepository, IRepository<BookingList> _bookListRepository)
        {
            this.reservationRepository = _reservationRepository;   
            this.userRepository = _userRepository;
            this.bookListRepository = _bookListRepository;
        }

        public bool BookReservationConfirmed(BookReservation bookReservation, string userId)
        {
            var loggedInUser = userRepository.GetById(userId);

            var bookingList = loggedInUser.BookingList;

            if (bookingList.BookingReservations == null)
            {
                bookingList.BookingReservations = new List<BookReservation>();
            }
            bookingList.BookingReservations.Add(bookReservation);
            bookListRepository.Update(bookingList);
            return true;

        }

        public void CreateReservation(Reservation reservation)
        {
            this.reservationRepository.Insert(reservation);
        }

        public void DeleteReservation(Reservation reservation)
        {
            this.reservationRepository.Delete(reservation);
        }

        public List<Reservation> GetAllReservations()
        {
            return this.reservationRepository.GetAll().ToList();
        }

        public Reservation GetReservation(Guid? id)
        {
            var reservation = this.reservationRepository.Get(id);
            return reservation;
        }

        public Reservation GetReservationDetails(Guid? id)
        {
            var reservation = this.reservationRepository.Get(id);
            return reservation;
        }

        public bool RemoveReservation(Guid? reservationId, string? userId)
        {

            if(reservationId != null)
            {

                var loggedInUser = userRepository.GetById(userId);

                var bookingList = loggedInUser.BookingList;
                var bReservation = bookingList.BookingReservations.FirstOrDefault(res => res.Id == reservationId);

                bookingList.BookingReservations.Remove(bReservation);

                bookListRepository.Update(bookingList);
                return true;
            }
            return false;
        }

        public void UpdateExistingReservation(Reservation reservation)
        {
            this.reservationRepository.Update(reservation);
        }
    }
}
