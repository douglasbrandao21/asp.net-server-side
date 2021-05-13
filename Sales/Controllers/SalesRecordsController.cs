using Microsoft.AspNetCore.Mvc;
using Sales.Models;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordsService _salesRecordsService;

        public SalesRecordsController(SalesRecordsService salesRecordsService)
        {
            _salesRecordsService = salesRecordsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? initialDate, DateTime? finalDate)
        {
            List<SalesRecord> records = await _salesRecordsService.FindByDateAsync(initialDate, finalDate);

            if(!initialDate.HasValue)
                initialDate = new DateTime(DateTime.Now.Year, 1, 1);

            if(!finalDate.HasValue)
                finalDate = DateTime.Now;

            ViewData["initialDate"] = initialDate.Value.ToString("yyyy-MM-dd");
            ViewData["finalDate"] = finalDate.Value.ToString("yyyy-MM-dd");

            return View(records);
        }

        public async Task<IActionResult> GroupingSearch(DateTime? initialDate, DateTime? finalDate)
        {
            List<IGrouping<Department, SalesRecord>> records = await _salesRecordsService.FindByDateGroupingAsync(initialDate, finalDate);

            if(!initialDate.HasValue)
                initialDate = new DateTime(DateTime.Now.Year, 1, 1);

            if(!finalDate.HasValue)
                finalDate = DateTime.Now;

            ViewData["initialDate"] = initialDate.Value.ToString("yyyy-MM-dd");
            ViewData["finalDate"] = finalDate.Value.ToString("yyyy-MM-dd");

            return View(records);
        }
    }
}
