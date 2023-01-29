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
using Booking.Models.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Collections.Immutable;
using NuGet.Versioning;
using Microsoft.Data.SqlClient;

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

        public async Task<IActionResult> BookingToggle(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { Area = "Identity" });


                
            }
            if (id == null) return BadRequest();
            
            var userId = userManager.GetUserId(User); //ramverket fixar med cookie innehåll (se program.cs) tillbaka till server
            if (userId == null) return BadRequest();


            var gymClassId = id;

            var booked = await _context.ApplicationUserGymClass.FirstOrDefaultAsync(c => c.GymClassId == gymClassId && c.ApplicationUserId == userId);

            //var attending = await _context.ApplicationUserGymClass.FindAsync(userId, id); //samma som ovan


            if (booked == null)
            {
                var applicationUserGymClass = new ApplicationUserGymClass();
                applicationUserGymClass.GymClassId = (int)gymClassId;  //cast pga int? id
                applicationUserGymClass.ApplicationUserId = userId;

                _context.Add(applicationUserGymClass);
                await _context.SaveChangesAsync();
            }
            else
            {
                var gymClass = await _context.ApplicationUserGymClass.FirstOrDefaultAsync(au => au.GymClassId == gymClassId && au.ApplicationUserId == userId);  //finns redan...?
                if (gymClass != null)
                {
                    _context.ApplicationUserGymClass.Remove(gymClass);
                    await _context.SaveChangesAsync();


                }
            }


            return RedirectToAction(nameof(Index));


        }

        // GET: GymClasses
        public async Task<IActionResult> Index(bool showAll = false)
        {
            var userId = userManager.GetUserId(User);

            var GymClassListwithUserBookings = new List<GymClass>();

            ViewData["showAll"] = !showAll;

            if (showAll == false) { 

                GymClassListwithUserBookings = await _context.GymClass
                    .Include(a => a.ApplicationUserGymClasses).ToListAsync();
            }
            else
            {
                GymClassListwithUserBookings = await _context.GymClass
                    .Include(a => a.ApplicationUserGymClasses).Where(i => i.StartTime> DateTime.Now).ToListAsync();
            }

            var viewModel = GymClassListwithUserBookings
                .Select(s => new GymClassWithUsersViewModel
                {
                    
                    attending = s.ApplicationUserGymClasses.Any(ag => ag.ApplicationUserId == userId),
                    
                    GymClassId = s.Id,
                    GymClassName = s.Name,
                    StartTime = s.StartTime,
                    Duration = s.Duration,
                    Description = s.Description
                    
                })
                .ToList();

            

            



            //viewmodel
            // .select
            // boolean i vyn... 2 sätt: viewmodel eller injection i vyn

            //await _context.GymClass
            //.Include(ga => ga.ApplicationUserGymClasses)
            // .ThenInclude(a => a.ApplicationUser)

            //.ToListAsync();


            if (_context.GymClass == null) { Problem("Entity set 'ApplicationDbContext.GymClass'  is null."); }

            return View(viewModel);


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

            //var listAttending = await _context.GymClass
            //    .Include(g => g.ApplicationUserGymClasses)
            //        .ThenInclude(a => a.ApplicationUser)
            //    .Where(i => i.Id==id)
            //    .ToListAsync();

            var detailsWithAttendants = await _context.GymClass
                .Include(au => au.ApplicationUserGymClasses)
                    .ThenInclude(g => g.ApplicationUser)
                .Where(i => i.Id== id).FirstOrDefaultAsync();




            //var s = await _context.GymClass
            //    .FirstOrDefaultAsync(m => m.Id == id);


            if (detailsWithAttendants == null)
            {
                return NotFound();
            }

            if (detailsWithAttendants.ApplicationUserGymClasses == null)
            {

            }


            var viewModel = new DetailsViewModel
            {
                GymClassId = detailsWithAttendants.Id,
                Description = detailsWithAttendants.Description,
                Duration = detailsWithAttendants.Duration,
                StartTime = detailsWithAttendants.StartTime,

                GymClassName = detailsWithAttendants.Name,

                Attendants = detailsWithAttendants.ApplicationUserGymClasses.ToList(),
            };




            return View(viewModel);


        }
        [Authorize(Roles = "Admin")]

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

        [Authorize(Roles = "Admin")]

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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
