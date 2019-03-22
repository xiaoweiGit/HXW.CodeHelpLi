using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Data;
/***************************************************************   
* 作者：	   zxz
* 书写时间： 2015-09-19
* 内容概要： datatable操作的一些辅助方法
*  ------------------------------------------------------
* 修改
****************************************************************/
namespace  HXW.CodeHelpLi
{
    /// <summary>
    /// DataTable操作的一些辅助方法
    /// </summary>
    public static class DataTableHelp
    {
        /// <summary>
        /// 将DataTable转换成List集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dataTable">传入的DataTable</param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return default(List<T>);
            string jsonString = JsonConvert.SerializeObject(dataTable, new DataTableConverter());
            return JsonConvert.DeserializeObject<List<T>>(jsonString);
        }

        /// <summary>
        /// 将只有一行的DataTable转换成实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dataTable">只有一行的DataTable</param>
        /// <returns></returns>
        public static T ToEntity<T>(DataTable dataTable)
        {
            if (dataTable == null||dataTable.Rows.Count==0)
                return default(T);
            return ToList<T>(dataTable).Count == 0 ? default(T) : ToList<T>(dataTable)[0];
        }

        /// <summary>
        /// 将List集合转换成DataTable
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="list">泛型集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                return null;
            string jsonString = JsonConvert.SerializeObject(list);
            return JsonConvert.DeserializeObject<DataTable>(jsonString, new DataTableConverter());
        }
        /// <summary>
        /// 将DataTable转换成JOSN
        /// </summary>
        /// <param name="dataTable">传入的DataTable</param>
        /// <returns></returns>
        public static string ToJson(DataTable dataTable)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return string.Empty;
            return JsonConvert.SerializeObject(dataTable);
        }
        /// <summary>
        /// 将Object转为json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将只有一行的DataTable转换成实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static T ToEntity<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns>分好页的DataTable数据</returns>              第1页        每页10条
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0) { return dt; }
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
            { return newdt; }

            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }

        #region DataRow 相关方法
        /// <summary>
        /// 检查DataRow某列是否存在（为NULL数据等同于不存在）
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="colname"></param>
        /// <returns></returns>
        public static bool CheckDataRowColumnsValue(this DataRow dr, string colname)
        {
            bool rel = false;
            if (dr.Table.Columns.Contains(colname) && !dr.IsNull(colname))
            {
                rel = true;
            }
            return rel;
        }
        #endregion

    }
}
