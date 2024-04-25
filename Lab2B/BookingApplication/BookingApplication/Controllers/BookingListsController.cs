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
using Stripe;
using BookingApplication.Repository.Interface;

namespace BookingApplication.Controllers
{
    public class BookingListsController : Controller
    {
        private readonly IBookingListService bookingListSerivce;
        private readonly IUserRepository userRepository;

        public BookingListsController(IBookingListService _bookingListSerivce, IUserRepository _userRepository)
        {
            this.bookingListSerivce = _bookingListSerivce;
            this.userRepository = _userRepository;  
        }

        // GET: BookingLists
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = bookingListSerivce.GetAllBookingListInfo(userId);

            return View(dto);
        }

        public IActionResult BookNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bookingListSerivce.BookNow(userId);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult PayBooking(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            StripeConfiguration.ApiKey = "sk_test_51P97sJ06gNlu5LfD30asLgszutxK2EJDuxzwY1kU9oG75gqL4BNMbNsOyammhVurS83dH4cgaUoavx1ndNdawd4800URrGuK8B";

            var bookingList = userRepository.GetById(userId).BookingList;

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (bookingList.BookingReservations.Select(r => r.Number_of_nights * r.Reservation.Apartment.Price_per_night).Sum()) * 100,

                Description = "Booking Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if(charge.Status == "succeeded")
            {
                var result = this.BookNow();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: BookingLists/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingList = bookingListSerivce.GetBookingList(id);
            if (bookingList == null)
            {
                return NotFound();
            }

            return View(bookingList);
        }

        // GET: BookingLists/Create
        public IActionResult Create()
        {
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
                bookingListSerivce.CreateBookingList(bookingList);
                return RedirectToAction(nameof(Index));
            }
            return View(bookingList);
        }

        // GET: BookingLists/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingList = bookingListSerivce.GetBookingList(id);
            if (bookingList == null)
            {
                return NotFound();
            }
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
                   bookingListSerivce.UpdateExistingBookingList(bookingList);   
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
            return View(bookingList);
        }

        // GET: BookingLists/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingList = bookingListSerivce.GetBookingList(id);
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
            var bookingList = bookingListSerivce.GetBookingList(id);
            bookingListSerivce.DeleteBookingList(bookingList);
            return RedirectToAction(nameof(Index));
        }

        private bool BookingListExists(Guid id)
        {
           return bookingListSerivce.GetBookingList(id) != null;
        }
    }
}
