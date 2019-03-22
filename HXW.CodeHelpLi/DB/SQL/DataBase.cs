using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using HXW.CodeHelpLi;

namespace HXW.CodeHelpLi.DB
{
    /// <summary>
    /// 数据库抽象类
    /// </summary>
    public abstract class DataBase
    {
        /// <summary>
        /// 数据库连接字符串的默认节点名称
        /// </summary>
        public const string DEFAULT_CONNECTIONSTRING_SECTION_NAME = "ConnectionString";
        public static string STATE_CONNECTIONSTRING = string.Empty;

        protected DataBase(DbProviderFactory databaseFactory)
        {
            //string connect = "Data Source=47.96.190.189;Initial Catalog=HomeManage_20180507;Persist Security Info=True;User ID=sa;Password=Storemax001;Pooling=False";
            //string connect = "Data Source=xuyang7606.xicp.net,2466;Initial Catalog=HomeManage;Persist Security Info=True;User ID=XuHaoFengII;Password=adminxy#12345^&*();Pooling=False";
            //string jiami = EncryptUtil.Encrypt(connect);
            if (string.IsNullOrEmpty(STATE_CONNECTIONSTRING))
            {
                string jiemi = EncryptUtil.Decrypt(ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTIONSTRING_SECTION_NAME].ConnectionString);
                STATE_CONNECTIONSTRING = jiemi;
                this.ConnectionString = jiemi;
            }
            else
            {
                this.ConnectionString = STATE_CONNECTIONSTRING;
            }
            this.DatabaseFactory = databaseFactory;
        }

