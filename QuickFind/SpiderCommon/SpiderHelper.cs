using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

//教程一****post数据时序列化操作
//var dic = new SortedDictionary<string, string>
//            {
//                {"fromLang","zh-Hans" },
//                {"text", "如图一所示"},
//                {"to","en"}
//            };
//var jsonParam = JsonConvert.SerializeObject(dic);
//****END*******


namespace VideoAnalysis.Common
{
    public class Request : IDisposable
    {
        private HttpWebRequest HttpWReq;

        public bool StatusCode = false;


        public Request(string url)
        {
            HttpWReq = (HttpWebRequest)WebRequest.Create(url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="Headers">头文件</param>
        /// <param name="postFromOrObjData">post数据</param>
        public Request(string url, Dictionary<string, string> Headers, string postFromOrObjData = "")
        {
            HttpWReq = (HttpWebRequest)WebRequest.Create(url);
            HttpWReq.AllowAutoRedirect = false;
            HttpWReq.KeepAlive = true;

            if (Headers.ContainsKey("Timeout"))
            {
                HttpWReq.Timeout = Convert.ToInt16(Headers["Timeout"]);
            }
            if (Headers.ContainsKey("ContentType"))
            {
                HttpWReq.ContentType = Headers["ContentType"];
            }
            if (Headers.ContainsKey("Cookie"))
            {
                HttpWReq.Headers.Add("Cookie", Headers["Cookie"]);
            }
            if (Headers.ContainsKey("UserAgent"))
            {
                HttpWReq.UserAgent = Headers["UserAgent"];
            }
            if (Headers.ContainsKey("Host"))
            {
                HttpWReq.Host = Headers["Host"];
            }
            if (Headers.ContainsKey("Referer"))
            {
                HttpWReq.Referer = Headers["Referer"];
            }
            if (Headers.ContainsKey("Accept"))
            {
                HttpWReq.Accept = Headers["Accept"];
            }
            //Method方法应该在最后赋值
            if (Headers.ContainsKey("Method"))
            {
                HttpWReq.Method = Headers["Method"];
                if ("POST" == Headers["Method"])
                {
                    byte[] postBytes = Encoding.UTF8.GetBytes(postFromOrObjData);
                    HttpWReq.ContentLength = postBytes.Length;
                    Stream postDataStream = HttpWReq.GetRequestStream();
                    postDataStream.Write(postBytes, 0, postBytes.Length);
                    postDataStream.Close();
                }
            }
        }

        public string GetText()
        {
            HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
            StreamReader sr = new StreamReader(HttpWResp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string retString = sr.ReadToEnd();
            StatusCode = HttpWResp.StatusCode == HttpStatusCode.OK;
            return retString;
        }

        public Stream GetContent()
        {
            HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
            var content = HttpWResp.GetResponseStream();
            StatusCode = HttpWResp.StatusCode == HttpStatusCode.OK;
            return content;
        }

        public bool SaveContent(string path, Stream content)
        {
            try
            {
                using (FileStream fl = new FileStream(path, FileMode.Create))//展开一个流
                {
                    content.CopyTo(fl);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
       

        public JObject GetJson()
        {
            HttpWebResponse HttpWResp = (HttpWebResponse)HttpWReq.GetResponse();
            StatusCode = HttpWResp.StatusCode == HttpStatusCode.OK;
            StreamReader sr1 = new StreamReader(HttpWResp.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            string retString = sr1.ReadToEnd();
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(retString);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);

                return JsonConvert.DeserializeObject<JObject>(textWriter.ToString());
            }
            else
            {
                return null;
            }
        }



        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Request() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
