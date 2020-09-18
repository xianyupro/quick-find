using BingWallpaper;
using MouseKeyboardLibrary;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using TranslateApi;

namespace QuickFind
{
    public partial class ResultForm : MaterialSkin.Controls.MaterialForm
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr SetActiveWindow(IntPtr handle);

        private MouseHook mouseHook = new MouseHook();
        private Point localP = new Point();
        private string TitleX = "", Result = "";
        private bool lock_Form = false;
        private Settings settings;
        public ResultForm(String _Title, String result ,Settings _settings)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;///使窗体不显示在任务栏
            this.Result = result;
            TitleX = _Title;
            settings = _settings;
            #region 设置按键事件
            mouseHook.MouseDown += new MouseEventHandler((s, e) =>
            {
                AddMouseEvent("MouseDown", e.Button.ToString(), e.X, e.Y, "");
            });
            mouseHook.MouseUp += new MouseEventHandler((s, e) =>
            {
                AddMouseEvent("MouseUp", e.Button.ToString(), e.X, e.Y, "");
            });
            mouseHook.MouseWheel += new MouseEventHandler((s, e) =>
            {
                AddMouseEvent("MouseWheel", "", -1, -1, e.Delta.ToString());
            });
            mouseHook.Start();
            #endregion

            this.Text = _Title.Length > 5 ?  _Title.Substring(0, 5) + "..." :  _Title;
            ResultLabel.Text = result == "Error -2" ? "网络出错，请稍后重试" + result : result;

            #region 设置窗体宽高
            while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
            {
                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
            }
            
            this.Height = 74 + ResultLabel.Height + 8;
            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
            this.Width = 10 + ResultLabel.Width + 10;
            //SEbt.Location =new Point( Width-85,SEbt.Location.Y);
            SetBt(_settings.TranslateMode);
            #endregion

