using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  HXW.CodeHelpLi
{
    public class DataBasePaging
    {
        /// <summary>
        /// 返回分页查询查询语句（Oracle数据库）
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="StrWhere"></param>
        /// <param name="OrderBy"></param>
        /// <param name="CurPage"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static string GetPagingSqlStr(string TableName, string StrWhere, string OrderBy, int CurPage, int PageSize)
        {
            if (string.IsNullOrEmpty(OrderBy))
                return "SELECT * FROM (SELECT A.*, rownum r FROM (SELECT * FROM " + TableName + " WHERE 1=1 " + StrWhere + ") A WHERE rownum <= " + PageSize * CurPage + ") B WHERE r > " + (CurPage - 1) * PageSize + "";
            else
                return "SELECT * FROM (SELECT A.*, rownum r FROM (SELECT * FROM " + TableName + " WHERE 1=1 " + StrWhere + " ORDER BY  " + OrderBy + " ) A WHERE rownum <= " + PageSize * CurPage + ") B WHERE r > " + (CurPage - 1) * PageSize + "";
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
        public static string GetPagingSqlStr1(string strDatasetsql, string strWhere, string orderBy, int curPage, int pageSize)
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
    }
}
