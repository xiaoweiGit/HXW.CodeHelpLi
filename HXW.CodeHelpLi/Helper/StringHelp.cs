using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace  HXW.CodeHelpLi
{
    /***************************************************************   
* 作者：	 SSL
* 书写时间： 2016-01-05
* 内容概要： 字符串操作辅助类
*  ------------------------------------------------------
* 修改
****************************************************************/
    public static class StringHelp
    {
		/// <summary>
        /// 个人-虚拟商户
        /// </summary>
        public static string PersonalGuid = "EAFFAF3F-716D-415C-B45E-AA3C1422B5E7";
		
        /// <summary>
        /// 获取请求参数
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">请求的参数</param>
        /// <returns></returns>
        public static T Converts<T>(object obj)
        {
            if (obj == null) { return default(T); }
            else
            {
                string objStr = obj.ToString().Trim();
                if (objStr == "") { return default(T); }
                bool b = objStr.StartsWith("\"") && objStr.EndsWith("\"");
                bool b1 = objStr.StartsWith("{") && objStr.EndsWith("}");
                bool b2 = objStr.StartsWith("[") && objStr.EndsWith("]");
                if (!b && !(b1 || b2)) { obj = "\"" + objStr + "\""; }
            }
            try { return JsonConvert.DeserializeObject<T>(obj.ToString()); }
            catch { return default(T); }
        }
        /// <summary>
        /// 构建sql查询条件
        /// </summary>
        /// <param name="colName">列名</param>
        /// <param name="Val">参数</param>
        /// <param name="Operation">操作符<>=like!=</param>
        /// <returns></returns>
        public static string BuildSqlCondition(string colName, string Val, string Operation)
        {
            string sql = "";
            Val = RemoveDangerSql(Val);
            Operation = Operation.Trim().ToLower();
            switch (Operation)
            {
                case ">": sql += " and " + colName + " >'" + Val + "'"; break;
                case "=": sql += " and " + colName + " ='" + Val + "'"; break;
                case ">=": sql += " and " + colName + " >='" + Val + "'"; break;
                case "<": sql += " and " + colName + " <'" + Val + "'"; break;
                case "<=": sql += " and " + colName + " <='" + Val + "'"; break;
                case "!=": sql += " and " + colName + " !='" + Val + "'"; break;
                case "<>": sql += " and " + colName + " <>'" + Val + "'"; break;
                case "like": sql += " and " + colName + " like '%" + Val + "%'"; break;
                case "in":
                    {
                        string[] arrPara = Val.Split(',');
                        string inVal = "";
                        foreach (string str in arrPara)
                        {
                            inVal += "'" + str + "',";
                        }
                        inVal = inVal.TrimEnd(',');
                        sql += " and " + colName + " in (" + inVal + ")";
                    }
                    break;
                case "orlike":
                    sql += " or " + colName + " like '%" + Val + "%'"; break;
                case "notin":
                    sql += " and " + colName + " not in(" + Val + ")"; break;
                default: break;
            }
            return sql;
        }

        /// <summary>
        /// 删除字符串中可能存在的注入参数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveDangerSql(string str)
        {
            str = Regex.Replace(str, "select", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "insert", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete from", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "count''", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop table", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "asc", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "mid", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "char", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "exec master", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net localgroup administrators", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "and", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net user", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "or", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "'", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "%", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, ";", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "script", "", RegexOptions.IgnoreCase);
            return str;
        }

        /// <summary>
        /// 将datarow的列转换成字符串，如果列或datarow不存在，则返回空白字符串
        /// </summary>
        /// <param name="dr">datarow</param>
        /// <param name="index">数字类型的索引或者是字符串类型的列名</param>
        /// <returns></returns>
        public static string GetDataRowColValue(DataRow dr, object index)
        {
            int ind = -1;
            string col = index.ToString().Trim();
            bool b = int.TryParse(col, out ind);
            string str = "";
            if (dr == null)
            { return str; }
            if (b)
            {
                try { str = Convert.ToString(dr[ind]); }
                catch (Exception) { str = ""; }
            }
            else
            {
                try { str = Convert.ToString(dr[col]); }
                catch (Exception) { str = ""; }
            }
            return str;
        }


        /// <summary>
        /// 将字符串转换成byte数组
        /// </summary>
        /// <param name="strHex">要转换成byte数组的字符串</param>
        /// <returns></returns>
        public static byte[] ConvertToByteArray(string strHex)
        {
            List<byte> bytList = new List<byte>();
            foreach (char c in strHex)
            {
                bytList.Add(Convert.ToByte(c.ToString(), 16));
            }
            return bytList.ToArray();
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="pToEncrypt">待加密字符串</param>
        /// <param name="desKeyStr">Key字符串8位</param>
        /// <param name="desIVStr">向量字符串8位</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptStr(string pToEncrypt, string desKeyStr, string desIVStr)
        {
            if (desIVStr.Length != desKeyStr.Length || desKeyStr.Length != 8)
            {
                return "";
            }
            byte[] desKey = ConvertToByteArray(desKeyStr);
            byte[] desIV = ConvertToByteArray(desIVStr);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                //把字符串放到byte数组中
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = desKey;
                des.IV = desIV;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch
            {
                return pToEncrypt;
            }
            finally
            {
                des = null;
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="pToDecrypt">要解密的字符串</param>
        ///<param name="desKeyStr">Key字符串8位</param>
        /// <param name="desIVStr">向量字符串8位</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptStr(string pToDecrypt, string desKeyStr, string desIVStr)
        {
            if (desIVStr.Length != desKeyStr.Length || desKeyStr.Length != 8)
            {
                return "";
            }
            byte[] desKey = ConvertToByteArray(desKeyStr);
            byte[] desIV = ConvertToByteArray(desIVStr);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                //建立加密对象的密钥和偏移量，此值重要，不能修改
                des.Key = desKey;
                des.IV = desIV;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return pToDecrypt;
            }
            finally
            {
                des = null;
            }
        }

        /// <summary>
        /// 32位大写MD5加密算法
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后生成的32位大写MD5字符串</returns>
        public static string ToMd5Str(string str)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            string res = BitConverter.ToString(hashmd5.ComputeHash(Encoding.Default.GetBytes(str)));
            res = res.Replace("-", "").ToUpper();
            return res;
        }

        /// <summary>
        /// 生成大写随机字符串
        /// </summary>
        /// <param name="counts">要生成的字符串数量</param>
        /// <returns>生成的随机字符串</returns>
        public static string CreateRndStr(int counts)
        {
            //设定字符范围为:大小写字母及数字的随机字符串.
            string rndChar = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strRes = "";
            Random rnd = new Random();
            int ranNum = 0;
            for (int i = 0; i < counts; i++)
            {
                ranNum = rnd.Next(rndChar.Length);
                strRes += rndChar[ranNum];
            }
            return strRes;
        }

        /// <summary>
        /// 生成六位纯数字验证码
        /// </summary>
        /// <returns></returns>
        public static string CreateRandomNumbers()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        /// <summary>
        /// 格式化obj为yyyy-MM-dd HH:mm:ss字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>格式化失败返回NULL，否则返回格式化后的时间</returns>
        public static string FormatTimeStr<T>(T obj)
        {
            DateTime dt = new DateTime(1991, 1, 1);
            bool b = DateTime.TryParse(obj.ToString(), out dt);
            return b ? dt.ToString("yyyy-MM-dd HH:mm:ss") : null;
        }

        /// <summary>
        /// 验证是否是Email格式
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsEmail(string str)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 验证是否是手机号码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsMobilePhone(string str)
        {
            string pattern = @"^(1[3|4|5|7|8])[\d]{9}$";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 验证是否是手机号码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsMobilePhoneEx(string str)
        {
            string pattern = @"^[0-9]*$";
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
        public static bool IsIntNum(string str)
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
        public static bool IsCellPhone(string str)
        {
            string pattern = @"\d{3}-\d{8}|\d{4}-\d{7}";
            return Regex.IsMatch(str, pattern);
        }
        /// <summary>
        /// 验证是否是合法的IP地址
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsIPAdress(string str)
        {
            string pattern = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            return Regex.IsMatch(str, pattern);
        }
        /// <summary>
        /// 验证是否是网址格式
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsUrlStr(string str)
        {
            string pattern = @"^((http|https)\:(\/\/)?)?(\w+\.)+\w+(\/.*)?$";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 验证是否是身份证号码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsIDCard(string str)
        {
            string pattern = @"^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 验证是否是邮政编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsPostCode(string str)
        {
            string pattern = @"^\d{6}$";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        ///用户名验证只能是字母数字下划线，并且以字母开头(5-16位)
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是true，否false</returns>
        public static bool IsUserName(string str)
        {
            string pattern = @"^[a-zA-Z]\w{4,15}$";
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 文件名称中是否包含&，空格，=特殊符号
        /// 防止文件名非法，导致文件无法预览
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsVadFileName(string str)
        {
            string pattern = @"(&|=|\s)";
            return !Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 验证输入字母和数字
        /// Add by yangyang 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsVadNumAndCode(string str)
        {
            string pattern = @"/^[A-Za-z0-9]+$/";
            return !Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 验证金额
        /// Add by yangyang 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsVadMoney(string str)
        {
            string pattern = @"/^[0-9]*(\.[0-9]{1,2})?$/";
            return !Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 查询条件中对结束日期做加一处理
        /// Add by yangyang 2015-09-23
        /// </summary>
        /// <param name="oriDate"></param>
        /// <returns></returns>
        public static string DateTimeAddOne(string oriDate)
        {
            string retVal = "";
            if (string.IsNullOrEmpty(oriDate))
            {
                retVal = null;
            }
            else
            {
                retVal = DateTime.Parse(oriDate).AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00";
            }
            return retVal;
        }
        /// <summary>
        /// 将金额字符串转换成大写金额
        /// </summary>
        /// <param name="money">金额字符串</param>
        /// <returns></returns>
        public static string ConvertChineseCost(string money)
        {
            double MyNumber = 0;
            bool b = double.TryParse(money, out MyNumber);
            if (!b)
            {
                return "无效金额";
            }
            string[] MyScale = { "分", "角", "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "兆", "拾", "佰", "仟" };
            string[] MyBase = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            string M = "";
            bool isPoint = false;
            if (money.IndexOf(".") != -1)
            {
                money = money.Remove(money.IndexOf("."), 1);
                isPoint = true;
            }
            for (int i = money.Length; i > 0; i--)
            {
                int MyData = Convert.ToInt16(money[money.Length - i].ToString());
                M += MyBase[MyData];
                if (isPoint == true)
                {
                    M += MyScale[i - 1];
                }
                else
                {
                    M += MyScale[i + 1];
                }
            }
            return M;
        }


        /// <summary>
        /// 获取指定日期是周几
        /// Add by yangyang 
        /// 2016-01-19
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ReturnDayWeek(DateTime date)
        {
            string retVal = string.Empty;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    retVal = "周日";
                    break;
                case DayOfWeek.Monday:
                    retVal = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    retVal = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    retVal = "周三";
                    break;
                case DayOfWeek.Thursday:
                    retVal = "周四";
                    break;
                case DayOfWeek.Friday:
                    retVal = "周五";
                    break;
                case DayOfWeek.Saturday:
                    retVal = "周六";
                    break;
                default: break;
            }
            return retVal;
        }

        /// <summary>
        /// 获取周一的日期
        /// Add by yangyang 
        /// 2016-01-20
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ReturnMondayDate(DateTime date)
        {
            DateTime retVal = new DateTime();
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    retVal = date.AddDays(-6);
                    break;
                case DayOfWeek.Monday:
                    retVal = date;
                    break;
                case DayOfWeek.Tuesday:
                    retVal = date.AddDays(-1);
                    break;
                case DayOfWeek.Wednesday:
                    retVal = date.AddDays(-2);
                    break;
                case DayOfWeek.Thursday:
                    retVal = date.AddDays(-3);
                    break;
                case DayOfWeek.Friday:
                    retVal = date.AddDays(-4);
                    break;
                case DayOfWeek.Saturday:
                    retVal = date.AddDays(-5);
                    break;
                default: break;
            }
            return retVal;
        }

        /// <summary>
        /// 获取当前季度第一天的日期
        /// Add by yangyang 
        /// 2016-01-20
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ReturnCurrentQuarterDate(DateTime date)
        {
            DateTime retVal = new DateTime();
            switch (date.Month)
            {
                case 1:
                case 2:
                case 3:
                    retVal = DateTime.Parse(date.Year + "-01-01 00:00:00");
                    break;
                case 4:
                case 5:
                case 6:
                    retVal = DateTime.Parse(date.Year + "-04-01 00:00:00");
                    break;
                case 7:
                case 8:
                case 9:
                    retVal = DateTime.Parse(date.Year + "-07-01 00:00:00");
                    break;
                case 10:
                case 11:
                case 12:
                    retVal = DateTime.Parse(date.Year + "-10-01 00:00:00");
                    break;
                default: break;
            }
            return retVal;
        }
        /// <summary>
        /// 获取距离字符串
        /// </summary>
        /// <param name="str">字符串数字米为单位</param>
        /// <returns></returns>
        public static string FormatDistance(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return "";
            try
            {
                double distance = Convert.ToDouble(str.Trim());
                if (distance < 1000)
                    return distance + "米";
                return Math.Round((distance / 1000.0), 2) + "千米";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 格式转换
        /// 数值转时刻
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string FormartTime(decimal dec)
        {
            string retval = string.Empty;
            retval = dec.ToString("0.00").Replace(".", ":").PadLeft(5, '0');
            return retval;
        }

        /// <summary>
        /// 返回券链接
        /// </summary>
        /// <param name="TpUrl">模板url</param>
        /// <param name="BussinessGuid">商户ID</param>
        /// <param name="CouponID">券ID</param>
        /// <param name="ChannelDetailGuid">渠道详情编码</param>
        /// <returns></returns>
        public static string GetItemUrl(string TpUrl, string BussinessGuid, string CouponID, string ChannelDetailGuid)
        {
            TpUrl = TpUrl ?? "";
            BussinessGuid = BussinessGuid ?? Guid.Empty.ToString();
            CouponID = CouponID ?? Guid.Empty.ToString();
            ChannelDetailGuid = ChannelDetailGuid ?? Guid.Empty.ToString();
            string itemUrl = "";
            if (TpUrl.Contains("?"))
                itemUrl = TpUrl + "&BussinessGuid=" + BussinessGuid + "&CouponID=" + CouponID + "&ChannelDetailGuid=" + ChannelDetailGuid;
            else
                itemUrl = TpUrl + "?BussinessGuid=" + BussinessGuid + "&CouponID=" + CouponID + "&ChannelDetailGuid=" + ChannelDetailGuid;
            return itemUrl;
        }

        /// <summary>
        /// 返回券链接
        /// </summary>
        /// <param name="TpUrl">模板url</param>
        /// <param name="BussinessGuid">商户ID</param>
        /// <param name="CouponID">券ID</param>
        /// <returns></returns>
        public static string GetItemUrl(string TpUrl, string BussinessGuid, string CouponID)
        {
            TpUrl = TpUrl ?? "";
            BussinessGuid = BussinessGuid ?? Guid.Empty.ToString();
            CouponID = CouponID ?? Guid.Empty.ToString();
            string itemUrl = "";
            if (TpUrl.Contains("?"))
                itemUrl = TpUrl + "&BussinessGuid=" + BussinessGuid + "&CouponID=" + CouponID;
            else
                itemUrl = TpUrl + "?BussinessGuid=" + BussinessGuid + "&CouponID=" + CouponID;
            return itemUrl;
        }

        /// <summary>
        /// 格式化数值，去掉末尾的0
        /// </summary>
        /// <returns></returns>
        public static string DecimalParseData(decimal value)
        {
            string retVal = string.Empty;
            if (value.ToString().Substring(value.ToString().Length - 1, 1) == "0")
            {
                if (value.ToString().Substring(value.ToString().Length - 2, 1) == "0")
                {
                    retVal = value.ToString().Split('.')[0];
                }
                else
                {
                    retVal = value.ToString().Split('.')[0] + "." + value.ToString().Substring(value.ToString().Length - 2, 1);
                }
            }
            else
            {
                retVal = value.ToString();
            }
            return retVal;
        }
        /// <summary>
        /// 券的状态
        /// </summary>
        /// <param name="oldValue">原始状态</param>
        /// <param name="validNote">起止日期以-分割</param>
        /// <param name="remainingCount">剩余量</param>
        /// <param name="chanelSendState">渠道记录状态</param>
        /// <returns>0:正常发放；1:停止发放；2:已领完；3：已过期</returns>
        public static int GetSendState(int oldValue, string validNote, int remainingCount, int chanelSendState)
        {
            try
            {
                if (chanelSendState == 1)
                    return 1;
                validNote = validNote.Split('-')[1].Trim();
                if (Convert.ToDateTime(validNote).AddDays(1) < DateTime.Now)
                    return 3;
                if (remainingCount < 1)
                    return 2;
                return oldValue;
            }
            catch
            {
                return oldValue;
            }

        }

        /// <summary>
        /// 将long转换成时间datetime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ConvertTimestamp(long timestamp)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime newDateTime = converted.AddSeconds(timestamp);
            return newDateTime.ToLocalTime();
        }
        /// <summary>
        /// 将时间datetime转换成Long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLongTimestamp(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (long)span.TotalSeconds;
        }

        /// <summary>  
        /// 每隔n个字符插入一个字符  
        /// </summary>  
        /// <param name="input">源字符串</param>  
        /// <param name="interval">间隔字符数</param>  
        /// <param name="maxinsertnum">最多插入字符</param>  
        /// <param name="value">待插入值</param>  
        /// <returns>返回新生成字符串</returns>  
        public static string InsertFormat(string input, int interval, int maxinsertnum, string value)
        {
            int crrnum = 0;
            for (int i = interval; i < input.Length; i += interval + 1)
            {
                if (crrnum <= maxinsertnum)
                {
                    input = input.Insert(i, value);
                }
                else
                {
                    break;
                }
                crrnum++;
            }
            return input;
        }

        /// <summary>
        /// 生成6位不重复的编码
        /// </summary>
        /// <returns></returns>
        public static string Get6BitCode()
        {
            return Guid.NewGuid().GetHashCode().ToString("X8").Substring(0, 6);
        }

        /// <summary>
        /// byte[]数组转换为十六进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                strB.Append("0x");
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }

                hexString = strB.ToString();

            } return hexString;

        }

        /// <summary>
        /// 获取指定日期是周几
        /// Add by yangyang 
        /// 2016-01-19
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ReturnDayNumWeek(DateTime date)
        {
            string retVal = string.Empty;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    retVal = "7";
                    break;
                case DayOfWeek.Monday:
                    retVal = "1";
                    break;
                case DayOfWeek.Tuesday:
                    retVal = "2";
                    break;
                case DayOfWeek.Wednesday:
                    retVal = "3";
                    break;
                case DayOfWeek.Thursday:
                    retVal = "4";
                    break;
                case DayOfWeek.Friday:
                    retVal = "5";
                    break;
                case DayOfWeek.Saturday:
                    retVal = "6";
                    break;
                default: break;
            }
            return retVal;
        }
    }
}
