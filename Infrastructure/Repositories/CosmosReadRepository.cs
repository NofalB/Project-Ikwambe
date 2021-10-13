using Domain;
using Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CosmosReadRepository<TEntity> : ICosmosReadRepository<TEntity> where TEntity : class, new()
    {

        protected readonly IkwambeContext _ikambeContext;

        public CosmosReadRepository(IkwambeContext ikambeContext)
        {
            _ikambeContext = ikambeContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            var result = _ikambeContext.Set<TEntity>();

            if (result == null)
            {
                throw new Exception($"Couldn't retrieve entities of type: {typeof(TEntity)}.");
            }

            return result;
        }
    }
}
