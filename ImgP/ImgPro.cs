using Baidu.Aip.ImageProcess;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgP
{
    public class ImgPro
    {
        static string API_KEY = "utq2WGAfv8XulRoOD567sWfP";
        static string SECRET_KEY = "RfPu1d1GlsOeSqC8PcEHuWO0L2mdGCs8 ";
        static ImageProcess client = new Baidu.Aip.ImageProcess.ImageProcess(API_KEY, SECRET_KEY);

            
        //图像无损放大 -- 输入一张图片，可以在尽量保持图像质量的条件下，将图像在长宽方向各放大两倍。 None
        public static void ImageQualityEnhanceDemo(string pic)
        {
            client.Timeout = 60000;  // 修改超时时间
            var image = File.ReadAllBytes(pic);
            // 调用图像无损放大，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.ImageQualityEnhance(image);
            FileInfo fileInfo = new FileInfo(pic);
            string newfilename = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_IQE" + fileInfo.Extension;
            SaveBase64Image(result["image"].ToString(), newfilename);
        }

        //图像去雾 -- 对浓雾天气下拍摄，导致细节无法辨认的图像进行去雾处理，还原更清晰真实的图像。 None
        public static void DehazeDemo(string pic)
        {
            client.Timeout = 60000;  // 修改超时时间
            var image = File.ReadAllBytes(pic);
            // 调用图像去雾，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.Dehaze(image);
            FileInfo fileInfo = new FileInfo(pic);
            string newfilename = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_Dehaze" + fileInfo.Extension;
            SaveBase64Image(result["image"].ToString(), newfilename);
        }

        //图像对比度增强 -- 调整过暗或者过亮图像的对比度，使图像更加鲜明。 None
        public static void ContrastEnhanceDemo(string pic)
        {
            client.Timeout = 60000;  // 修改超时时间
            var image = File.ReadAllBytes(pic);
            // 调用图像对比度增强，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.ContrastEnhance(image);
            FileInfo fileInfo = new FileInfo(pic);
            string newfilename = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_CEH" + fileInfo.Extension;
            SaveBase64Image(result["image"].ToString(), newfilename);
        }

        //黑白图像上色 -- 智能识别黑白图像内容并填充色彩，使黑白图像变得鲜活。 None
        public static void ColourizeDemo(string pic)
        {
            client.Timeout = 60000;  // 修改超时时间
            var image = File.ReadAllBytes(pic);
            // 调用黑白图像上色，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.Colourize(image);
            FileInfo fileInfo = new FileInfo(pic);
            string newfilename = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_Colourize" + fileInfo.Extension;
            SaveBase64Image(result["image"].ToString(), newfilename);
        }

        //拉伸图像恢复  -- 自动识别过度拉伸的图像，将图像内容恢复成正常比例。 None
        public static void StretchRestoreDemo(string pic)
        {
            client.Timeout = 60000;  // 修改超时时间
            var image = File.ReadAllBytes(pic);
            // 调用拉伸图像恢复，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.StretchRestore(image);
            FileInfo fileInfo = new FileInfo(pic);
            string newfilename = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_SR" + fileInfo.Extension;
            SaveBase64Image(result["image"].ToString(), newfilename);
        }
        //图像转ICO图标格式
        public static void transformateIcoDemo(string pic)
        {

            FileInfo fileInfo = new FileInfo(pic);
            string newfilename = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + ".ico";
            //获得原始图片文件
            using (Bitmap image = new Bitmap(pic))
            {
                //如果是windows调用，直接下面一行代码就可以了
                //此代码不能在web程序中调用，会有安全异常抛出
                using (Icon icon = Icon.FromHandle(image.GetHicon()))
                {
                    using (Stream stream = new FileStream(newfilename, FileMode.Create))
                    {

                        icon.Save(stream);
                    }
                }
            }
        }
        /// <summary>
        /// 图片尺寸压缩
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static void CompressImageWithSizeDemo(string pic)
        {
            Bitmap bitmap = new Bitmap(pic);
            FileInfo fileInfo = new FileInfo(pic);
            string newfilename = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_压缩" + fileInfo.Extension;

            int actualWidth = (int)(bitmap.Width / 2);
            int actualHeight = (int)(bitmap.Height / 2);
            var actualBitmap = new Bitmap(actualWidth, actualHeight);
            var g = Graphics.FromImage(actualBitmap);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            g.DrawImage(bitmap, new Rectangle(0, 0, actualWidth, actualHeight)
                , new Rectangle(0, 0, bitmap.Width, bitmap.Height)
                , GraphicsUnit.Pixel);
            g.Dispose();
            actualBitmap.Save(newfilename);
            bitmap.Dispose();
            actualBitmap.Dispose();
        }

        /// <summary>
        /// 图像质量压缩
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="quality">质量压缩率，取值0到100</param>
        /// <returns></returns>
        public static void CompressImageWithQualityDemo(string pic, int quality = 90)
        {
            Bitmap bitmap = new Bitmap(pic);
            FileInfo fileInfo = new FileInfo(pic);
            string newfilename = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_20%" + fileInfo.Extension;
            var ps = new EncoderParameters(1);
            ps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            var stream = new MemoryStream();
            bitmap.Save(stream, GetImageEncoders()["image/jpeg"], ps);
            var compressedBitmap = new Bitmap(stream);
            compressedBitmap.Save(newfilename);
            bitmap.Dispose();
            compressedBitmap.Dispose();
        }

        /// <summary>
        /// 图像灰度化
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static void PicGrayDemo(string pic)
        {
            Bitmap bitmap = new Bitmap(pic);
            FileInfo fileInfo = new FileInfo(pic);
            string newfilename = fileInfo.DirectoryName + @"\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length) + "_gray" + fileInfo.Extension;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    //获取该点的像素的RGB的颜色
                    Color color = bitmap.GetPixel(i, j);
                    //利用公式计算灰度值
                    int gray = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    bitmap.SetPixel(i, j, newColor);
                }
            }
            bitmap.Save(newfilename);
            bitmap.Dispose();
        }

        private static Dictionary<string, ImageCodecInfo> GetImageEncoders()
        {
            var result = new Dictionary<string, ImageCodecInfo>();
            var encoders = ImageCodecInfo.GetImageEncoders().ToList();
            foreach (var encode in encoders)
                result.Add(encode.MimeType, encode);
            return result;
        }



        //将base64编码的字符串转为图片并保存
        static void SaveBase64Image(string source, string filePath)
        {
            try
            {
                string strbase64 = source.Substring(source.IndexOf(',') + 1);
                strbase64 = strbase64.Trim('\0');
                byte[] arr = Convert.FromBase64String(strbase64);
                using (MemoryStream ms = new MemoryStream(arr))
                {
                    Bitmap bmp = new Bitmap(ms);
                    bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ms.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
