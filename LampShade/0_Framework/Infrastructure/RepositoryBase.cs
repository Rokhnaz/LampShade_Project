using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace _0_Framework.Infrastructure
{
    public class RepositoryBase<Tkey,T>: IRepository<Tkey,T> where T : class
    {
        private readonly DbContext _Context;

        public RepositoryBase(DbContext context)
        {
            _Context = context; 
        }
        public T Get(Tkey id)
        {
            return _Context.Find<T>(id);
        }

        public void Create(T entity)
        {
            _Context.Add<T>(entity);
        }

        public List<T> GetAll()
        {
            return _Context.Set<T>().ToList();
        }

        public bool Exist(Expression<Func<T, bool>> expression)
        {
            return _Context.Set<T>().Any(expression);
        }

        public void SaveChanges()
        {
            _Context.SaveChanges();
        }
    }
}
