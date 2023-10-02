using EmersonAPI.Interfaces;
using EmersonDB;
using Microsoft.EntityFrameworkCore;

namespace EmersonAPI.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T: class
    {
        protected readonly EmersonContext _dbContext;
        public GenericRepository(EmersonContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
            return  _dbContext.Set<T>();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}
