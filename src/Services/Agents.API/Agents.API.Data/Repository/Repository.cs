﻿using Agents.API.Data.Database;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents.API.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {

        protected readonly AgentsDbContext AgentsDbContext;

        public Repository(AgentsDbContext agentsDbContext)
        {
            this.AgentsDbContext = agentsDbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            IExecutionStrategy strategy = AgentsDbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                if (entity == null)
                {
                    throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
                }

                try
                {
                    await AgentsDbContext.AddAsync(entity);
                    await AgentsDbContext.SaveChangesAsync();

                    return entity;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{nameof(entity)} could not be saved {ex.Message}");
                }
            });  
        }

        public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                try
                {
                    await AddAsync(entity);
                }
                catch(Exception ex)
                {
                    //TODO log
                    continue;
                }
            }
            return entities;
        }

        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                return AgentsDbContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                AgentsDbContext.Update(entity);
                await AgentsDbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated {ex.Message}");
            }
        }

        public async Task UpdateRangeAsync(List<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateRangeAsync)} entities must not be null");
            }

            try
            {
                AgentsDbContext.UpdateRange(entities);
                await AgentsDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be updated {ex.Message}");
            }
        }
    }
}
