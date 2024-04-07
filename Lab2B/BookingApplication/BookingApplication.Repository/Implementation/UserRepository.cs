using BookingApplication.Domain.Domain;
using BookingApplication.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext context;
        private DbSet<BookingApplicationUser> enteties;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context; 
            enteties = context.Set<BookingApplicationUser>();
        }

        public void Delete(BookingApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            enteties.Remove(user);
            context.SaveChanges();
        }

        public IEnumerable<BookingApplicationUser> GetAll()
        {
            return enteties.AsEnumerable();
        }

        public BookingApplicationUser GetById(string? id)
        {
            return enteties
                .Include(e => e.BookingList)
                .Include("BookingList.BookingReservations")
                .Include("BookingList.BookingReservations.Reservation")
                .Include("BookingList.BookingReservations.Reservation.Apartment")
                .SingleOrDefault(u => u.Id == id);
        }

        public void Inser(BookingApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            enteties.Add(user); 
            context.SaveChanges();
        }

        public void Update(BookingApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            enteties.Update(user);
            context.SaveChanges();
        }
    }
}
