using Bulky.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal DbSet<T> dbSet;
        public AppDbContext _db;
        public Repository(AppDbContext db) {
            _db = db;
            dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id, string? include = null)
        {
            IQueryable<T> query = dbSet;

            
            if (!string.IsNullOrEmpty(include))
            {
                foreach (var includeProp in include.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            
            return query.FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
        }



        public IEnumerable<T> GetAll(string? include=null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(include))
            {
                foreach (var includeProp in include
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
