using Booking.Core.Entities;
using Booking.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Data.Repositories
{
    public class GymClassRepository
    {
       

        private readonly ApplicationDbContext db;

        public GymClassRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));

        }

        public async Task<IEnumerable<GymClass>> GymClassListwithUserBookings(string userId)
        {
            return await db.GymClass
                .Include(a => a.ApplicationUserGymClasses)
                .Where(au=>au.ApplicationUserGymClasses
                .Any(i=>i.ApplicationUserId ==userId && i.GymClass.StartTime >=DateTime.Now))
                .ToListAsync();

            
        }
    }
}
