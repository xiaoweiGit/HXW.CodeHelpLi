using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace  HXW.CodeHelpLi
{
    /// <summary>
    /// 生成sql的方法
    /// </summary>
    public static class GeneraSqlHelper
    {


        /// <summary>
        /// 返回分页查询查询语句（Sql数据库）
        /// </summary>
        /// <param name="strDatasetsql">数据集</param>
        /// <param name="strWhere">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="curPage">当前索引页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public static string GetPagingSqlStr(string strDatasetsql, string strWhere, string orderBy, int curPage, int pageSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT T.* , ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderBy.Trim()))
            {
                strSql.Append("order by T." + orderBy);
            }
            else
            {
                strSql.Append("order by T.PROJECTCODE desc");
            }
            strSql.Append(")AS Row from " + strDatasetsql.ToString() + " T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE 1=1 " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", ((curPage - 1) * pageSize + 1), (curPage * pageSize));
            return strSql.ToString();
        }

        /// <summary>
        /// 返回分页查询查询语句（Sql数据库）
        /// </summary>
        /// <param name="strDatasetsql">数据集</param>
        /// <param name="strWhere">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="curPage">当前索引页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        public static string GetPagingSqlStrMulti(string strDatasetsql, string strWhere, string orderBy, int curPage, int pageSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT T.* , ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderBy.Trim()))
            {
                strSql.Append("order by " + orderBy);
            }
            else
            {
                strSql.Append("order by T.PROJECTCODE desc");
            }
            strSql.Append(")AS Row from " + strDatasetsql.ToString() + " T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE 1=1 " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", ((curPage - 1) * pageSize + 1), (curPage * pageSize));
            return strSql.ToString();
        }

        /// <summary>
        /// 根据实体生成插入sql 
        /// </summary>
        /// <typeparam name="T">对应的数据库实体</typeparam>
        /// <param name="t">当前需要生成的实体对象</param>
        /// <param name="tablename">数据库表名称</param>
        /// <param name="notgenerafield">不参与生成的字段的名称（不区分大小写）</param>
        /// <returns></returns>
        public static string GeneraInsertSql<T>(T t, string tablename, List<string> notgenerafield)
        {
            string rel = string.Empty;

            StringBuilder sbsql = new StringBuilder();

            sbsql.Append("INSERT INTO " + tablename + " (");

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (notgenerafield != null && notgenerafield.Count > 0)
                {
                    if (notgenerafield.Where(o => o.ToLower() == propertyInfo.Name.ToLower()).Count() == 0)
                    {
                        sbsql.Append(propertyInfo.Name + ",");
                    }
                }
                else
                {
                    sbsql.Append(propertyInfo.Name + ",");
                }
            }
            //**去掉最后一个逗号 
            sbsql.Remove(sbsql.Length - 1, 1);
            sbsql.Append(" ) VALUES (");

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (notgenerafield != null && notgenerafield.Count > 0)
                {
                    if (notgenerafield.Where(o => o.ToLower() == propertyInfo.Name.ToLower()).Count() == 0)
                    {
                        sbsql.Append("@" + propertyInfo.Name + "@,");
                    }
                }
                else
                {
                    sbsql.Append("@" + propertyInfo.Name + "@,");
                }
            }
            //去掉最后一个逗号 
            sbsql.Remove(sbsql.Length - 1, 1);
            sbsql.Append(")");

            rel = ReplaceSqlInModelValue<T>(t, sbsql.ToString());

            return rel;
        }

        /// <summary>
        /// 根据实体生成更新sql 
        /// </summary>
        /// <typeparam name="T">对应的数据库实体</typeparam>
        /// <param name="t">当前需要生成的实体对象</param>
        /// <param name="tablename">数据库表名称</param>
        /// <param name="updatewherefield">更新条件字段的名称（不区分大小写）</param>
        /// <param name="notgenerafield">不参与生成的字段的名称（不区分大小写）</param>
        /// <returns></returns>
        public static string GeneraUpdateSql<T>(T t, string tablename, List<string> updatewherefield, List<string> notgenerafield)
        {
            string rel = string.Empty;

            StringBuilder sbsql = new StringBuilder();

            sbsql.Append(" UPDATE " + tablename + " SET ");

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (notgenerafield != null && notgenerafield.Count > 0)
                {
                    if (notgenerafield.Where(o => o.ToLower() == propertyInfo.Name.ToLower()).Count() == 0)
                    {
                        if (updatewherefield != null && updatewherefield.Count > 0)
                        {
                            if (updatewherefield.Where(o => o.ToLower() == propertyInfo.Name.ToLower()).Count() == 0)
                            {
                                sbsql.Append(propertyInfo.Name + "=@" + propertyInfo.Name + "@,");
                            }
                        }
                        else
                        {
                            sbsql.Append(propertyInfo.Name + "=@" + propertyInfo.Name + "@,");
                        }
                    }
                }
                else
                {
                    if (updatewherefield != null && updatewherefield.Count > 0)
                    {
                        if (updatewherefield.Where(o => o.ToLower() == propertyInfo.Name.ToLower()).Count() == 0)
                        {
                            sbsql.Append(propertyInfo.Name + "=@" + propertyInfo.Name + "@,");
                        }
                    }
                    else
                    {
                        sbsql.Append(propertyInfo.Name + "=@" + propertyInfo.Name + "@,");
                    }
                }

            }
            //去掉最后一个逗号 
            sbsql.Remove(sbsql.Length - 1, 1);
            sbsql.Append("");
            sbsql.Append(" WHERE 1=1 ");

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (updatewherefield != null && updatewherefield.Count > 0)
                {
                    if (updatewherefield.Where(o => o.ToLower() == propertyInfo.Name.ToLower()).Count() > 0)
                    {
                        sbsql.Append(" and " + propertyInfo.Name + "=@" + propertyInfo.Name + "@");
                    }
                }
            }
            rel = ReplaceSqlInModelValue<T>(t, sbsql.ToString());

            return rel;
        }
        /// <summary>
        /// 替换sql中的实体值
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="t">实体</param>
        /// <param name="tempsql">需要替换的sql</param>
        /// <returns></returns>
        private static string ReplaceSqlInModelValue<T>(T t, string tempsql)
        {
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                object obj = propertyInfo.GetValue(t, null);
                if (obj != null)
                {
                    string value = string.Empty;
                    string typestr = propertyInfo.PropertyType.Name;
                    if (propertyInfo.PropertyType.Name == "")
                    {

                    }
                    if (typestr == "String")
                    {
                        value = "'" + obj.ToString() + "'";
                    }
                    else if (typestr == "DateTime")
                    {
                        value = "'" + Convert.ToDateTime(obj).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                    }
                    else if (typestr == "Byte[]")
                    {
                        value = StringHelp.ToHexString(obj as byte[]);
                    }
                    else
                    {
                        value = obj.ToString();
                    }
                    tempsql = tempsql.Replace("@" + propertyInfo.Name + "@", value);
                }
                else
                {
                    tempsql = tempsql.Replace("@" + propertyInfo.Name + "@", "NULL");
                }

            }
            return tempsql;
        }
    }
}
