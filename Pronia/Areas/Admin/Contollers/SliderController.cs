using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.DataAccesLayer;
using Pronia.Models;
using Pronia.ViewModels.Sliders;

namespace Pronia.Areas.Admin.Contollers
{
    [Area("Admin")]
    public class SliderController(ProniaContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var data = await _context.Sliders
               
               .Select(s => new GetSliderVM
               {
                   Discount = s.Discount,
                   Id = s.Id,
                   ImageUrl = s.ImageUrl,
                   Subtitle = s.Subtitle,
                   Title = s.Title,
               }).ToListAsync();
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Slider slider = new Slider
            {
                Discount = vm.Discount,
                CreatedTime = DateTime.Now,
                ImageUrl = vm.ImageUrl,
                IsDeleted = false,
                Subtitle = vm.Subtitle,
                Title = vm.Title,
            };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (slider is null) return NotFound();

            UpdateSliderVM sliderVM = new UpdateSliderVM
            {
                Discount = slider.Discount,
                Title = slider.Title,
                Subtitle = slider.Subtitle,
                ImageUrl = slider.ImageUrl,
            };
            return View(sliderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSliderVM sliderVM)
        {
            if (id == null || id < 1) return BadRequest();
            Slider ecisted = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (ecisted is null) return NotFound();

            ecisted.Title = sliderVM.Title;
            ecisted.Subtitle = sliderVM.Subtitle;
            ecisted.Discount = sliderVM.Discount;
            ecisted.ImageUrl = sliderVM.ImageUrl;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            var ds = await _context.Sliders.FindAsync(id);
            if (ds == null) return BadRequest();
            _context.Sliders.Remove(ds);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