        protected DataBase(string connectionString, DbProviderFactory databaseFactory)
        {
            this.ConnectionString = connectionString;
            this.DatabaseFactory = databaseFactory;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库提供者工厂类
        /// </summary>
        public DbProviderFactory DatabaseFactory
        {
            get;
            set;
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns>DbConnection</returns>
        private DbConnection CreateConnection()
        {
            DbConnection connection = this.DatabaseFactory.CreateConnection();
            connection.ConnectionString = ConnectionString;
            return connection;
        }

        /// <summary>
        /// 创建数据库命令
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns>DbCommand</returns>
        private DbCommand CreateCommand(DbConnection connection)
        {
            DbCommand command = this.DatabaseFactory.CreateCommand();

            if (connection == null)
            {
                command.Connection = CreateConnection();
            }
            else
            {
                command.Connection = connection;
            }

            return command;
        }

        /// <summary>
        /// 创建数据库命令
        /// </summary>
        /// <returns>DbCommand</returns>
        private DbCommand CreateCommand()
        {
            return CreateCommand(null);
        }

        /// <summary>
        /// 创建数据库适配器
        /// </summary>
        /// <returns></returns>
        private DbDataAdapter CreateDataAdapter()
        {
            DbDataAdapter adapter = this.DatabaseFactory.CreateDataAdapter();
            adapter.SelectCommand = CreateCommand();
            return adapter;
        }


        public object ExecuteScalar(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            try
            {
                DbCommand command = CreateCommand();
                command.CommandText = sql;
                command.CommandType = commandType;
                command.Connection.Open();

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                object value = command.ExecuteScalar();
                command.Connection.Close();
                command.Parameters.Clear();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public object ExecuteScalar(string sql, params DbParameter[] parameters)
        {
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }


        public DbDataReader ExecuteDataReader(string sql, params DbParameter[] parameters)
        {
            try
            {
                DbCommand command = CreateCommand();
                command.CommandText = sql;
                command.Connection.Open();

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                DbDataReader dtr = command.ExecuteReader(CommandBehavior.CloseConnection);
                command.Parameters.Clear();
                return dtr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet ExecuteDataSet(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            try
            {
                DbDataAdapter adapter = CreateDataAdapter();

                adapter.SelectCommand.CommandType = commandType;
                adapter.SelectCommand.CommandText = sql;

                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                }

                DataSet dst = new DataSet();
                adapter.Fill(dst);
                adapter.SelectCommand.Parameters.Clear();
                return dst;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet ExecuteDataSet(string sql, params DbParameter[] parameters)
        {
            return ExecuteDataSet(sql, CommandType.Text, parameters);
        }

        public DataTable ExecuteDataTable(string sql, CommandType commandType, params DbParameter[] parameters)
        {
            DataSet dst = ExecuteDataSet(sql, commandType, parameters);
            if (dst == null) return null;
            return dst.Tables[0];
        }


        public DataTable ExecuteDataTable(string sql, params DbParameter[] parameters)
        {
            return ExecuteDataTable(sql, CommandType.Text, parameters);
        }


        public int ExecuteNonQuery(string sql, params DbParameter[] parameters)
        {
            int count = 0;
            try
            {
                DbCommand command = CreateCommand();
                command.CommandText = sql;
                command.Connection.Open();

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                count = command.ExecuteNonQuery();
                command.Parameters.Clear();
                command.Connection.Close();
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return count;
        }

        public bool ExecuteSomeSqls(List<SqlItem> sqlItems)
        {
            DbConnection connection = CreateConnection();
            try
            {
                connection.Open();
                DbCommand command = null;
                foreach (SqlItem item in sqlItems)
                {
                    command = CreateCommand(connection);
                    item.InitCommand(command);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool ExecuteSomeSqlsWithTran(List<SqlItem> sqlItems)
        {
            DbConnection connection = CreateConnection();

            connection.Open();
            DbTransaction transaction = connection.BeginTransaction();

            try
            {
                DbCommand command = null;

                foreach (SqlItem item in sqlItems)
                {
                    command = CreateCommand(connection);
                    item.InitCommand(command);
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
                transaction.Commit();
                return true;

            }
            catch (Exception ex)
            {
                transaction.Rollback();

                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool ExecuteSomeSqlsWithTranEx(List<SqlItem> sqlItems)
        {
            bool flag = true;
            DbConnection connection = CreateConnection();

            connection.Open();
            DbTransaction transaction = connection.BeginTransaction();

            try
            {
                DbCommand command = null;

                foreach (SqlItem item in sqlItems)
                {
                    command = CreateCommand(connection);
                    item.InitCommand(command);
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                flag = false;
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>
        /// <returns>执行事务影响的行数</returns>
        public int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (SqlConnection conntion = new SqlConnection(EncryptUtil.Decrypt(ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTIONSTRING_SECTION_NAME].ConnectionString)))
            {
                conntion.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conntion;

                SqlTransaction ts = conntion.BeginTransaction();
                cmd.Transaction = ts;

                int count = 0;
                try
                {

                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n];
                        if (strsql.Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    ts.Commit();//提交数据库事务
                    return count;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 带事务的执行SQL语句，返回影响的记录数，select类型的语句此方法不可行。
        /// 对于select方法应该通过Dataset.Tables[0].Rows.Count来判断
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSqlTran(string SQLString)
        {
            using (SqlConnection conntion = new SqlConnection(EncryptUtil.Decrypt(ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTIONSTRING_SECTION_NAME].ConnectionString)))
            {
                conntion.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conntion;

                SqlTransaction ts = conntion.BeginTransaction();
                cmd.Transaction = ts;
                try
                {
                    int count = 0;
                    string strsql = SQLString;
                    if (strsql.Length > 1)
                    {
                        cmd.CommandText = strsql;
                        count = cmd.ExecuteNonQuery();
                    }
                    ts.Commit();//提交数据库事务
                    return count;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    ts.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }


        /// <summary>
        ///执行查询，并将查询返回的结果集中第一行的第一列作为 .NET Framework 数据类型返回。忽略额外的列或行。返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(EncryptUtil.Decrypt(ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTIONSTRING_SECTION_NAME].ConnectionString)))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
    }
}
