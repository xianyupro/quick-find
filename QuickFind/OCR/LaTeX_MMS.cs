using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoAnalysis.Common;

namespace QuickFind.OCR
{
    public class LaTeX_MMS
    {
        public static string LaTeXAPI(Bitmap bitmap)
        {
            string LaTeXRresult = "";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Timeout", "20000");
            headers.Add("ContentType", "application/json");
            headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            headers.Add("Method", "POST");

            string LaTeX_MMS_url = "https://www.latexlive.com:5001/api/mathpix/posttomathpix?";
            try
            {
                var postData = "{\"src\":\"data:image/png;base64," + ImageToBase64(bitmap)+"\"}";
            
                Request Response = new Request(LaTeX_MMS_url, headers, postData);
                var text = Response.GetText();
                var json_LaTeX_MMS = JsonHelper.ConvertJson(text);
                LaTeXRresult = json_LaTeX_MMS["latex_styled"].ToString();
                return LaTeXRresult;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Image 转成 base64
        /// </summary>
        /// <param name="fileFullName"></param>
        public static string ImageToBase64(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                byte[] arr = new byte[ms.Length]; ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length); ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
