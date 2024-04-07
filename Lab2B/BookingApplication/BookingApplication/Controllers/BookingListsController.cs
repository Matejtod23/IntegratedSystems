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

namespace BookingApplication.Controllers
{
    public class BookingListsController : Controller
    {
        private readonly IBookingListService bookingListSerivce;

        public BookingListsController(IBookingListService _bookingListSerivce)
        {
            this.bookingListSerivce = _bookingListSerivce;
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
