using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XTabAPI.Domain;

namespace XTabAPI.Repository.IRepository
{
    public interface IRepository
    {
        //IEnumerable<T> GetAll();
        //T Single(Expression<Func<T, bool>> predicate);
        //IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);
        //IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        //IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        //bool Any(Expression<Func<T, bool>> predicate);
        //T First(Expression<Func<T, bool>> predicate);
        //T FirstOrDefault(Expression<Func<T, bool>> predicate);
        //T Last(Expression<Func<T, bool>> predicate);
        //void Add(T entity);
        //void Add(List<T> entityList);
        //void Update(T entity);
        //void AddOrUpdate(T entity);
        //void Delete(T entity);
        //void DeleteAll(List<T> entityList);
        //void Attach(T entity);
        //IQueryable<T> Table { get; }
        //IQueryable<T> GetIncluding(params Expression<Func<T, object>>[] includeProperties);
        DataSet ExecuteStoredProcedure(string procName, List<SQLParameter> parameterList, string databaseConnection);
        DataSet ExecuteQuery(string query, string databaseConnection);
    }
}
