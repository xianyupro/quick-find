using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using VideoAnalysis.Common;

namespace BingWallpaper
{
    public class BingImageProvider
    {
        //public async Task<BingImage> GetImage()
        //{
        //    string baseUri = "https://www.bing.com";
        //    using (var client = new HttpClient())
        //    {
        //        using (var jsonStream = await client.GetStreamAsync("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US"))
        //        {
        //            var ser = new DataContractJsonSerializer(typeof(Result));
        //            var res = (Result)ser.ReadObject(jsonStream);
        //            using (var imgStream = await client.GetStreamAsync(new Uri(baseUri + res.images[0].URL)))
        //            {
        //                return new BingImage(Image.FromStream(imgStream), res.images[0].Copyright, res.images[0].CopyrightLink);
        //            }
        //        }
        //    }
        //}
        public BingImage GetImage()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Timeout", "2000");
            headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
            headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            string baseUri = "https://www.bing.com";

            Request Response = new Request("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US", headers);
            var jsonStream = Response.GetContent();
            var ser = new DataContractJsonSerializer(typeof(Result));
            var res = (Result)ser.ReadObject(jsonStream);
            var imgStream = new Request(baseUri + res.images[0].URL, headers).GetContent();
            return new BingImage(Image.FromStream(imgStream), res.images[0].Copyright, res.images[0].CopyrightLink);
        }

        [DataContract]
        private class Result
        {
            [DataMember(Name = "images")]
            public ResultImage[] images { get; set; }
        }

        [DataContract]
        private class ResultImage
        {
            [DataMember(Name = "enddate")]
            public string EndDate { get; set; }
            [DataMember(Name = "url")]
            public string URL { get; set; }
            [DataMember(Name = "urlbase")]
            public string URLBase { get; set; }
            [DataMember(Name = "copyright")]
            public string Copyright { get; set; }
            [DataMember(Name = "copyrightlink")]
            public string CopyrightLink { get; set; }
        }
    }

    public class BingImage
    {
        public BingImage(Image img, string copyright, string copyrightLink)
        {
            Img = img;
            Copyright = copyright;
            CopyrightLink = copyrightLink;
        }
        public Image Img { get; set; }
        public string Copyright { get; set; }
        public string CopyrightLink { get; set; }
    }
}
