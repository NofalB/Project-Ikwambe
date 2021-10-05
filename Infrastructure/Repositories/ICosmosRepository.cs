using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ICosmosRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();

        Task AddAsync(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TEntity entity);

    }
}
