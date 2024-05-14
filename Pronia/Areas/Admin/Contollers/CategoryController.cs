using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccesLayer;
using Pronia.ViewModels.Categories;

namespace Pronia.Areas.Admin.Contollers
{
    [Area("Admin")]
    public class CategoryController(ProniaContext _sql) : Controller
    {
        // GET: CategoryController
        public async Task<IActionResult> Index()
        {

            return View(await _sql.Categories.Select(c=>new GetCategoryVM
            {
                Id = c.Id,
                Name = c.Name,
            }).ToListAsync());
        }


        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<ActionResult>  Create(CreateCategoryVM vm)
        {
            if (vm.Name != null && await _sql.Categories.AllAsync(c=>c.Name==vm.Name))
            {
                ModelState.AddModelError("Name","Ad mövcuddur");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await _sql.Categories.AddAsync(new Models.Category
            {
                CreatedTime = DateTime.Now,
                Name = vm.Name,
                IsDeleted =false,
            });
            await _sql.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
