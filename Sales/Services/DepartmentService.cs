using Microsoft.EntityFrameworkCore;
using Sales.Data;
using Sales.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Services
{
    public class DepartmentService
    {
        private readonly SalesContext _context;

        public DepartmentService(SalesContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync()
        {
            return await _context.Department.OrderBy(department => department.Name).ToListAsync();
        }
    }
}
