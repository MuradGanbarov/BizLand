using BizLand.Areas.Admin.ViewModels;
using BizLand.Areas.Admin.ViewModels.Service;
using BizLand.DAL;
using BizLand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLand.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Services.CountAsync();
            List<Service> services = await _context.Services.Skip(page*3).Take(3).ToListAsync();
            PaginationVM<Service> vm = new()
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 3),
                Items = services
            };
            return View(vm);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ServiceCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool check = await _context.Services.AnyAsync(s=>s.Name.ToLower().Trim()==vm.Name.ToLower().Trim());
            if (check)
            {
                ModelState.AddModelError("Name", "This service already existed");
                return View();
            }
            Service service = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Icon= vm.Icon,
            };
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Service existed = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            ServiceUpdateVM vm = new()
            {
                Name = existed.Name,
                Description = existed.Description,
                Icon = existed.Icon,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,ServiceUpdateVM vm)
        {
            if(!ModelState.IsValid) return View();
            Service existed = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            existed.Name = vm.Name;
            existed.Description = vm.Description;
            existed.Icon = vm.Icon;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id<= 0) return BadRequest();
            Service existed = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            _context.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
