using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using Utility;
using DAL;

namespace DAL
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class,new()
    {
        public DbContext Context;

        protected RepositoryBase(db_Zon_HuwEntities context)
        {
            try
            {
                if (context == null) throw new ArgumentNullException("context");
                Context = context;
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        protected RepositoryBase()
        {
            try
            {
                Context = new db_Zon_HuwEntities();
            }
            catch (Exception ex)
            {
//Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public int Count()
        {
            try
            {
                return Context.Set<T>().Count();
            }
            catch (Exception ex)
            {
               // Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public int Count(Expression<Func<T, bool>> IsSold)
        {
            try
            {
                return Context.Set<T>().Count(IsSold);
            }
            catch (Exception ex)
            {
               // Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public T Create()
        {
            try
            {

                return Context.Set<T>().Create();
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public T Update(T entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                Context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
               // Utility.// Shared.Log.Error(ex);
                throw;
            }
        }


        public ICollection<T> Update(ICollection<T> entity)
        {
            try
            {
                foreach (var a in entity)
                {
                    if (Context.Entry(a).State == EntityState.Detached)
                    {
                        Context.Set<T>().Add(a);
                    }
                    else
                    {
                        Context.Entry(a).State = EntityState.Modified;
                    }
                }

                Context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public T Insert(T entity)
        {
            try
            {

                Context.Set<T>().Add(entity);

                Context.SaveChanges();
                return entity;
            }

            catch (System.Data.SqlTypes.SqlTypeException sdex)
            {
                throw sdex;
            }

            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public ICollection<T> Insert(ICollection<T> entity)
        {
            try
            {
                foreach (var a in entity)
                    Context.Set<T>().Add(a);

                Context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                // Context.Entry(entity).State = System.Data.EntityState.Deleted;
                Context.Set<T>().Remove(entity);

                Context.SaveChanges();
            }
            catch (Exception ex)
            {
               // Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var records = Context.Set<T>().Where(predicate).ToList();
                foreach (T record in records)
                {
                    Context.Set<T>().Remove(record);
                }
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public T Find(params object[] keyValues)
        {
            try
            {
                return Context.Set<T>().Find(keyValues);
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return Context.Set<T>().Where(predicate);
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return Context.Set<T>().Where(predicate).FirstOrDefault();
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return Context.Set<T>().Where(predicate).SingleOrDefault();
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public List<T> FetchAllByList()
        {
            try
            {
                return Context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
             //   Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public IEnumerable<T> FetchAllByIEnumerable()
        {
            try
            {
                return FetchAllByList().AsEnumerable();
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public IQueryable<T> FetchAllByIQueryable()
        {
            try
            {
                return Context.Set<T>();
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public List<T> FetchAllByPage(Func<T, object> keySelector, out int totalRecordsForPaging, int skipRows = 0, int nextRows = 0, Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> columns = null, string includeProperties = "", bool desc = false)
        {
            try
            {
                totalRecordsForPaging = 0;
                IQueryable<T> query = Context.Set<T>();

                //if (columns != null)
                //    query = (IQueryable<T>) query.Select (columns);

                if (filter != null)
                    query = query.Where(filter);

                if (skipRows == 0 && nextRows == 0)
                    return desc ? query.OrderByDescending(keySelector).ToList() : query.OrderBy(keySelector).ToList();

                totalRecordsForPaging = query.OrderByDescending(keySelector).Count();

                if (includeProperties != "")
                    query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                return desc ? query.OrderByDescending(keySelector).Skip(skipRows).Take(nextRows).ToList() : query.OrderBy(keySelector).Skip(skipRows).Take(nextRows).ToList();
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public IEnumerable<T> GetAllOrderBy(Func<T, object> keySelector)
        {
            try
            {
                return Context.Set<T>().OrderBy(keySelector).ToList();
            }
            catch (Exception ex)
            {
               // Utility.// Shared.Log.Error(ex);
                throw;
            }
        }

        public IEnumerable<T> GetAllOrderByDescending(Func<T, object> keySelector)
        {
            try
            {
                return Context.Set<T>().OrderByDescending(keySelector).ToList();
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }


        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            try
            {
                IQueryable<T> query = Context.Set<T>();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                return orderBy != null ? orderBy(query).ToList() : query.ToList();
            }
            catch (Exception ex)
            {
              //  Utility.// Shared.Log.Error(ex);
                throw;
            }
        }


    }
}