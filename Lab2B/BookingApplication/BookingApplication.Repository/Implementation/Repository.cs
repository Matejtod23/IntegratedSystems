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
    public class Repository<T> : IRepository<T> where T : BaseEntity    
    {
        private readonly ApplicationDbContext context;
        private DbSet<T> enteties;
        string errorMessage = string.Empty;

        public Repository(ApplicationDbContext _context)
        {
            this.context = _context;
            enteties = context.Set<T>();
        }

        public void Delete(T entity)
        {
           if (entity == null) {
                throw new ArgumentNullException("entity"); 
           }

           enteties.Remove(entity);
           context.SaveChanges();
        }

        public T Get(Guid? id)
        {
            if(id == null) { throw new ArgumentNullException("id"); }

            return enteties.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return enteties.AsEnumerable();
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            enteties.Add(entity);   
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            enteties.Update(entity);
            context.SaveChanges();
        }
    }
}
