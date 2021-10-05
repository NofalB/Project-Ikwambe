using Domain;
using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CosmosRepository<TEntity> : ICosmosRepository<TEntity> where TEntity : class, new()
    {

        protected readonly IkwambeContext _ikambeContext;

        public CosmosRepository(IkwambeContext ikambeContext)
        {
            _ikambeContext = ikambeContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _ikambeContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Entity must not be null.");
                }

                await _ikambeContext.AddAsync(entity);
                await _ikambeContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting a new record of {nameof(entity)}. {ex.Message}");
            }
        }

        public TEntity Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Entity must not be null.");
                }

                _ikambeContext.Update(entity);
                _ikambeContext.SaveChanges();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating {nameof(entity)}. {ex.Message}");
            }
        }

        public void Delete(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("Entity must not be null.");
                }

                _ikambeContext.Remove(entity);
                _ikambeContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting {nameof(entity)}. {ex.Message}");
            }
        }
    }
}
