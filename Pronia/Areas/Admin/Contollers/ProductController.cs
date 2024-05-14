using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Pronia.DataAccesLayer;
using Pronia.Extensions;
using Pronia.Models;
using Pronia.ViewModels.Products;
using System.IO.Pipelines;
using System.Text;

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
            ViewBag.Categories = await _context.Categories
                .Where(s=> !s.IsDeleted)
                .ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM data)
        {
            if(data.ImageFile != null)
            {
                if (!data.ImageFile.IsValidType("image"))
                    ModelState.AddModelError("ImageFile", "fayl sekil formatinda olmalidir");

                if (!data.ImageFile.IsValidLength(200))
                    ModelState.AddModelError("ImageFile", "faylin olcusu 200kb-dan coxdu");
            }
            bool isImageValid = true;
            StringBuilder sb = new StringBuilder();
            
            foreach(var img in data.ImageFiles)
            {

                if (!img.IsValidType("image")) 
                {
                    sb.Append("-" + img.FileName + " fayl sekil formatinda olmalidir" + "-");
                    isImageValid = false;
                    //ModelState.AddModelError("ImageFiles", img.FileName + " fayl sekil formatinda olmalidir");
                }



                if (!img.IsValidLength(200))
                {
                    sb.Append("-" + img.FileName + " faylin olcusu 200kb-dan cox olamamalidir" + "-");
                    isImageValid = false;
                    //ModelState.AddModelError("ImageFiles", img.FileName + " faylin olcusu 200kb-dan cox olamamalidir");
                }

            }
            if (!isImageValid)
            {
               
                ModelState.AddModelError("ImageFiles",sb.ToString());
            }

            if(!ModelState.IsValid)           
                return View(data);

            string fileName = await data.ImageFile.SaveFileAsync(Path.Combine(_env.WebRootPath,"imgs", "products"));
            Product prod = new Product
            {
                CostPrice = data.CostPrice,
                Discount = data.Discount,
                CreatedTime = DateTime.Now,
                ImageUrl = Path.Combine("imgs", "products", fileName),
                IsDeleted = false,
                Name = data.Name,
                Raiting = data.Raiting,
                SellPrice = data.SellPrice,
                StockCount = data.StockCount,
                Images = new List<ProductImage>()
            };
            foreach (var img in data.ImageFiles)
            {
                string imgName = await img.SaveFileAsync(Path.Combine(_env.WebRootPath, "imgs", "products"));
                prod.Images.Add(new ProductImage
                {
                    ImageUrl = Path.Combine("imgs", "products", imgName),
                    CreatedTime = DateTime.Now,
                    IsDeleted = false,                   
                });
            }
            await _context.Product.AddAsync(prod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
