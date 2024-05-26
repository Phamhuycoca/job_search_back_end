using job_search_be.Domain.BaseModel;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Context;
using job_search_be.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace job_search_be.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        private readonly job_search_DbContext _context;
        DbSet<T> _dbSet;
        public GenericRepository(job_search_DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        DateTime now = DateTime.Now;
        public List<T> GetAllData()
        {
            return _dbSet.Where(x => x.deletedAt == null).OrderByDescending(x => x.createdAt).ToList();
        }
        public T Create(T entity)
        {
            try
            {
                if (!_dbSet.Any(e => e == entity))
                {
                    entity.createdAt = DateTime.Today.AddDays(1).AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second);
                    _dbSet.Add(entity);
                    _context.SaveChanges();
                    return entity;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error in Create method: {ex.Message}");
                throw;
            }
        }

        public T Delete(Guid id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    entity.deletedAt = DateTime.Today.AddDays(1).AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second);
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();
                    return entity;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error in Delete method: {ex.Message}");
                throw;
            }
        }


        public T GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        /* public T Update(T entity)
         {
             if (!_dbSet.Any(e => e == entity))
             {
                 throw new ApiException(404, "Không tìm thấy thông tin");
             }
             entity.updatedAt = DateTime.Now;
             _context.Entry(entity).State = EntityState.Modified;
             try
             {
                 _context.SaveChanges();
                 return entity;

             }
             catch (DbUpdateConcurrencyException)
             {
                 throw;
             }
         }*/
        public T Update(T entity)
        {
            entity.updatedAt = DateTime.Now;
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var existingEntity = _dbSet.Find(GetKeyValues(entity).ToArray());

                if (existingEntity == null)
                {
                    throw new ApiException(404, "Không tìm thấy thông tin");
                }

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            try
            {
                _context.SaveChanges();
                return entity;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }


        private IEnumerable<object> GetKeyValues(T entity)
        {
            var keyProperties = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
            foreach (var property in keyProperties)
            {
                yield return property.PropertyInfo.GetValue(entity);
            }
        }

        public T GetByString(string id)
        {
            return _dbSet.Find(id);
        }

        public T DeleteByString(string id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    entity.deletedAt = DateTime.Today.AddDays(1).AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second);
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();
                    return entity;
                }
                return null;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Error in Delete method: {ex.Message}");
                throw;
            }
        }
    }
}