            #region 设置窗体出现位置
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            GetCursorPos(ref localP);
            this.Left = localP.X + 20 + this.Width > SW ? SW - this.Width - 10 : localP.X + 20;
            this.Top = localP.Y + 30 + this.Height > SH ? SH - this.Height - 30 : localP.Y + 20;
            #endregion

        }

        private void AddMouseEvent(string eventType, string button, int x, int y, string delta)
        {
            if (eventType == "MouseDown" && button == "Left")
            {
                if (x < this.Left || x > this.Left + this.Width)
                {
                    if(!lock_Form)
                        DisposeForm();
                    return;
                }
                if (y < this.Top || y > this.Top + this.Height)
                {
                    if (!lock_Form)
                        DisposeForm();
                    return;
                }
            }
            if (eventType == "MouseUp" && button == "Left")
            {
            }

            if (eventType == "MouseDown" && button == "Right")
            {
            }

            if (eventType == "MouseUp" && button == "Right")
            {
            }
        }

        private void DisposeForm()
        {
            mouseHook.Stop();
            this.Close();
            this.Dispose();
        }

        private void ResultForm_DoubleClick(object sender, EventArgs e)
        {
            lock_Form = true;
            if (TitleX != "") 
            {
                Clipboard.SetText(TitleX);
                ResultLabel.Text = "原文内容已复制至剪切板"; ResultLabel.Update();
            }
            else
            {
                ResultLabel.Text = "文本为空"; ResultLabel.Update();
            }

                
            new Thread(() =>
            {
                Thread.Sleep(700); 
                BeginInvoke(new Action(() =>
                {
                    lock_Form = false;
                    DisposeForm();
                }), null);
            }).Start();
        }

        private void SEbt_Click(object sender, EventArgs e)
        {
            switch (SEbt.Text)
            {
                case "小牛翻译":
                    SEbt.Text = "有道翻译";
                    new Thread(() =>
                    {
                        lock_Form = true;
                        settings.TranslateMode = "Youdao";
                        Result = TranslateAPI.Translate(TitleX, settings); BeginInvoke(new Action(() => { ResultLabel.Text = Result; while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
                            {
                                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
                            }
                            this.Height = 74 + ResultLabel.Height + 8;
                            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
                            this.Width = 10 + ResultLabel.Width + 10;
                        }), null);
                        lock_Form = false;
                    }).Start();
                    break;
                case "有道翻译":
                    SEbt.Text = "必应翻译";
                    new Thread(() =>
                    {
                        lock_Form = true;
                        settings.TranslateMode = "Bing";
                        Result = TranslateAPI.Translate(TitleX, settings); BeginInvoke(new Action(() => { ResultLabel.Text = Result; while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
                            {
                                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
                            }
                            this.Height = 74 + ResultLabel.Height + 8;
                            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
                            this.Width = 10 + ResultLabel.Width + 10;
                        }), null);
                        lock_Form = false;
                    }).Start();
                    break;
                case "必应翻译":
                    SEbt.Text = "谷歌翻译";
                    new Thread(() =>
                    {
                        lock_Form = true;
                        settings.TranslateMode = "Google";
                        Result = TranslateAPI.Translate(TitleX, settings); BeginInvoke(new Action(() => { ResultLabel.Text = Result; while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
                            {
                                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
                            }
                            this.Height = 74 + ResultLabel.Height + 8;
                            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
                            this.Width = 10 + ResultLabel.Width + 10;
                        }), null);
                        lock_Form = false;
                    }).Start();
                    break;
                case "谷歌翻译":
                    SEbt.Text = "百度翻译";
                    new Thread(() =>
                    {
                        lock_Form = true;
                        settings.TranslateMode = "Baidu";
                        Result = TranslateAPI.Translate(TitleX, settings); BeginInvoke(new Action(() => { ResultLabel.Text = Result; while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
                            {
                                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
                            }
                            this.Height = 74 + ResultLabel.Height + 8;
                            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
                            this.Width = 10 + ResultLabel.Width + 10;
                        }), null);
                        lock_Form = false;
                    }).Start();
                    break;
                case "百度翻译":
                    SEbt.Text = "小牛翻译";
                    new Thread(() =>
                    {
                        lock_Form = true;
                        settings.TranslateMode = "XiaoNiu";
                        Result = TranslateAPI.Translate(TitleX, settings); BeginInvoke(new Action(() => {
                            ResultLabel.Text = Result; while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
                            {
                                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
                            }
                            this.Height = 74 + ResultLabel.Height + 8;
                            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
                            this.Width = 10 + ResultLabel.Width + 10;
                        }), null);
                        lock_Form = false;
                    }).Start();
                    break;
            }

        }

        private void SetBt(string mode)
        {
            switch (mode)
            {
                case "Youdao":
                    SEbt.Text = "有道翻译";
                    break;
                case "Bing":
                    SEbt.Text = "必应翻译";
                    break;
                case "Google":
                    SEbt.Text = "谷歌翻译";
                    break;
                case "Baidu":
                    SEbt.Text = "百度翻译";
                    break;
                case "XiaoNiu":
                    SEbt.Text = "小牛翻译";
                    break;
            }
        }

        private void cBt1_Click(object sender, EventArgs e)
        {
            TitleX = TitleX.Replace("\r\n", "");
            TitleX = TitleX.Replace("\r", "");
            TitleX = TitleX.Replace("\n", "");
            switch (SEbt.Text)
            {
                case "百度翻译":
                    new Thread(() =>
                    {
                        lock_Form = true;
                        Result = TranslateAPI.Translate(TitleX, settings); BeginInvoke(new Action(() => { ResultLabel.Text = Result; while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
                            {
                                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
                            }
                            this.Height = 74 + ResultLabel.Height + 8;
                            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
                            this.Width = 10 + ResultLabel.Width + 10;
                        }), null);
                        lock_Form = false;
                    }).Start();
                    break;
                case "有道翻译":
                    new Thread(() =>
                    {
                        lock_Form = true;
                        Result = TranslateAPI.Translate(TitleX, settings); BeginInvoke(new Action(() => { ResultLabel.Text = Result; while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
                            {
                                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
                            }
                            this.Height = 74 + ResultLabel.Height + 8;
                            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
                            this.Width = 10 + ResultLabel.Width + 10;
                        }), null);
                        lock_Form = false;
                    }).Start();
                    break;
                case "必应翻译":
                    new Thread(() =>
                    {
                        lock_Form = true;
                        Result = TranslateAPI.Translate(TitleX, settings); BeginInvoke(new Action(() => { ResultLabel.Text = Result; while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
                            {
                                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
                            }
                            this.Height = 74 + ResultLabel.Height + 8;
                            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
                            this.Width = 10 + ResultLabel.Width + 10;
                        }), null);
                        lock_Form = false;
                    }).Start();
                    break;
                case "谷歌翻译":
                    new Thread(() =>
                    {
                        lock_Form = true;
                        Result = TranslateAPI.Translate(TitleX, settings); BeginInvoke(new Action(() => { ResultLabel.Text = Result; while (ResultLabel.Height * 3 / 2 > ResultLabel.MaximumSize.Width)
                            {
                                ResultLabel.MaximumSize = new Size(ResultLabel.MaximumSize.Width + 50, 1800);
                            }
                            this.Height = 74 + ResultLabel.Height + 8;
                            this.MaximumSize = new Size(10 + ResultLabel.Width + 10, 1800);
                            this.Width = 10 + ResultLabel.Width + 10;
                        }), null);
                        lock_Form = false;
                    }).Start();
                    break;
            }
        }

        private void ResultLabel_DoubleClick(object sender, EventArgs e)
        {
            lock_Form = true;
            if (Result != "")  Clipboard.SetText(Result);
            ResultLabel.Text = "翻译内容已复制至剪切板"; ResultLabel.Update();
            new Thread(() =>
            {
                Thread.Sleep(700); 
                BeginInvoke(new Action(() =>
                {
                    lock_Form = false;
                    DisposeForm();
                }), null);
            }).Start();
        }
    }
}
