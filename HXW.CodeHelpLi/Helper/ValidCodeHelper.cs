﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  HXW.CodeHelpLi
{
    public class ValidCodeHelper
    {
        /// <summary>
        /// 数字验证码
        /// </summary>
        /// <param name="codeCount">验证码的位数‘n’</param>
        /// <returns>返回‘n’位验证码的字符串</returns>
        public static String GetRandomint(int codeCount)
        {
            Random random = new Random();
            StringBuilder sbmin = new StringBuilder();
            StringBuilder sbmax = new StringBuilder();
            for (int i = 0; i < codeCount; i++)
            {
                sbmin.Append("1");
                sbmax.Append("9");
            }
            return random.Next(Convert.ToInt32(sbmin.ToString()), Convert.ToInt32(sbmax.ToString())).ToString();
        }

        /// <summary>
        /// 生成验证码字符串
        /// </summary>
        /// <param name="codeLen">验证码字符长度</param>
        /// <returns>返回验证码字符串</returns>
        public static string MakeCode(int codeLen)
        {
            if (codeLen < 1)
            {
                return string.Empty;
            }
            int number;
            StringBuilder sbCheckCode = new StringBuilder();
            Random random = new Random();

            for (int index = 0; index < codeLen; index++)
            {
                number = random.Next();

                if (number % 2 == 0)
                {
                    sbCheckCode.Append((char)('0' + (char)(number % 10))); //生成数字
                }
                else
                {
                    sbCheckCode.Append((char)('A' + (char)(number % 26))); //生成字母
                }
            }
            return sbCheckCode.ToString();
        }

        ///<summary>
        /// 获取验证码图片流
        /// </summary>
        /// <param name="checkCode">验证码字符串</param>
        /// <returns>返回验证码图片流</returns>
        public static MemoryStream CreateCodeImg(string checkCode)
        {
            if (string.IsNullOrEmpty(checkCode))
            {
                return null;
            }
            Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics graphic = Graphics.FromImage(image);
            try
            {
                Random random = new Random();
                graphic.Clear(Color.White);
                int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                for (int index = 0; index < 25; index++)
                {
                    x1 = random.Next(image.Width);
                    x2 = random.Next(image.Width);
                    y1 = random.Next(image.Height);
                    y2 = random.Next(image.Height);

                    graphic.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Red, Color.DarkRed, 1.2f, true);
                graphic.DrawString(checkCode, font, brush, 2, 2);

                int x = 0;
                int y = 0;

                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    x = random.Next(image.Width);
                    y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                graphic.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //将图片验证码保存为流Stream返回
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                ms.Seek(0, SeekOrigin.Begin);
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms;
            }
            finally
            {
                graphic.Dispose();
                image.Dispose();
            }
        }

        
        
    }
}
