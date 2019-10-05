using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyVet.Web.Data;
using MyVet.Web.Data.Entities;
using MyVet.Web.Helpers;
using MyVet.Web.Models;

namespace MyVet.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;

        public HomeController(
            DataContext dataContext,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Authorize(Roles = "Customer")]
        public IActionResult MyPets()
        {
            return View(_dataContext.Pets
                .Include(p => p.PetType)
                .Include(p => p.Histories)
                .Where(p => p.Owner.User.Email.ToLower().Equals(User.Identity.Name.ToLower())));
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _dataContext.Pets
                .Include(p => p.Owner)
                .Include(p => p.PetType)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            var model = new PetViewModel
            {
                Born = pet.Born,
                Id = pet.Id,
                ImageUrl = pet.ImageUrl,
                Name = pet.Name,
                OwnerId = pet.Owner.Id,
                PetTypeId = pet.PetType.Id,
                PetTypes = _combosHelper.GetComboPetTypes(),
                Race = pet.Race,
                Remarks = pet.Remarks
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = model.ImageUrl;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Pets",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Pets/{file}";
                }

                var pet = new Pet
                {
                    Born = model.Born,
                    Id = model.Id,
                    ImageUrl = path,
                    Name = model.Name,
                    Owner = await _dataContext.Owners.FindAsync(model.OwnerId),
                    PetType = await _dataContext.PetTypes.FindAsync(model.PetTypeId),
                    Race = model.Race,
                    Remarks = model.Remarks
                };

                _dataContext.Pets.Update(pet);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(MyPets));
            }

            model.PetTypes = _combosHelper.GetComboPetTypes();
            return View(model);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _dataContext.Pets
                .Include(p => p.Owner)
                .ThenInclude(o => o.User)
                .Include(p => p.Histories)
                .ThenInclude(h => h.ServiceType)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _dataContext.Pets
                .Include(p => p.Histories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }

            if (pet.Histories.Count > 0)
            {
                return RedirectToAction(nameof(MyPets));
            }

            _dataContext.Pets.Remove(pet);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(MyPets));
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create()
        {
            var owner = await _dataContext.Owners
                .FirstOrDefaultAsync(o => o.User.Email.ToLower().Equals(User.Identity.Name.ToLower()));
            if (owner == null)
            {
                return NotFound();
            }

            var model = new PetViewModel
            {
                Born = DateTime.Now,
                PetTypes = _combosHelper.GetComboPetTypes(),
                OwnerId = owner.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Pets",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Pets/{file}";
                }

                var pet = new Pet
                {
                    Born = model.Born,
                    ImageUrl = path,
                    Name = model.Name,
                    Owner = await _dataContext.Owners.FindAsync(model.OwnerId),
                    PetType = await _dataContext.PetTypes.FindAsync(model.PetTypeId),
                    Race = model.Race,
                    Remarks = model.Remarks
                };

                _dataContext.Pets.Add(pet);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(MyPets)}");
            }

            model.PetTypes = _combosHelper.GetComboPetTypes();
            return View(model);
        }

    }
}
