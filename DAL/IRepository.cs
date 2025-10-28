using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL
{
    public interface IRepository<T> where T : class, new()
    {
        int Count();
        T Create();
        T Update(T entity);
      //  T Update(T entity, params Object[] pkey);
        T Insert(T entity);
        ICollection<T> Insert(ICollection<T> entity);
        void Delete(T entity);
        T Find(params object[] keyValues);
        List<T> FetchAllByList();
        IQueryable<T> FetchAllByIQueryable();
        IEnumerable<T> FetchAllByIEnumerable();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T Single(Expression<Func<T, bool>> predicate);
        T First(Expression<Func<T, bool>> predicate);

        List<T> FetchAllByPage(Func<T, object> keySelector, out int totalRecordsForPaging, int page = 0, int rows = 0,
                               Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> columns = null,
                               string includeProperties = "",bool desc=true);
        IEnumerable<T> GetAllOrderBy(Func<T, object> keySelector);
        IEnumerable<T> GetAllOrderByDescending(Func<T, object> keySelector);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

    }

}



