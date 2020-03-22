using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data.SqlClient;

namespace JST.TPLMS.Contract
{
    /// <summary>
    /// 业务操作的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>where T:class
    {
        T FindSingle(Expression<Func<T, bool>> exp = null);
        bool IsExist(Expression<Func<T, bool>> exp = null);
        IQueryable<T> Find(Expression<Func<T, bool>> exp = null);
        IQueryable<T> Find(int pageindex = 1, int pagesize = 10, Expression<Func<T, bool>> exp = null);
        int GetCount(Expression<Func<T, bool>> exp = null);
        void Add(T entity);
        void BatchAdd(T[] entities);
        /// <summary>
        /// 更新一个实体的所有属性
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        void Delete(T entity);
        void Save();
        string GetNo(string name, int OrgId);
        int ExecProcedure(string sp, params SqlParameter[] parameters);
    }
}
