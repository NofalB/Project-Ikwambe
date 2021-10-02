using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public abstract class CosmosRepository<TEntity> : ICosmosRepository<TEntity> where TEntity : class, new()
    {

        protected readonly IkwambeContext _ikambeContext;

        public CosmosRepository(IkwambeContext ikambeContext)
        {
            _ikambeContext = ikambeContext;
        }

        public abstract IEnumerable<TEntity> GetAll();

        public abstract Task<TEntity> GetByIdAsync(string entityId);

        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            await _ikambeContext.AddAsync(entity);
            await _ikambeContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            _ikambeContext.Update(entity);
             _ikambeContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            _ikambeContext.Remove(entity);
            _ikambeContext.SaveChanges();
        }
    }
}
