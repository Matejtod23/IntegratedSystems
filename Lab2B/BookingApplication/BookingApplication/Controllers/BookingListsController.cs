using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingApplication.Data;
using BookingApplication.Models;
using System.Security.Claims;
using BookingApplication.Models.DTO;

namespace BookingApplication.Controllers
{
    public class BookingListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookingLists
        public async Task<IActionResult> Index()
        
        
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = await _context.Users
                .Include(x => x.BookingList)
                .ThenInclude(bl => bl.BookingReservations)
                .ThenInclude(r => r.Reservation)
                .ThenInclude(a => a.Apartment)
                .FirstOrDefaultAsync(z => z.Id == userId);

            var userBookingList = loggedInUser?.BookingList;
            var allBookings = userBookingList?.BookingReservations.ToList();

            var totalPrice = allBookings.Select(b => (b.Reservation?.Apartment?.Price_per_night * b.Number_of_nights)).Sum();

             ReservationBookingListDTO dto = new ReservationBookingListDTO
            {
                Reservations = allBookings,
                TotalPrice = (double)totalPrice,
            };

            return View(dto);
        }

        public IActionResult BookNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = _context.Users
                  .Include(x => x.BookingList)
                  .Include("BookingList.BookingReservations")
                  .Include("BookingList.BookingReservations.Reservation")
                  .FirstOrDefault(z => z.Id == userId);


            var userReseravions = loggedInUser?.BookingList;
            var allBookings = userReseravions?.BookingReservations?.ToList();
            
            foreach (var res in allBookings)
            {
                userReseravions.BookingReservations.Remove(res);
            }

            _context.BookingLists.Update(userReseravions);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: BookingLists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingList = await _context.BookingLists
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookingList == null)
            {
                return NotFound();
            }

            return View(bookingList);
        }

        // GET: BookingLists/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BookingLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId")] BookingList bookingList)
        {
            if (ModelState.IsValid)
            {
                bookingList.Id = Guid.NewGuid();
                _context.Add(bookingList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookingList.UserId);
            return View(bookingList);
        }

        // GET: BookingLists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingList = await _context.BookingLists.FindAsync(id);
            if (bookingList == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookingList.UserId);
            return View(bookingList);
        }

        // POST: BookingLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserId")] BookingList bookingList)
        {
            if (id != bookingList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingListExists(bookingList.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookingList.UserId);
            return View(bookingList);
        }

        // GET: BookingLists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingList = await _context.BookingLists
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookingList == null)
            {
                return NotFound();
            }

            return View(bookingList);
        }

        // POST: BookingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var bookingList = await _context.BookingLists.FindAsync(id);
            if (bookingList != null)
            {
                _context.BookingLists.Remove(bookingList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingListExists(Guid id)
        {
            return _context.BookingLists.Any(e => e.Id == id);
        }
    }
}
