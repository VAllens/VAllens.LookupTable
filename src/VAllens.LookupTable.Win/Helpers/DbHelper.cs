using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dapper;

namespace VAllens.LookupTable
{
    public class DbHelper
    {
        private readonly string _connectionStrings;

        public DbHelper(string connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        /// <summary>
        /// 根据SQL查询语句从数据库获取数据
        /// </summary>
        public virtual IEnumerable<T> GetData<T>(string sql)
        {
            IEnumerable<T> result;
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");

            IDbConnection conn = dbFactory.CreateConnection();
            if (conn == null)
            {
                throw new NullReferenceException("请检查数据访问提供程序的完全限定名是否正确，或有无提供相关dll");
            }

            using (conn)
            {
                conn.ConnectionString = _connectionStrings;
                result = conn.Query<T>(sql);
            }

            return result;
        }

        /// <summary>
        /// 数据库连接测试
        /// </summary>
        public virtual void TestDbConnection()
        {
            if (string.IsNullOrEmpty(_connectionStrings))
            {
                throw new Exception("请填写数据库连接字符串！");
            }

            var dbFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            DbConnectionStringBuilder connectionStringBuilder = dbFactory.CreateConnectionStringBuilder();
            IDbConnection conn = dbFactory.CreateConnection();

            if (connectionStringBuilder == null || conn == null)
            {
                throw new Exception("数据库连接提供程序未实现！");
            }

            try
            {
                connectionStringBuilder.ConnectionString = _connectionStrings;
            }
            catch (Exception ex)
            {
                throw new Exception("数据库连接字符串格式不正确！异常: " + ex.Message);
            }

            try
            {
                using (conn)
                {
                    conn.ConnectionString = connectionStringBuilder.ConnectionString;
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("连接测试失败！异常: " + ex.Message);
            }
        }
    }
}