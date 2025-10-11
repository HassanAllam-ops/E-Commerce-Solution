using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Persistence.Data.Contexts;

namespace Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext _dbContext) : IUintOfWork
    {
        private readonly Dictionary<string , object> _repositories = new Dictionary<string, object>();
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;
            if(_repositories.ContainsKey(typeName))
            {
                return (IGenericRepository<TEntity, TKey>) _repositories[typeName]; 
            }
            // Create repo Object
            var repo = new GenericRepository<TEntity, TKey>(_dbContext);
            // Store Reference From repo Object
            _repositories[typeName] = repo;
            // Return Repo Object
            return repo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
