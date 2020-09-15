using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace BingWallpaper
{
    public class Settings
    {
        private Options _options;
        private string _settingsPath;

        public Settings()
        {
            _settingsPath = Path.Combine(@"C:\Program Files\菠萝工具箱", "settings.txt");
            //if (!File.Exists(_settingsPath)) File.Create(_settingsPath);
            //_settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.txt");
            try
            {
                using (var stream = new FileStream(_settingsPath, FileMode.Open))
                {
                    var ser = new DataContractJsonSerializer(typeof(Options));
                    _options = (Options)ser.ReadObject(stream);
                }
            }
            catch (FileNotFoundException)
            {
                _options = new Options();
            }
            catch (SerializationException)
            {
                _options = new Options();
            }
        }

        #region User settings

        public bool LaunchOnStartup
        {
            get { return _options.LaunchOnStartup; }
            set
            {
                _options.LaunchOnStartup = value;
                Save();
            }
        }
        public bool UpdataWallpaper
        {
            get { return _options.UpdataWallpaper; }
            set
            {
                _options.UpdataWallpaper = value;
                Save();
            }
        }
        public bool aiTranslate
        {
            get { return _options.aiTranslate; }
            set
            {
                _options.aiTranslate = value;
                Save();
            }
        }
        public string ImageCopyright
        {
            get { return _options.ImgCopyright; }
            set
            {
                _options.ImgCopyright = value;
                Save();
            }
        }

        public string ImageCopyrightLink
        {
            get { return _options.ImgCopyrightLink; }
            set
            {
                _options.ImgCopyrightLink = value;
                Save();
            }
        }

        public string UpdateImgDay
        {
            get { return _options.UpdateImgDay; }
            set
            {
                _options.UpdateImgDay = value;
                Save();
            }
        }

        public int VersionNum
        {
            get { return _options.VersionNum; }
            set
            {
                _options.VersionNum = value;
                Save();
            }
        }
        public double UpdateTime
        {
            get { return _options.UpdateTime; }
            set
            {
                _options.UpdateTime = value;
                Save();
            }
        }

        public string TranslateMode
        {
            get { return _options.TranslateMode; }
            set
            {
                _options.TranslateMode = value;
                Save();
            }
        }
        #endregion

        private void Save()
        {
            using (var stream = new FileStream(_settingsPath, FileMode.Create))
            {
                var ser = new DataContractJsonSerializer(typeof(Options));
                ser.WriteObject(stream, _options);
            }
        }

        [DataContract]
        private class Options
        {
            [DataMember]
            public bool LaunchOnStartup = true;
            [DataMember]
            public bool aiTranslate = true;
            [DataMember]
            public bool UpdataWallpaper = false;
            [DataMember]
            public string ImgCopyright = "Bing Wallpaper";
            [DataMember]
            public string ImgCopyrightLink = "https://www.bing.com";
            [DataMember]
            public string UpdateImgDay = "0";
            [DataMember]
            public int VersionNum = 10;
            [DataMember]
            public double UpdateTime = 0.5;
            [DataMember]
            public string TranslateMode = "Google";
        }
    }
}
