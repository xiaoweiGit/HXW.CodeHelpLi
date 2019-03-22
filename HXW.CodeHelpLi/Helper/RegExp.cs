using System;
using System.Text.RegularExpressions;

namespace  HXW.CodeHelpLi
{
    public class RegExp
    {
        public static bool IsEmail(string s)
        {
            string text1 = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return Regex.IsMatch(s, text1);
        }

        public static bool IsIp(string s)
        {
            string text1 = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            return Regex.IsMatch(s, text1);
        }

        public static bool IsNumeric(string s)
        {
            string text1 = @"^\-?[0-9]+$";
            return Regex.IsMatch(s, text1);
        }

        public static bool IsPhysicalPath(string s)
        {
            string text1 = @"^\s*[a-zA-Z]:.*$";
            return Regex.IsMatch(s, text1);
        }

        public static bool IsRelativePath(string s)
        {
            if ((s == null) || (s == ""))
            {
                return false;
            }
            if (s.StartsWith("/") || s.StartsWith("?"))
            {
                return false;
            }
            if (Regex.IsMatch(s, @"^\s*[a-zA-Z]{1,10}:.*$"))
            {
                return false;
            }
            return true;
        }

        public static bool IsSafety(string s)
        {
            string text1 = s.Replace("%20", " ");
            text1 = Regex.Replace(text1, @"\s", " ");
            string text2 = "select |insert |delete from |count\\(|drop table|update |truncate |asc\\(|mid\\(|char\\(|xp_cmdshell|exec master|net localgroup administrators|:|net user|\"|\\'| or ";
            return !Regex.IsMatch(text1, text2, RegexOptions.IgnoreCase);
        }

        public static bool IsUnicode(string s)
        {
            string text1 = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return Regex.IsMatch(s, text1);
        }

        public static bool IsUrl(string s)
        {
            string text1 = @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
            return Regex.IsMatch(s, text1, RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 验证是否是手机号码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsMobile(string str)
        {
            string pattern = @"^(1[3|4|5|7|8])[\d]{9}$";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 验证是否是时间格式
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsDateTime(string str)
        {
            DateTime dt = DateTime.Now;
            return DateTime.TryParse(str, out dt);
        }

        /// <summary>
        /// 验证是否是正整数数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsInt(string str)
        {
            int k = 1;
            int.TryParse(str, out k);
            return k >= 0;
        }
        /// <summary>
        /// 验证是否是GUID格式字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsGuid(string str)
        {
            Guid gid = Guid.NewGuid();
            return Guid.TryParse(str, out gid);
        }
        /// <summary>
        /// 验证是否是座机电话
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsTel(string str)
        {
            string pattern = @"\d{3}-\d{8}|\d{4}-\d{7}";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 验证是否是身份证号码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsIdentity(string str)
        {
            string pattern = @"^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 验证是否是邮政编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsZipCode(string str)
        {
            string pattern = @"^\d{6}$";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        ///用户名验证只能是字母数字下划线，并且以字母开头(5-16位)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsUser(string str)
        {
            string pattern = @"^[a-zA-Z]\w{4,15}$";
            return Regex.IsMatch(str, pattern);
        }
    }
}
