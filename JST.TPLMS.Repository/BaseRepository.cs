using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using JST.TPLMS.Contract;
using JST.TPLMS.DataBase;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace JST.TPLMS.Repository
{
    /// <summary>
    /// 基本数据库操作类
    /// </summary>
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected TPLMSDbContext Context;
        public BaseRepository(TPLMSDbContext m_Context)
        {
            Context = m_Context;
        }

        /// <summary>
        /// 根据过滤条件获取记录
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public IQueryable<T> Find(Expression<Func<T, bool>> exp = null)
        {
            return Filter(exp);
        }

        public bool IsExist(Expression<Func<T, bool>> exp = null)
        {
            return Context.Set<T>().Any(exp);
        }

        /// <summary>
        /// 查找单个对象
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public T FindSingle(Expression<Func<T, bool>> exp = null)
        {
            return Context.Set<T>().AsNoTracking().FirstOrDefault(exp);
        }

        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public IQueryable<T> Find(int pageindex, int pagesize, Expression<Func<T, bool>> exp = null)
        {
            if (pageindex < 1) pageindex = 1;
            return Filter(exp).Skip(pagesize * (pageindex - 1)).Take(pagesize);
        }

        /// <summary>
        /// 根据过滤条件获取记录数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> exp = null)
        {
            return Filter(exp).Count();
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Save();
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        public void BatchAdd(T[] entities)
        {
            Context.Set<T>().AddRange(entities);
            Save();
        }

        public void Update(T entity)
        {
            var entry = this.Context.Entry(entity);
            //todo:如果状态没有任何更改，则会报错
            //注释，使用下面的方式
            entry.State = EntityState.Modified;
            Save();
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
            Save();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public virtual void Delete(object[] entitys)
        {
            Context.RemoveRange(entitys);
            Save();
        }

        public IQueryable<T> Filter(Expression<Func<T, bool>> exp)
        {
            var dbSet = Context.Set<T>().AsQueryable();
            if (exp != null)
            {
                dbSet = dbSet.Where(exp);
            }
            return dbSet;
        }

        public virtual string GetNo(string name, int OrgId)
        {
            SqlParameter[] parameters = {
                new System.Data.SqlClient.SqlParameter("@Name",System.Data.SqlDbType.NVarChar,10),
                new System.Data.SqlClient.SqlParameter("@BH",System.Data.SqlDbType.NVarChar,30),
            };
            parameters[0].Value = name;
            parameters[1].Direction = System.Data.ParameterDirection.Output;
            //int numdata = Context.ExecuteNonQueryAsync("p_NextBH", parameters);
            int numdata = 1;  //ExecuteNonQueryAsyn找不到定义，这个方法先这样，以后使用时再处理
            string no = parameters[1].Value.ToString();
            if (numdata < 0)
            {
                no = string.Empty;     
            } 
            return no;                                                                                              
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual int ExecProcedure(string sp, params SqlParameter[] param)
        {
            int flag;
            try
            {
                flag = this.Context.Database.ExecuteSqlCommand(sp, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        //#region 网上找的
        ///// <summary>
        ///// 异步执行带有参数的存储过程方法  增删改操作以及返回带有输出的参数
        ///// </summary>
        ///// <param name="db"></param>
        ///// <param name="sql"></param>
        ///// <param name="sqlParams"></param>
        ///// <returns></returns>
        //public async static Task<int> ExecuteNonQueryAsync(this TPLMSDbContext db, string sql, SqlParameter[] sqlParams)
        //{
        //    int numint;
        //    var connection = db.Database.GetDbConnection();
        //    using (var cmd = connection.CreateCommand())
        //    {
        //        await db.Database.OpenConnectionAsync();
        //        cmd.CommandText = sql;
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        cmd.Parameters.AddRange(sqlParams);
        //        numint = await cmd.ExecuteNonQueryAsync();
        //    }

        //    return numint;
        //}
        //#endregion
    }
}
