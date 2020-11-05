using System.Threading.Tasks;
using RemindersManagement.API.Domain.Interfaces;
using RemindersManagement.API.Infrastructure.Repositories;

namespace RemindersManagement.API.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RemindersDbContext _context;
        private IRemindersRepository _remindersRepository;
        private ICategoriesRepository _categoriesRepository;

        public UnitOfWork(RemindersDbContext context)
        {
            _context = context;
        }

        public IRemindersRepository RemindersRepository
        {
            get
            {
                return _remindersRepository = _remindersRepository ?? new RemindersRepository(_context);
            }
        }

        public ICategoriesRepository ListsRepository
        {
            get
            {
                return _categoriesRepository = _categoriesRepository ?? new CategoriesRepository(_context);
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RollbackAsync()
        {
            await _context.DisposeAsync();
        }

    }
}
