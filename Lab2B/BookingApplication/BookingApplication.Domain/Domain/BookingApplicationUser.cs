﻿using Microsoft.AspNetCore.Identity;

namespace BookingApplication.Domain.Domain
{
    public class BookingApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
        public Guid? BookingId { get; set; }
        public BookingList? BookingList { get; set; }
    }
}
