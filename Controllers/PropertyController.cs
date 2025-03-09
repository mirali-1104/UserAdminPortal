using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using UserAdminPortal.ViewModel;
using UserAdminPortal.Data;
using UserAdminPortal.Models;

namespace UserAdminPortal.Controllers
{
    public class PropertyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public PropertyController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var properties = _context.Properties.ToList();
            return View(properties);
        }
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var property = new Property
            {
                UserId = user.Id,
                PropertyName = model.PropertyName,
                Title = model.Title,
                Price = model.Price,
                Description = model.Description
            };

            if (model.ImageFile != null)
            {
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                property.Image = "/images/" + uniqueFileName;
            }

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var property = await _context.Properties.FindAsync(id.Value);
            if (property == null)
            {
                return NotFound();
            }

            var viewModel = new PropertyViewModel
            {
                PropertyId = property.PropertyId, 
                PropertyName = property.PropertyName,
                Title = property.Title,
                Price = property.Price,
                Description = property.Description,
                ImagePath = property.Image
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PropertyViewModel model)
        {
            if (id != model.PropertyId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            property.PropertyName = model.PropertyName;
            property.Title = model.Title;
            property.Price = model.Price;
            property.Description = model.Description;

            if (model.ImageFile != null)
            {
                var fileName = Path.GetFileName(model.ImageFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                property.Image = "/images/" + uniqueFileName;
            }

            try
            {
                _context.Update(property);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the property.");
                Console.WriteLine(ex.Message);
                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int PropertyId)
        {
            var property = await _context.Properties.FindAsync(PropertyId);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> PropertyIds)
        {
            foreach (var PropertyId in PropertyIds)
            {
                var property = await _context.Properties.FindAsync(PropertyId);
                if (property != null)
                {
                    _context.Properties.Remove(property);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



    }
}
