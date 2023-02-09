using Booking.Core.Entities;
using Booking.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Data.Repositories
{
    public class UnitOfWork 
    {
        private readonly ApplicationDbContext db;

        
        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            
        }
        public GymClassRepository gymClassRepository { get; set; }

        
        public ApplicationUserGymClassRepository applicationUserGymClassRepository { get; private set; }

        


        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
