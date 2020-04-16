using MessageBoard.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MessageBoard.DataAccess.Base
{
	public interface IGenericRepository<T>
	{
		bool Insert(T obj, bool save = true);

		bool Update(T obj, bool save = true);

		int Save();

		bool Delete(T obj, bool save = true);

		bool DeleteRange(Expression<Func<T, bool>> filter, bool save = true);

		bool Any(Expression<Func<T, bool>> filter = null);

		int Count(Expression<Func<T, bool>> filter = null);

		int Max(Expression<Func<T, int>> filter);

		int Min(Expression<Func<T, int>> filter);

		T GetById(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);

		IQueryable<T> GetAll(
			Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);

		T FirstOrDefault(
			Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null);
	}

	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		internal MessageContext context;
		internal DbSet<T> dbSet;

		public GenericRepository(MessageContext context)
		{
			this.context = context;
			dbSet = context.Set<T>();
		}

		public virtual bool Insert(T obj, bool save = true)
		{
			dbSet.Add(obj);
			return save ? Save() > 0 : false;
		}

		public virtual bool Update(T obj, bool save = true)
		{
			if (context.Entry(obj).State == EntityState.Detached)
			{
				dbSet.Attach(obj);
			}
			context.Entry(obj).State = EntityState.Modified;
			return save ? Save() > 0 : false;
		}

		public virtual bool Delete(T obj, bool save = true)
		{
			context.Entry(obj).State = EntityState.Deleted;
			dbSet.Remove(obj);
			return save ? Save() > 0 : false;
		}

		public virtual bool DeleteRange(Expression<Func<T, bool>> filter, bool save = true)
		{
			var query = dbSet.Where(filter);
			dbSet.RemoveRange(query);
			return save ? Save() > 0 : false;
		}

		public virtual int Save()
		{
			return context.SaveChanges();
		}

		public virtual bool Any(Expression<Func<T, bool>> filter = null)
		{
			IQueryable<T> query = dbSet;
			return filter != null ? query.Any(filter) : query.Any();
		}

		public virtual int Count(Expression<Func<T, bool>> filter = null)
		{
			IQueryable<T> query = dbSet;
			return filter != null ? query.Count(filter) : query.Count();
		}

		public virtual int Max(Expression<Func<T, int>> filter)
		{
			IQueryable<T> query = dbSet;
			return query.AsQueryable().Max(filter);
		}

		public virtual int Min(Expression<Func<T, int>> filter)
		{
			IQueryable<T> query = dbSet;
			return query.AsQueryable().Min(filter);
		}

		public virtual T GetById(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
		{
			return GetAll(obj => ((GenericEntity)(object)obj).Id == id, includeProperties: includeProperties).FirstOrDefault();
		}

		public virtual IQueryable<T> GetAll(
			Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
			{
				query = includeProperties(query);
			}
			return OrderedQuery(orderBy, query);
		}

		public virtual T FirstOrDefault(Expression<Func<T, bool>> filter = null,
								Func<IQueryable<T>, IIncludableQueryable<T, object>> includeProperties = null)
		{
			return GetAll(filter, null, includeProperties).FirstOrDefault();
		}

		private IQueryable<T> OrderedQuery(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, IQueryable<T> query)
		{
			if (orderBy != null)
			{
				return orderBy(query);
			}
			else
			{
				return query;
			}
		}
	}
}
