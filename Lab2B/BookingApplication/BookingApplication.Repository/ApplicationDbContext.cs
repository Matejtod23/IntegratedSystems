using BookingApplication.Domain.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BookingApplication.Repository
{
    public class ApplicationDbContext : IdentityDbContext<BookingApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Apartment> Apartments { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        public virtual DbSet<BookReservation> BookingReservations { get; set; }
        public virtual DbSet<BookingList> BookingLists { get; set; }
    }
}
