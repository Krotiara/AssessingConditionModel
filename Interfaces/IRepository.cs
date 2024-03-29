﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{ 
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        public IEnumerable<TEntity> GetAll();

        Task<TEntity> AddAsync(TEntity entity);

        Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task UpdateRangeAsync(List<TEntity> entities);

        Task DeleteAsync(TEntity entity);
    }
}
