using Domain;
using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CosmosWriteRepository<TEntity> : ICosmosWriteRepository<TEntity> where TEntity : class, new()
    {

        protected readonly IkwambeContext _ikambeContext;

        public CosmosWriteRepository(IkwambeContext ikambeContext)
        {
            _ikambeContext = ikambeContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            await _ikambeContext.AddAsync(entity);
            await _ikambeContext.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            _ikambeContext.Update(entity);
            await _ikambeContext.SaveChangesAsync();

            return entity;
        }

        public async Task Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            _ikambeContext.Remove(entity);
            await _ikambeContext.SaveChangesAsync();
        }
    }
}
