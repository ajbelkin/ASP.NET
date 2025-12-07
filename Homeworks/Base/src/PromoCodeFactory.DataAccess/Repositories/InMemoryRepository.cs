using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>: IRepository<T> where T: BaseEntity
    {
        protected ConcurrentDictionary<Guid, T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = new ConcurrentDictionary<Guid, T>(data.ToDictionary(k => k.Id));
        }

        /// <inheritdoc />
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data.Values.Select(it => it));
        }

        /// <inheritdoc />
        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.GetValueOrDefault(id));
        }

        /// <inheritdoc />
        public Task<T> CreateAsync(T item)
        {
            if (Data.TryAdd(item.Id, item))
            {
                return Task.FromResult(item);
            }

            throw new ArgumentException($"An employee with key {item.Id} already exists");

        }

        /// <inheritdoc />
        public Task<T> UpdateAsync(T item)
        {
            if (!Data.ContainsKey(item.Id))
            {
                throw new ArgumentException($"An employee with key {item.Id} not exists");
            }

            Data[item.Id] = item;
            return Task.FromResult(item);
        }

        /// <inheritdoc />
        public Task<T> UpsertAsync(T item)
        {
            return Task.FromResult(Data.AddOrUpdate(item.Id, item, (id, old) => item));
        }

        /// <inheritdoc />
        public Task<bool> DeleteAsync(Guid id)
        {
            return Task.FromResult(Data.Remove(id, out _));
        }
    }
}