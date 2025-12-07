using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T> where T: BaseEntity
    {
        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns>Items collection</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get item by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Item</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Created item</returns>
        Task<T> CreateAsync(T item);

        /// <summary>
        /// Update existing item
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Updated item</returns>
        Task<T> UpdateAsync(T item);

        /// <summary>
        /// Create new item or update existing one
        /// </summary>
        /// <param name="item">Item</param>
        /// <returns>Upserted item</returns>
        Task<T> UpsertAsync(T item);

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>True if item deleted</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}