using BingWallpaper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoAnalysis.Common;

namespace QuickFind.Update
{
    public class CheckUpdate
    {
        private  string VersionString = "https://raw.githubusercontent.com/xianyupro/quick-find/master/QuickFind/bin/Debug/Version.json";
        private  string DownloadUrl = "https://github.com/xianyupro/quick-find/raw/master/QuickFind/bin/Debug/%E8%8F%A0%E8%90%9D.exe";
        private  string UpdateDetail = "https://raw.githubusercontent.com/xianyupro/quick-find/master/QuickFind/bin/Debug/UpdateDetail.html";
        private int VersionNum;
        Settings settings;
        public CheckUpdate(Settings _settings)
        {
            settings = _settings;
        }
        public bool CheckVersion()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Timeout", "2000");
            headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
            headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");
            try
            {
                Request Response = new Request(VersionString, headers);
                var result = Response.GetJson();
                VersionNum = (int)result["Version"][0]["VersionNum"];
                if(VersionNum> settings.VersionNum)
                {
                    UpdateDetail = result["Version"][0]["UpdateDetail"].ToString();
                    DownloadUrl = result["Version"][0]["DownloadUrl"].ToString();
                    var Response_rd = new Request(UpdateDetail, headers);
                    Response_rd.SaveContent(@"C:\Program Files\菠萝工具箱\UpdateDetail.html", Response_rd.GetContent());
                    DownloadEXE();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 分段下载文件
        /// 读取速度和分块大小、网速有关
        /// </summary>
        private bool DownloadEXE()
        {
            try
            {
                DateTime start = DateTime.Now;
                Uri uri = new Uri(DownloadUrl);
                var filename =  @"C:\Program Files\菠萝工具箱\菠萝.exe";
                //指定url 下载文件
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                Stream stream = request.GetResponse().GetResponseStream();
                //创建写入流
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                byte[] bytes = new byte[1024 * 512];
                int readCount = 0;
                while (true)
                {
                    readCount = stream.Read(bytes, 0, bytes.Length);
                    if (readCount <= 0)
                        break;
                    fs.Write(bytes, 0, readCount);
                    fs.Flush();
                }
                fs.Close();
                stream.Close();
                Console.WriteLine("下载文件成功,用时：" + (DateTime.Now - start).TotalSeconds + "秒");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ResumeSoftware()
        {
            string sleep2 = "ping -n 4 127.1>nul ";
            string deleteBolo = "del / a / f / q \"菠萝.exe\"";
            string renameBolo = "move \"C:\\Program Files\\菠萝工具箱\\菠萝.exe\" \"菠萝.exe\"";
            string startBolo = "\"菠萝.exe\"";

            Process p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;

            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;

            //不显示程序窗口
            p.StartInfo.CreateNoWindow = false;
            //启动程序
            p.Start();
            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(sleep2 + "&" + deleteBolo + "&" + renameBolo + "&" + startBolo + "&exit");
            //p.StandardInput.WriteLine(sleep2 + "&" + deleteBolo + "&" + renameBolo );
            settings.VersionNum = VersionNum;
            Application.Exit();
        }


    }
}
