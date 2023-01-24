using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Identity;

namespace Booking.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> BookingToggle(int id)
        {
            if (id == null) return NotFound();
            var gymClassId = id;
            var userId = userManager.GetUserId(User);


            var booked = await _context.ApplicationUserGymClass.FirstOrDefaultAsync(c => c.GymClassId == gymClassId && c.ApplicationUserId == userId);

            if (booked == null)
            {
                var applicationUserGymClass = new ApplicationUserGymClass();
                applicationUserGymClass.GymClassId = gymClassId;
                applicationUserGymClass.ApplicationUserId = userId;

                _context.Add(applicationUserGymClass);
                await _context.SaveChangesAsync();
            }
            else
            {
                var gymClass = await _context.ApplicationUserGymClass.FirstOrDefaultAsync(au => au.GymClassId == gymClassId && au.ApplicationUserId == userId);
                if (gymClass != null)
                {
                    _context.ApplicationUserGymClass.Remove(gymClass);
                    await _context.SaveChangesAsync();


                }
            }


            return RedirectToAction(nameof(Index));


        }

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);

            var GymClassListwithUserBookings = await _context.GymClass.Include(g => g.ApplicationUserGymClasses).ToListAsync();


            if (_context.GymClass == null) { Problem("Entity set 'ApplicationDbContext.GymClass'  is null."); }

            return View(GymClassListwithUserBookings);


            //return _context.GymClass != null ? 
            //              View(await _context.GymClass.ToListAsync()) :
            //              Problem("Entity set 'ApplicationDbContext.GymClass'  is null.");
        }

        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GymClass == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GymClass == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GymClass == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GymClass == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GymClass'  is null.");
            }
            var gymClass = await _context.GymClass.FindAsync(id);
            if (gymClass != null)
            {
                _context.GymClass.Remove(gymClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
          return (_context.GymClass?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
