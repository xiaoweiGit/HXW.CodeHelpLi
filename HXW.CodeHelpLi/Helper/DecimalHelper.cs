using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HXW.CodeHelpLi
{
    /// <summary>
    /// Decimal数据类型辅助类
    /// </summary>
    public static class DecimalHelper
    {
        /// <summary> 
        /// 将小数值按指定的小数位数截断 
        /// </summary> 
        /// <param name="d">要截断的小数</param> 
        /// <param name="s">小数位数，s大于等于0，小于等于28</param> 
        /// <returns></returns> 
        public static decimal ToFixed(this Decimal d, int s)
        {
            decimal sp = Convert.ToDecimal(Math.Pow(10, s));

            if (d < 0)
                return Math.Truncate(d) + Math.Ceiling((d - Math.Truncate(d)) * sp) / sp;
            else
                return Math.Truncate(d) + Math.Floor((d - Math.Truncate(d)) * sp) / sp;
        }

        /// <summary>
        /// 将数值按照指定小数位中国四舍五入
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s">保留小数位</param>
        /// <returns></returns>
        public static decimal ToRounding(this Decimal d, int s)
        {
            return Math.Round(d, s, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 将数值转换为负值
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s">保留小数位</param>
        /// <returns></returns>
        public static decimal ToNegativeg(this Decimal d)
        {
            return Math.Abs(d)*-1;
        }
        /// <summary>
        /// 将数值转换为正值
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s">保留小数位</param>
        /// <returns></returns>
        public static decimal ToPositive(this Decimal d)
        {
            return Math.Abs(d);
        }
        /// <summary>
        /// 将数值转换为反值 即正数转换负值，负值转换为正值
        /// </summary>
        /// <param name="d"></param>
        /// <param name="s">保留小数位</param>
        /// <returns></returns>
        public static decimal ToInvert(this Decimal d)
        {
            return d * -1;
        }
    }
}
