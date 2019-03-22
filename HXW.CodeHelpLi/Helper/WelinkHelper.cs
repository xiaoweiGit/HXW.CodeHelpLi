using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace  HXW.CodeHelpLi
{
    public class WelinkHelper
    {
        /// <summary>
        /// 短信接口URL 
        /// </summary>
        static String HTTP_URL = "http://cf.51welink.com/submitdata/service.asmx/";

        /// <summary>
        /// 账号
        /// </summary>
        private const string USERNAME = "dllkang00";

        /// <summary>
        /// 密码
        /// </summary>
        private const string PASSWORD = "nj52110211";

        /// <summary>
        /// 产品编号
        /// </summary>
        private const string PRODUCTCODE = "1012888";

        public WelinkHelper() { }

        /// <summary>
        /// 发送普通短信
        /// </summary>
        /// <param name="mobile">手机号码, 多个号码以英文逗号隔开,最多支持100个号码</param>
        /// <param name="content">短信内容</param>
        /// <returns>返回发送状态，状态说明见下载包中的接口文档</returns>
        public SmsSendResponse SendTextSms(String mobile, String content)
        {
            StringBuilder param = new StringBuilder();
            param.Append("sname=").Append(USERNAME);
            param.Append("&spwd=").Append(PASSWORD);
            param.Append("&scorpid=").Append(string.Empty);
            param.Append("&sprdid=").Append(PRODUCTCODE);
            param.Append("&sdst=").Append(mobile);
            param.Append("&smsg=").Append(content);

            String address = String.Format("{0}g_Submit", HTTP_URL);

            //发送短信，并获取发送状态
            SmsSendResponse result = Post(address, param.ToString());

            return result;
        }

        private SmsSendResponse Post(string url, string data)
        {
            SmsSendResponse result = new SmsSendResponse();
            try
            {
                //发送短信
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] postData = encoding.GetBytes(data);

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;

                using (Stream sr = request.GetRequestStream())
                {
                    //发送数据
                    sr.Write(postData, 0, postData.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result.Code = Convert.ToString((int)HttpStatusCode.OK);
                        result.Message = reader.ReadToEnd();
                    }
                }
                else 
                {
                    result.Code = Convert.ToString((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                result.Code = "-1";
                result.Message = ex.Message;
            }
            return result;
        }
    }

    public class SmsSendResponse 
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}
