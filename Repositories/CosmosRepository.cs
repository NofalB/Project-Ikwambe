using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using ProjectIkwambe.CosmosDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIkwambe.Repositories
{
    public class CosmosRepository : ICosmosRepository
    {

        private readonly IkambeContext _ikambeContext;

        public CosmosRepository(IkambeContext ikambeContext)
        {
            _ikambeContext = ikambeContext;
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync<T>(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null.");
            }

            await _ikambeContext.AddAsync(entity);
            await _ikambeContext.SaveChangesAsync();
        }

        public void Update<T>(T entity)
        {
             _ikambeContext.Update(entity);
             _ikambeContext.SaveChanges();

        }

        public void Delete<T>(T entity)
        {
             _ikambeContext.Remove(entity);
             _ikambeContext.SaveChanges();
        }
    }
}
