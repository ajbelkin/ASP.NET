using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            using var context = new DataBaseContext();
            return await context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task AddAsync(T item)
        {
            using var context = new DataBaseContext();
            await context.Set<T>().AddAsync(item);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T item)
        {
            using var context = new DataBaseContext();
            context.Set<T>().Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var context = new DataBaseContext();
            var entities = await context.Set<T>().ToListAsync();
            return entities;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            using var context = new DataBaseContext();
            return await context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(T item)
        {
            using var context = new DataBaseContext();
            context.Set<T>().Update(item);
            await context.SaveChangesAsync();
        }
    }
}
