using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Models;
using Sales.Services.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sales.Services
{
    public class SellerService
    {
        private readonly SalesContext _context;

        public SellerService(SalesContext context)
        {
            _context = context;
        }


        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);

            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(seller => seller.Department).FirstOrDefaultAsync(seller => seller.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                Seller seller = await _context.Seller.FindAsync(id);

                _context.Seller.Remove(seller);

                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException exception)
            {
                throw new IntegrityException(exception.Message);
            }   
        }

        public async Task UpdateAsync(Seller seller)
        {
            bool sellerExists = await _context.Seller.AnyAsync(_seller => _seller.Id == seller.Id);

            if(!sellerExists)
                throw new NotFoundException("Id not found");

            try
            {
                _context.Update(seller);

                await _context.SaveChangesAsync();
            }
            catch(DbConcurrencyException exception)
            {
                throw new DbConcurrencyException(exception.Message);
            }
        }
    }
}
