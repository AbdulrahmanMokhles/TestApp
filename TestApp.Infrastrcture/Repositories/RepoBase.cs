using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Domain.Interfaces;
using TestApp.Infrastrcture.Data;

namespace TestApp.Infrastrcture.Repositories
{
    public class RepoBase<T> : IGenericRepo<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public Context DbContext { get; }
        public RepoBase(Context dbContext)
        {
            DbContext = dbContext;
            _dbSet = DbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var entities = _dbSet.AsQueryable();
            //var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
            //if (isDeletedProperty != null)
            //{
            //    entities = entities.Where(e => EF.Property<bool>(e, "IsDeleted") == false || EF.Property<bool>(e, "IsDeleted") == null);
            //}

            return await entities.ToListAsync();
        }
        public async Task<T> GetById(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            //var propertyInfo = entity?.GetType().GetProperty("IsDeleted");
            //if (propertyInfo.GetValue(entity) != null)
            //{
            //    bool isDeleted = (bool)propertyInfo.GetValue(entity);
            //    if (isDeleted)
            //    {
            //        return null;
            //    }
            //}

            return entity;
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            DbContext.Update(entity);
            await DbContext.SaveChangesAsync();
        }
        
        public async Task Delete(int id)
        {
            var entity = await GetById(id);

            _dbSet.Remove(entity);
            await DbContext.SaveChangesAsync();

        }
       

        
    }
}
