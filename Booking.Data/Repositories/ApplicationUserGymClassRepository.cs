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
    public class ApplicationUserGymClassRepository
    {
        private readonly ApplicationDbContext db;

        public ApplicationUserGymClassRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));

        }

        
        public async Task<ApplicationUserGymClass?> UserAttending(string userId, int gymClassId)
        {
            return await db.ApplicationUserGymClass.FindAsync(userId, gymClassId);
        }
        public async Task<ApplicationUserGymClass?> FindAsync(string userId, int gymClassId)
        {
            return await db.ApplicationUserGymClass.FindAsync(userId, gymClassId);
        }

        public void add(ApplicationUserGymClass applicationUserGymClass)
        {
            db.Add(applicationUserGymClass);
        }

        public void remove(ApplicationUserGymClass applicationUserGymClass)
        {
            db.Remove(applicationUserGymClass);
        }


    }
}
