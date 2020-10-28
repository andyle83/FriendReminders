using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemindersManagement.API.Domain.Interfaces;
using RemindersManagement.API.Domain.Models;
using RemindersManagement.API.Infrastructure.Data;

namespace RemindersManagement.API.Infrastructure.Repositories
{
    public class CategoriesRepository : BaseRepository, ICategoriesRepository
    {
        public CategoriesRepository(RemindersDbContext context) : base(context)
        {
        }

        public Task<Category> AddAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> FindAsync(Guid id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(l => l.Id == id);

            return category;
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }

        public void Remove(Category list)
        {
            _context.Categories.Remove(list);
        }

        public void Update(Category list)
        {
            _context.Categories.Update(list);
        }
    }
}
