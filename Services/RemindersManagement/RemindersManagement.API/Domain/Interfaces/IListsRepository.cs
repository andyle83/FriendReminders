using RemindersManagement.API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindersManagement.API.Domain.Interfaces
{
    public interface ICategoriesRepository
    {
        public Task<IEnumerable<Category>> ListAsync();

        public Task<Category> FindAsync(Guid id);

        public Task<Category> AddAsync(Category list);

        public void Update(Category list);

        public void Remove(Category list);
    }
}