using Microsoft.AspNetCore.Mvc;
using Sales.Models;
using Sales.Models.ViewModels;
using Sales.Services;
using Sales.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sales.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            List<Seller> sellers = await _sellerService.FindAllAsync();

            return View(sellers);
        }

        public async Task<IActionResult> Create()
        {
            List<Department> departments = await _departmentService.FindAllAsync();

            SellerFormViewModel sellerFormViewModel = new SellerFormViewModel { Departments = departments };

            return View(sellerFormViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
                return RedirectToAction("Error", new { message = "Id not found" });

            Seller seller = await _sellerService.FindByIdAsync(id.Value);

            if(seller == null)
                return RedirectToAction("Error", new { message = "Id not found" });

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
                return RedirectToAction("Error", new { message = "Id not found" });

            Seller seller = await _sellerService.FindByIdAsync(id.Value);

            if(seller == null)
                return RedirectToAction("Error", new { message = "Id not found" });

            return View(seller);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
                return RedirectToAction("Error", new { message = "Id not found" });

            Seller seller = await _sellerService.FindByIdAsync(id.Value);

            if(seller == null)
                return RedirectToAction("Error", new { message = "Id not found" });

            return View(seller);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if(!ModelState.IsValid)
            {
                List<Department> departments = await _departmentService.FindAllAsync();

                SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Seller seller)
        {
            if(!ModelState.IsValid)
            {
                List<Department> departments = await _departmentService.FindAllAsync();

                SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return View(viewModel);
            }

            try
            {
                if(id.Value != seller.Id)
                    return BadRequest();

                await _sellerService.UpdateAsync(seller);

                return RedirectToAction("Index");
            }
            catch(ApplicationException exception)
            {
                return RedirectToAction("Error", new { message = exception.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);

                return RedirectToAction("Index");
            }
            catch(IntegrityException)
            {
                return RedirectToAction("Error", new { message = "Cannot delete a seller with sales" });
            }   
        }

        public IActionResult Error(string message)
        {
            ErrorViewModel viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
