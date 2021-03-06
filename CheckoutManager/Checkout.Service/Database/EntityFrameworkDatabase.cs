﻿using Checkout.Service.Database;
using Microsoft.EntityFrameworkCore;
using Service.Common.Repository.Database;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Checkout.Service.Database
{
    public class EntityFrameworkDatabase<T> : IDatabase<T> where T : class
    {
        private DbSet<T> _objectSet;
        private PaymentDbContext _dbContext;
        public EntityFrameworkDatabase(PaymentDbContext dbContext)
        {
            _dbContext = dbContext;
            _objectSet = _dbContext.Set<T>();
        }

        public T Create(T entity)
        {
            _objectSet.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _objectSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public T GetByCriteria(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _objectSet.FirstOrDefault(predicate);
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
                throw;
            }
        }

        public async Task<T> GetByCriteriaAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _objectSet.FirstOrDefaultAsync(predicate);
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine(exception);
                throw;
            }
        }

        public T Update(T entity, string id)
        {
            _objectSet.Update(entity);
            _dbContext.SaveChanges();
            return entity;
        }


        public void Delete(T Entity)
        {
            var result = _objectSet.Remove(Entity);
            if (result.State != EntityState.Deleted)
                throw new Exception("The registry cannot be deleted");
            _dbContext.SaveChanges();
        }


        public IQueryable<T> ListAll()
        {
            return _objectSet;
        }
    }
}
