using System;
using RemindersManagement.API.Infrastructure.Data;

namespace RemindersManagement.API.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly RemindersDbContext _context;

        public BaseRepository(RemindersDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
