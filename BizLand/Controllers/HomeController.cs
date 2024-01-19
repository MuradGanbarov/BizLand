using BizLand.DAL;
using BizLand.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BizLand.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new()
            {
                Positions = await _context.Positions.ToListAsync(),
                Services= await _context.Services.ToListAsync(),
                Teams= await _context.Teams.Include(p=>p.Position).ToListAsync(),
            };


            return View(vm);
        }

      
    }
}