using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIkwambe.Repositories
{
    public interface ICosmosRepository
    {
        IEnumerable<T> GetAll();

        Task AddAsync<T>(T entity);

        void Update<T>(T entity);

        void Delete<T>(T entity);

    }
}
