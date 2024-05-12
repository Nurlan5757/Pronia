using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccesLayer;
using Pronia.Extensions;
using Pronia.ViewModels.Products;
using System.IO.Pipelines;

namespace Pronia.Areas.Admin.Contollers
{
    [Area("Admin")]
    public class ProductController(ProniaContext _context,IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
      
            return View(await _context.Product
                .Select(p=> new GetProductAdminVM
                {
                  CostPrice = p.CostPrice, 
                  Discount = p.Discount,
                  Id = p.Id,
                  ImageUrl = p.ImageUrl,
                  Name = p.Name,    
                  Raiting = p.Raiting,
                  SellPrice = p.SellPrice,
                  StockCount = p.StockCount,
                })
                .ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM data)
        {
            if (!data.ImageFile.IsValidType("image"))
            {
                ModelState.AddModelError("ImageFile","fayl sekil formatinda olmalidir");
            }
            if(!data.ImageFile.IsValidLength(200))
            {
                ModelState.AddModelError("ImageFile", "faylin olcusu 200kb-dan coxdu");
            }    

            if(!ModelState.IsValid)           
                return View(data);

            string fileName = await data.ImageFile.SaveFileAsync(Path.Combine(_env.WebRootPath,"imgs", "products"));
             await _context.Product.AddAsync(new Models.Product
            {
                CostPrice= data.CostPrice,
                Discount= data.Discount,
                CreatedTime= DateTime.Now,
                ImageUrl= Path.Combine("imgs", "products", fileName),
                IsDeleted= false,
                Name= data.Name,
                Raiting= data.Raiting,
                SellPrice= data.SellPrice,  
                StockCount= data.StockCount,
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
