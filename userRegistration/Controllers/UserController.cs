using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using userRegistration.Data;
using userRegistration.Models;

namespace userRegistration.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /User
        public IActionResult Index(string searchName, string genderFilter, int? stateFilter)
        {
            var users = _context.Users
                .Include(u => u.State)
                .Include(u => u.City)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                searchName = searchName.ToLower();
                users = users.Where(u => u.Name.ToLower().Contains(searchName));
            }

            // Filter by Gender
            if (!string.IsNullOrWhiteSpace(genderFilter))
            {
                users = users.Where(u => u.Gender.ToString() == genderFilter);
            }

            // Filter by State
            if (stateFilter.HasValue && stateFilter.Value != 0)
            {
                users = users.Where(u => u.StateId == stateFilter);
            }

            // Populate filters in ViewBag
            ViewBag.SearchName = searchName;
            ViewBag.GenderFilter = genderFilter;
            ViewBag.StateFilter = stateFilter;

            ViewBag.Genders = Enum.GetNames(typeof(userRegistration.Models.Enums.Gender)).ToList();

            ViewBag.States = _context.States.ToList();

            return View(users.ToList());
        }


        // GET: /User/Create
        public IActionResult CreateUser()
        {
            PopulateStates();
            ViewBag.Cities = new List<SelectListItem>();
            return View();
        }

        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User model, IFormFile? Photo)
        {
            if (ModelState.IsValid)
            {
                // Handle Photo Upload
                // Convert uploaded photo to byte[] for DB
                if (Photo != null && (Path.GetExtension(Photo.FileName).ToLower() == ".jpeg" || Path.GetExtension(Photo.FileName).ToLower() == ".jpg" || Path.GetExtension(Photo.FileName).ToLower() == ".png"))
                {
                    using (var ms = new MemoryStream())
                    {
                        Photo.CopyTo(ms);
                        byte[] photoBytes = ms.ToArray();
                        model.PhotoBlob = Convert.ToBase64String(photoBytes); // 👈 Store as base64 string

                    }
                }


                // Save to DB
                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            PopulateStates();
            if (model.StateId != 0)
            {
                ViewBag.Cities = _context.Cities
                    .Where(c => c.StateId == model.StateId)
                    .Select(c => new  { Id = c.Id.ToString(), Name = c.Name })
                    .ToList();
            }
            else
            {
                ViewBag.Cities = new List<SelectListItem>();
            }

            return View(model);
        }

        // AJAX: Get Cities by StateId
        [HttpGet]
        public JsonResult GetCities(int stateId)
        {
            var cities = _context.Cities
                .Where(c => c.StateId == stateId)
                .Select(c => new { id = c.Id, name = c.Name })
                .ToList();

            return Json(cities);
        }

        // Helpers
        private void PopulateStates()
        {
            ViewBag.States = _context.States
                .Select(s => new  { Id = s.Id.ToString(), Name = s.Name })
                .ToList();
        }
    }
}
