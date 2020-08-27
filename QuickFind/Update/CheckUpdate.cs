using BingWallpaper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoAnalysis.Common;

namespace QuickFind.Update
{
    public class CheckUpdate
    {
        private static string VersionString = "https://raw.githubusercontent.com/xianyupro/quick-find/master/QuickFind/bin/Debug/Version.json";
        private static string DownloadUrl = "https://github.com/xianyupro/quick-find/raw/master/QuickFind/bin/Debug/%E8%8F%A0%E8%90%9D.exe";
        private static string UpdateDetail = "https://raw.githubusercontent.com/xianyupro/quick-find/master/QuickFind/bin/Debug/UpdateDetail.html";

        public static bool CheckVersion(Settings settings )
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Timeout", "2000");
            headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
            headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");
            //headers.Add("Method", "POST");
            try
            {
                Request Response = new Request(VersionString, headers);
                var result = Response.GetJson();
                var VersionNum = (int)result["Version"][0]["VersionNum"];
                if(VersionNum> settings.VersionNum)
                {
                    UpdateDetail = result["Version"][0]["UpdateDetail"].ToString();
                    var Response_rd = new Request(UpdateDetail, headers);
                    Response_rd.SaveContent(@"C:\Program Files\菠萝工具箱\UpdateDetail.html", Response_rd.GetContent());
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

    }
}
