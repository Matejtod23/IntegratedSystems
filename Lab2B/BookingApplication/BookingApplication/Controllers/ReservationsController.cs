using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BookingApplication.Repository;
using BookingApplication.Domain.Domain;
using BookingApplication.Service.Interface;
using BookingApplication.Repository.Interface;

namespace BookingApplication.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IReservationService reservationService;
        private readonly IBookingListService bookingListService;
        private readonly IUserRepository userRepository;

        public ReservationsController(IReservationService _reservationService, IBookingListService _bookingListService, IUserRepository _userRepository)
        {
            this.reservationService = _reservationService;
            this.bookingListService = _bookingListService;
            this.userRepository = _userRepository;  
        }


        public async Task<ActionResult> BookReservation(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reseravation = reservationService.GetReservation(id);
            var bookingList = userRepository.GetById(userId).BookingList;

            BookReservation res = new BookReservation();
            if (reseravation != null && bookingList != null)
            {
                res.BookingListId = bookingList.Id;
                res.ReservationId = reseravation.Id;
            }
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> BookReservationConfirmed(BookReservation booking)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            reservationService.BookReservationConfirmed(booking, userId);

            return RedirectToAction("Index", reservationService.GetAllReservations());
        }

        [HttpPost]
        public async Task<IActionResult> RemoveReservation(Guid? id)
        {
            if(id == null)
            {
                return NotFound();  
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            reservationService.RemoveReservation(id, userId);


            return RedirectToAction("Index", "BookingLists", reservationService.GetAllReservations());

        }



        // GET: Reservations
        public IActionResult Index()
        {
            return View(reservationService.GetAllReservations());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = reservationService.GetReservation(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Check_in_date,ApartmentId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.Id = Guid.NewGuid();
                reservationService.CreateReservation(reservation);
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = reservationService.GetReservationDetails(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Check_in_date,ApartmentId")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    reservationService.UpdateExistingReservation(reservation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = reservationService.GetReservation(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var reservation = reservationService.GetReservation(id);
            reservationService.DeleteReservation(reservation);
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(Guid id)
        {
            return reservationService.GetReservation(id) != null;
        }
    }
}
