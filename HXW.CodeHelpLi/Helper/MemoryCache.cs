using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace  HXW.CodeHelpLi
{
    public class MemoryCache
    {
        public MemoryCache() {

            if (items.Keys.Contains(KEY_LOGIN_USER_CACHE) == false)
            {
                items.Add(KEY_LOGIN_USER_CACHE, new LoginUserCache());//new Dictionary<string, object>()
            }
        }


        private static Dictionary<string, object> items = new Dictionary<string, object>();

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void AddItem(string key, Object value)
        {
            items.Add(key, value);
        }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static object GetItem(string key)
        {
            return items[key];
        }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <param name="key">键</param>
        /// <typeparam name="T">类型占位符</typeparam>
        /// <returns>值</returns>
        public static T GetItem<T>(string key)
        {
            return (T)items[key];
        }

        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key">键</param>
        public static void RemoveItem(string key)
        {
            items.Remove(key);
        }

        /// <summary>
        /// 判断键名称是否存在
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>true表示存在，false表示不存在</returns>
        public static bool ContainsKey(string key)
        {
            return items.ContainsKey(key);
        }

        /// <summary>
        /// 获取缓存个数
        /// </summary>
        public static int Count
        {
            get
            {
                return items.Count;
            }
        }

        #region 保留的Key值

        /// <summary>
        /// 数据库脚本缓存KEY
        /// </summary>
        public const string CACHE_KEY_SQL_MANIFEST = @"sql_manifest_key";

         /// <summary>
        /// 记录登录人Token
        /// </summary>
        public  const string KEY_LOGIN_USER_CACHE = "LOGIN_USER";
        #endregion
    }


    public class LoginUserCache
    {

        public Dictionary<string, object> Items = new Dictionary<string, object>();

    }
    
}
