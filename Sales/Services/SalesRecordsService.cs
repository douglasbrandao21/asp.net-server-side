using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Services
{
    public class SalesRecordsService
    {
        private readonly SalesContext _context;

        public SalesRecordsService(SalesContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? initial, DateTime? final)
        {
            IQueryable<SalesRecord> records = from salesRecord in _context.SalesRecord select salesRecord;

            if(initial.HasValue)
                records = records.Where(record => record.Date >= initial.Value);

            if(final.HasValue)
                records = records.Where(record => record.Date <= final.Value);

            return await records
                .Include(record => record.Seller)
                .Include(record => record.Seller.Department)
                .OrderByDescending(record => record.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? initialDate, DateTime? finalDate)
        {
            IQueryable<SalesRecord> records = from salesRecords in _context.SalesRecord select salesRecords;

            if(initialDate.HasValue)
                records = records.Where(record => record.Date >= initialDate.Value);

            if(finalDate.HasValue)
                records = records.Where(record => record.Date <= finalDate.Value);

            return await records
                .Include(record => record.Seller)
                .Include(record => record.Seller.Department)
                .OrderByDescending(record => record.Date)
                .GroupBy(record => record.Seller.Department)
                .ToListAsync();
        }
    }
}
