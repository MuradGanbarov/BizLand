using BizLand.Areas.Admin.Models.Utilities.Enums;
using BizLand.Areas.Admin.Models.Utilities.Extentions;
using BizLand.Areas.Admin.ViewModels;
using BizLand.Areas.Admin.ViewModels.Team;
using BizLand.DAL;
using BizLand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLand.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Teams.CountAsync();
            List<Team> teams = await _context.Teams.Include(t => t.Position).Skip(page * 3).Take(3).ToListAsync();
            PaginationVM<Team> vm = new()
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 3),
                Items = teams
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            TeamCreateVM vm = new()
            {
                Positions = await _context.Positions.ToListAsync(),
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TeamCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            bool check = await _context.Positions.AnyAsync(p => p.Id == vm.PositionId);
            if (!check)
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Name", "This position doesn't existed");
                return View(vm);
            }

            if (!vm.Photo.IsValidType(FileType.Image))
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Image", "Photo should be image type");
                return View(vm);
            }
            if (!vm.Photo.IsValidSize(5, FileSize.Megabyte))
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Image", "Photo can be less or equal 5mb");
                return View(vm);
            }

            Team team = new()
            {
                FullName = vm.FullName,
                ImageURL = await vm.Photo.CreateAsync(_env.WebRootPath, "assets", "img", "team"),
                PositionId = vm.PositionId,
                FaceLink= vm.FaceLink,
                InstaLink= vm.InstaLink,
                LinkedInLink= vm.LinkedInLink,
                TwitLink = vm.TwitLink
            };
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Team existed = await _context.Teams.Include(t => t.Position).FirstOrDefaultAsync(t => t.Id == id);
            if (existed is null) return NotFound();
            TeamUpdateVM vm = new()
            {
                FullName = existed.FullName,
                ImageURL = existed.ImageURL,
                PositionId = existed.PositionId,
                FaceLink = existed.FaceLink,
                InstaLink = existed.InstaLink,
                TwitLink = existed.TwitLink,
                LinkedInLink = existed.LinkedInLink,
                Positions = await _context.Positions.ToListAsync(),
            };
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,TeamUpdateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Team existed = await _context.Teams.Include(t => t.Position).FirstOrDefaultAsync(t => t.Id == id);
            if (existed is null) return NotFound();
            if(vm.Photo is not null)
            {
                if (!vm.Photo.IsValidType(FileType.Image))
                {
                    vm.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Image", "Photo should be image type");
                    return View(vm);
                }
                if (!vm.Photo.IsValidSize(5, FileSize.Megabyte))
                {
                    vm.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Image", "Photo can be less or equal 5mb");
                    return View(vm);
                }
                existed.ImageURL.Delete(_env.WebRootPath, "assets", "img", "team");
                existed.ImageURL = await vm.Photo.CreateAsync(_env.WebRootPath, "assets", "img", "team");

            }
            existed.FullName = vm.FullName;
            existed.InstaLink = vm.InstaLink;
            existed.LinkedInLink = vm.LinkedInLink;
            existed.TwitLink = vm.TwitLink;
            existed.FaceLink = vm.FaceLink;
            existed.PositionId = vm.PositionId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Team existed = await _context.Teams.Include(t => t.Position).FirstOrDefaultAsync(t => t.Id == id);
            if (existed is null) return NotFound();
            existed.ImageURL.Delete(_env.WebRootPath, "assets", "img", "team");
            _context.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
