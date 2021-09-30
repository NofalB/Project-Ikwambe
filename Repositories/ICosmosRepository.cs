using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIkwambe.Repositories
{
    public interface ICosmosRepository<TEntity> where TEntity : class, new()
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(string id);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

    }
}
