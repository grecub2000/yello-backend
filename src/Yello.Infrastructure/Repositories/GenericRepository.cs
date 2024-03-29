﻿using Yello.Core.Interfaces.Repositories;
using Yello.Infrastructure.Context;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Yello.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _dbContext;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public virtual async Task<T> GetByIdAsync(int id)
        {
            var keyValues = new object[] { id };
            return await _dbContext.Set<T>().FindAsync(keyValues);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbContext.Set<T>();
        }

        //public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        //{
        //    var specificationResult = ApplySpecification(spec);
        //    return await specificationResult.ToListAsync();
        //}
        //public IQueryable<T> ListAsQueryable(ISpecification<T> spec)
        //{
        //    return ApplySpecification(spec);
        //}

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        public async Task AddRangeAsync(List<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateRangeAsync(List<T> entities)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<T> AddWithIdentityAsync(T entity)
        //{
        //    await _dbContext.Set<T>().AddAsync(entity);
        //    _dbContext.SaveChangesWithIdentityInsert<T>();
        //    return entity;
        //}

        public async Task<T> PatchAsync(int id, JsonPatchDocument<T> patchDocument)
        {
            var t = await GetByIdAsync(id);
            patchDocument.ApplyTo(t);
            await _dbContext.SaveChangesAsync();
            return t;
        }

        public async Task UpdateAsync(T entity)
        {
            //_dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<int> CountAsync(ISpecification<T> spec)
        //{
        //    var specificationResult = ApplySpecification(spec);
        //    return await specificationResult.CountAsync();
        //}

        //public async Task<T> FirstAsync(ISpecification<T> spec)
        //{
        //    var specificationResult = ApplySpecification(spec);
        //    return await specificationResult.FirstAsync();
        //}

        //public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        //{
        //    var specificationResult = ApplySpecification(spec);
        //    return await specificationResult.FirstOrDefaultAsync();
        //}

        //private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        //{
        //    var evaluator = new SpecificationEvaluator();
        //    return evaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        //}
    }
}
