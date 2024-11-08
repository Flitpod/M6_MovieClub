using M6_MovieClub.Data;
using M6_MovieClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace M6_MovieClub.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(UserManager<SiteUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> DelegateAdmin()
        {
            var user = await this._userManager.GetUserAsync(this.User);
            var role = new IdentityRole()
            {
                Name = "Admin",
            };

            if (!await this._roleManager.RoleExistsAsync("Admin"))
            {
                await this._roleManager.CreateAsync(role);
            }
            await this._userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View(_dbContext.Movies);
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(Movie movie)
        {
            movie.OwnerId = this._userManager.GetUserId(this.User);
            var old = this._dbContext.Movies.FirstOrDefault(m => m.Title == movie.Title && m.OwnerId == movie.OwnerId);
            if (old == null)
            {
                this._dbContext.Movies.Add(movie);
                this._dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string uid)
        {
            var item = this._dbContext.Movies.FirstOrDefault(m => m.Uid == uid);
            if (item != null && item.OwnerId == this._userManager.GetUserId(this.User))
            {
                this._dbContext.Movies.Remove(item);
                this._dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditVotes()
        {
            return View(this._dbContext.Movies);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVote(string uid)
        {
            var item = this._dbContext.Movies.FirstOrDefault(m => m.Uid == uid);
            if (item != null)
            {
                this._dbContext.Movies.Remove(item);
                this._dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(EditVotes));
        }

        public async Task<IActionResult> GetImage(string userId)
        {
            var user = this._userManager.Users.FirstOrDefault(u => u.Id == userId);
            return new FileContentResult(user.Data, user.ContentType);
        }

        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var principal = this.User; 
            var user = await this._userManager.GetUserAsync(principal);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
