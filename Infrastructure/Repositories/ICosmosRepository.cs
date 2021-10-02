﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ICosmosRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetById(string entityId);

        Task AddAsync(TEntity entity);

        TEntity Update(TEntity entity);

        void Delete(TEntity entity);

    }
}