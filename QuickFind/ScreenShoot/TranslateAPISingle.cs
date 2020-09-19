using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using VideoAnalysis.Common;

namespace TranslateApi
{
    public class TranslateAPISingle
    {
        /// <summary>
        /// 统一翻译接口
        /// </summary>
        /// <param name="str"></param>
        /// <param name="translateMode">Mode ：“Google”，“Bing”，“Youdao”，“Baidu”</param>
        public static string Translate(String str)
        {
            string TargetLanguage = "", LanguageParas = "", resultStr = "";
            if (str == "") return "Error -1";
            int count = 0;
            char[] strNum = str.ToArray();
            for (int i = 0; i < str.Length; i++)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(str.Substring(i, 1), @"(?i)^[0-9a-z]+$"))
                {
                    count++;
                }
            }
            if (count < str.Length / 2) //判断待翻译的字符类型
            {
                TargetLanguage = "tl=en";
                LanguageParas = "&fromLang=zh-Hans&to=en";
            }
            else
            {
                TargetLanguage = "tl=zh-cn";
                LanguageParas = "&fromLang=en&to=zh-Hans";
            }

            resultStr = XiaoNiuTranslate(str, TargetLanguage, LanguageParas);

            if (resultStr != "")
            {
                return resultStr;
            }
            resultStr = XiaoNiuTranslate(str, TargetLanguage, LanguageParas);
            if (resultStr != "")
            {
                return resultStr;
            }
            resultStr = GoogleTranslate(str, TargetLanguage, LanguageParas);
            if (resultStr != "")
            {
                return resultStr;
            }
            resultStr = BingTranslate(str, TargetLanguage, LanguageParas);
            if (resultStr != "")
            {
                return resultStr;
            }
            resultStr = YoudaoTranslate(str, TargetLanguage, LanguageParas);
            if (resultStr != "")
            {
                return resultStr;
            }
            resultStr = BaiduTranslate(str, TargetLanguage, LanguageParas);
            if (resultStr != "")
            {
                return resultStr;
            }
            return "Error -2";
        }

        /// <summary>
        /// 调用谷歌翻译接口
        /// </summary>
        /// <param name="str"></param>
        /// <param name="TargetLanguage">  tl=en ? tl=zh-cn</param>
        /// <param name="LanguageParas"></param>
        private static String GoogleTranslate(String str, String TargetLanguage, String LanguageParas)
        {
            string strX;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Timeout", "2000");
            headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
            headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            headers.Add("Method", "POST");
            string url_google = "http://translate.google.cn/translate_a/single?client=webapp&dt=t&dt=bd&sl=auto&" + TargetLanguage + "&q=" + str + "&tk=";
            try
            {
                Request Response = new Request(url_google, headers);
                using (StreamReader file = new StreamReader(Response.GetContent()))
                {
                    strX = "{\"A\":" + file.ReadToEnd() + "}";
                }
                var result = JsonHelper.ConvertJson(strX);
                var resultStr = "";
                for (int i = 0; i < result["A"][0].Count(); i++)
                {
                    resultStr += result["A"][0][i][0].ToString();
                }
                return resultStr;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 调用bing翻译接口
        /// </summary>
        /// <param name="str"></param>
        /// <param name="TargetLanguage">  tl=en ? tl=zh-cn</param>
        /// <param name="LanguageParas"></param>
        private static string BingTranslate(String str, String TargetLanguage, String LanguageParas)
        {
            string resultStr = "";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Timeout", "2000");
            headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
            headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            headers.Add("Method", "POST");
            string bing_url = "https://cn.bing.com/ttranslatev3?";
            var postData = LanguageParas + "&text=" + str;
            try
            {
                Request Response = new Request(bing_url, headers, postData);
                var text = Response.GetText();
                var json_bing = JsonHelper.ConvertJson(text.Substring(1, text.Length - 2));
                for (int i = 0; i < json_bing["translations"].Count(); i++)
                {
                    resultStr += json_bing["translations"][i]["text"].ToString();
                }
                return resultStr;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 调用有道翻译接口
        /// </summary>
        /// <param name="str"></param>
        /// <param name="TargetLanguage">  tl=en ? tl=zh-cn</param>
        /// <param name="LanguageParas"></param>
        private static string YoudaoTranslate(String str, String TargetLanguage, String LanguageParas)
        {
            var url = "http://fanyi.youdao.com/translate?smartresult=dict&smartresult=rule";
            long salt = (long)((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds * 10000);
            long ts = salt / 10;
            var webClientObj = new WebClient();
            var postVars = new NameValueCollection {
                {"i", str},
                {"from", "AUTO"},
                {"to", "AUTO"},
                {"smartresult", "dict"},
                {"client", "fanyideskweb"},
                {"salt", salt.ToString()},
                {"sign", "b60de97e3b25c56832e7012b82329152"},
                {"ts", ts.ToString()},
                {"bv", "04578d470e7a887288dc80a9420e88ec"},
                {"doctype", "json"},
                {"version", "2.1"},
                {"keyfrom", "fanyi.web"},
                {"action", "FY_BY_REALTlME"}
            };
            try
            {
                byte[] byRemoteInfo = webClientObj.UploadValues(url, "POST", postVars);
                string text = Encoding.UTF8.GetString(byRemoteInfo);
                var json_youdao = JsonHelper.ConvertJson(text);
                string resultStr = "";
                for (int i = 0; i < json_youdao["translateResult"][0].Count(); i++)
                {
                    resultStr += json_youdao["translateResult"][0][i]["tgt"].ToString();
                }
                return resultStr;
            }
            catch
            {
                return "";
            }
        }
        #region 百度翻译接口
        public static string BaiduTranslate(String str, String TargetLanguage, String LanguageParas)
        {
            string resultStr = "";
            // 改成您的APP ID
            string appId = "20190916000334798";
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = "49hQrTtsBGkmHTA_S0R8";
            string sign = EncryptString(appId + str + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(str);
            url += "&from=" + "auto";
            url += "&to=" + (TargetLanguage == "tl=zh-cn" ? "zh" : "en");
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 2000;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                var json_baidu = JsonHelper.ConvertJson(retString);
                for (int i = 0; i < json_baidu["trans_result"].Count(); i++)
                {
                    resultStr += json_baidu["trans_result"][i]["dst"].ToString();
                }
                return resultStr;
            }
            catch
            {
                return "";
            }
        }


        public static string XiaoNiuTranslate(String str, String TargetLanguage, String LanguageParas)
        {
            // 改成您的APP ID
            string apikey = "6ab6e32c506b0dd22c46df7554620c81";
            str = str.Replace("\r\n", "");
            str = str.Replace("\r", "");
            str = str.Replace("\n", "");
            string resultStr = "";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Timeout", "2000");
            headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
            headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            headers.Add("Method", "POST");
            string xiaoniu_url = "http://free.niutrans.com/NiuTransServer/translation";
            var postData = TargetLanguage == "tl=en"? "from=zh&to=en&apikey=" + apikey + "&src_text=" + str: "from=en&to=zh&apikey=" + apikey + "&src_text=" + str;
            try
            {
                Request Response = new Request(xiaoniu_url, headers, postData);
                var text = Response.GetText();
                var json_xiaoniu = JsonHelper.ConvertJson(text);
                resultStr = json_xiaoniu["tgt_text"].ToString();
                return resultStr;
            }
            catch
            {
                return "";
            }
        }

        private static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        { 
            return true; 
        }
        #endregion
    }
}
