using Baidu.Aip.Ocr;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QuickFind.UnionOCR
{
    public class UnionOCR
    {
        public static string BaiduAPI(Image bitmap)
        {
            string token = "24.5142d6d4850be2c098165f677b8fe68b.2592000.1603087490.282335-22701901";
            string OCRresult = "";
            string API_KEY = "p8Tgf4cVCWi0QOGjnqfu22G9";
            string SECRET_KEY = "UvzNMtiR728kmjai8UjMLEctfZ2eVPNm";
            var client = new Ocr(API_KEY, SECRET_KEY);
            client.Timeout = 6000;  // 修改超时时间
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] image = ms.GetBuffer();
            ms.Close();
            try
            {
                var result = client.GeneralBasic(image);
                for (int i = 0; i < result["words_result"].Count(); i++)
                {
                    OCRresult = OCRresult + result["words_result"][i]["words"].ToString() + "\r\n";
                }
            }
            catch (OverflowException)
            {
                OCRresult = "网络出错请重试";
            }
            return OCRresult;
        }

        public async void SougouAPI(Bitmap bitmap)//POST一个多部分编码
        {
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Add("user-agent", "User-Agent    Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; MALNJS; rv:11.0) like Gecko");//设置请求头
            string url = "http://ocr.shouji.sogou.com/v2/ocr/json";
            HttpResponseMessage response;
            MultipartFormDataContent mulContent = new MultipartFormDataContent("----WebKitFormBoundaryrXRBKlhEeCbfHIY");//创建用于可传递文件的容器
            string path = @"jietu.jpg";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);  // 读文件流
            HttpContent fileContent = new StreamContent(fs);//为文件流提供的HTTP容器
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");//设置媒体类型
            mulContent.Add(fileContent, "pic", "pic.jpg");//这里第二个参数是表单名，第三个是文件名。如果接收的时候用表单名来获取文件，那第二个参数就是必要的了 

            response = await client.PostAsync(new Uri(url), mulContent);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            JObject sougouJson = (JObject)JsonConvert.DeserializeObject(result);

        }

    }


}
