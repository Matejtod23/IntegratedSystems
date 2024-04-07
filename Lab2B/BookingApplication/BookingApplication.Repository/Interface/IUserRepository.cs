using BookingApplication.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<BookingApplicationUser> GetAll();
        BookingApplicationUser GetById(string? id);
        void Inser(BookingApplicationUser user);
        void Update(BookingApplicationUser user);
        void Delete(BookingApplicationUser user);

    }
}
