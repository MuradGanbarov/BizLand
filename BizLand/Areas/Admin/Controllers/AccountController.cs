using BizLand.Areas.Admin.Models.Utilities.Enums;
using BizLand.Areas.Admin.ViewModels;
using BizLand.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BizLand.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new()
            {
                Name = vm.Name,
                Surname = vm.Surname,
                Email = vm.Email,
                UserName = vm.UserName,
            };

            IdentityResult result = await _userManager.CreateAsync(user,vm.Password);
            if (!result.Succeeded)
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user,UserRole.Member.ToString());
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
            if(user is null)
            {
                user = await _userManager.FindByNameAsync(vm.UserNameOrEmail);
                if(user is null)
                {
                    ModelState.AddModelError(string.Empty, "Email,username or password is incorrect");
                    return View();
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email,username or password is incorrect");
                return View();
            }
            if(result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Please try later,we have maintance");
                return View();
            }
            return RedirectToAction("Index", "Home", new { Area = "" });

        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach(UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if(!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });
                }
            }
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

    }
}
