using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CosmosRepository<TEntity> : ICosmosRepository<TEntity> where TEntity : class, new()
    {

        private readonly IkwambeContext _ikambeContext;

        public CosmosRepository(IkwambeContext ikambeContext)
        {
            _ikambeContext = ikambeContext;
        }

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(string id)
        {
            throw new NotImplementedException();
        }

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
             _ikambeContext.Update(entity);
             _ikambeContext.SaveChanges();

        }

        public void Delete(TEntity entity)
        {
             _ikambeContext.Remove(entity);
             _ikambeContext.SaveChanges();
        }
    }
}
