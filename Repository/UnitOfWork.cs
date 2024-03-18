using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly Eyeglasses2024DBContext _context;
        public UnitOfWork(Eyeglasses2024DBContext context)
        {
            _context = context;
        }
        
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class 
            => new GenericRepository<TEntity>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
