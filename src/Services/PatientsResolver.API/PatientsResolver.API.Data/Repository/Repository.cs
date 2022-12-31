﻿using Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsResolver.API.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {

        //protected PatientsDataDbContext PatientsDataDbContext => dbContextFactory.CreateDbContext();
        private readonly IDbContextFactory<PatientsDataDbContext> dbContextFactory;
        protected readonly PatientsDataDbContext dbContext;

        public Repository(IDbContextFactory<PatientsDataDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
            this.dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                if (entity == null)
                {
                    throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
                }

                try
                {
                    await dbContext.AddAsync(entity);
                    await dbContext.SaveChangesAsync();

                    return entity;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{nameof(entity)} could not be saved {ex.Message}", ex);
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

        public async Task DeleteAsync(TEntity entity)
        {      
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();          
        }

        public IEnumerable<TEntity> GetAll()
        {
            try
            { 
                return dbContext.Set<TEntity>();        
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
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync();            

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
                dbContext.UpdateRange(entities);
                await dbContext.SaveChangesAsync();               
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be updated {ex.Message}");
            }
        }
    }
}
