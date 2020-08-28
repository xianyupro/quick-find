
using BingWallpaper;
using Microsoft.Win32;
using MouseKeyboardLibrary;
using QuickFind.OCR;
using QuickFind.Properties;
using QuickFind.Update;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TranslateApi;
using VideoAnalysis.Common;
using Settings = BingWallpaper.Settings;

namespace QuickFind
{
    public partial class QuickForm : MaterialSkin.Controls.MaterialForm
    {
        #region DLLImport
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        private static extern void DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体

        //API 常數定義
        private const int SW_HIDE = 0;
        private const int SW_NORMAL = 1;
        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWNOACTIVATE = 4;
        private const int SW_SHOW = 5;
        private const int SW_MINIMIZE = 6;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWDEFAULT = 10;
        [DllImport("user32.dll")]
        public static extern int ShowWindow(int hwnd,int nCmdShow);

        /// <summary>
        /// 导入模拟键盘的方法
        /// </summary>
        /// <param name="bVk" >按键的虚拟键值</param>
        /// <param name= "bScan" >扫描码，一般不用设置，用0代替就行</param>
        /// <param name= "dwFlags" >选项标志：0：表示按下，2：表示松开</param>
        /// <param name= "dwExtraInfo">一般设置为0</param>
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        //获取窗口标题
        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        //根据坐标获取窗口句柄
        [DllImport("user32")]
        private static extern IntPtr WindowFromPoint(Point Point);

        [DllImport("user32.dll")]
        private static extern IntPtr SetActiveWindow(IntPtr handle);

        #region bVk参数 常量定义

        public const byte vbKeyLButton = 0x1;    // 鼠标左键
        public const byte vbKeyRButton = 0x2;    // 鼠标右键
        public const byte vbKeyCancel = 0x3;     // CANCEL 键
        public const byte vbKeyMButton = 0x4;    // 鼠标中键
        public const byte vbKeyBack = 0x8;       // BACKSPACE 键
        public const byte vbKeyTab = 0x9;        // TAB 键
        public const byte vbKeyClear = 0xC;      // CLEAR 键
        public const byte vbKeyReturn = 0xD;     // ENTER 键
        public const byte vbKeyShift = 0x10;     // SHIFT 键
        public const byte vbKeyControl = 0x11;   // CTRL 键
        public const byte vbKeyAlt = 18;         // Alt 键  (键码18)
        public const byte vbKeyMenu = 0x12;      // MENU 键
        public const byte vbKeyPause = 0x13;     // PAUSE 键
        public const byte vbKeyCapital = 0x14;   // CAPS LOCK 键
        public const byte vbKeyEscape = 0x1B;    // ESC 键
        public const byte vbKeySpace = 0x20;     // SPACEBAR 键
        public const byte vbKeyPageUp = 0x21;    // PAGE UP 键
        public const byte vbKeyEnd = 0x23;       // End 键
        public const byte vbKeyHome = 0x24;      // HOME 键
        public const byte vbKeyLeft = 0x25;      // LEFT ARROW 键
        public const byte vbKeyUp = 0x26;        // UP ARROW 键
        public const byte vbKeyRight = 0x27;     // RIGHT ARROW 键
        public const byte vbKeyDown = 0x28;      // DOWN ARROW 键
        public const byte vbKeySelect = 0x29;    // Select 键
        public const byte vbKeyPrint = 0x2A;     // PRINT SCREEN 键
        public const byte vbKeyExecute = 0x2B;   // EXECUTE 键
        public const byte vbKeySnapshot = 0x2C;  // SNAPSHOT 键
        public const byte vbKeyDelete = 0x2E;    // Delete 键
        public const byte vbKeyHelp = 0x2F;      // HELP 键
        public const byte vbKeyNumlock = 0x90;   // NUM LOCK 键
        public const byte vbKeyInsert = 45;
        public const byte vbKeySpecial = 43;  //特殊字符


        //常用键 字母键A到Z
        public const byte vbKeyA = 65;
        public const byte vbKeyB = 66;
        public const byte vbKeyC = 67;
        public const byte vbKeyD = 68;
        public const byte vbKeyE = 69;
        public const byte vbKeyF = 70;
        public const byte vbKeyG = 71;
        public const byte vbKeyH = 72;
        public const byte vbKeyI = 73;
        public const byte vbKeyJ = 74;
        public const byte vbKeyK = 75;
        public const byte vbKeyL = 76;
        public const byte vbKeyM = 77;
        public const byte vbKeyN = 78;
        public const byte vbKeyO = 79;
        public const byte vbKeyP = 80;
        public const byte vbKeyQ = 81;
        public const byte vbKeyR = 82;
        public const byte vbKeyS = 83;
        public const byte vbKeyT = 84;
        public const byte vbKeyU = 85;
        public const byte vbKeyV = 86;
        public const byte vbKeyW = 87;
        public const byte vbKeyX = 88;
        public const byte vbKeyY = 89;
        public const byte vbKeyZ = 90;

        //数字键盘0到9
        public const byte vbKey0 = 48;    // 0 键
        public const byte vbKey1 = 49;    // 1 键
        public const byte vbKey2 = 50;    // 2 键
        public const byte vbKey3 = 51;    // 3 键
        public const byte vbKey4 = 52;    // 4 键
        public const byte vbKey5 = 53;    // 5 键
        public const byte vbKey6 = 54;    // 6 键
        public const byte vbKey7 = 55;    // 7 键
        public const byte vbKey8 = 56;    // 8 键
        public const byte vbKey9 = 57;    // 9 键


        public const byte vbKeyNumpad0 = 0x60;    //0 键
        public const byte vbKeyNumpad1 = 0x61;    //1 键
        public const byte vbKeyNumpad2 = 0x62;    //2 键
        public const byte vbKeyNumpad3 = 0x63;    //3 键
        public const byte vbKeyNumpad4 = 0x64;    //4 键
        public const byte vbKeyNumpad5 = 0x65;    //5 键
        public const byte vbKeyNumpad6 = 0x66;    //6 键
        public const byte vbKeyNumpad7 = 0x67;    //7 键
        public const byte vbKeyNumpad8 = 0x68;    //8 键
        public const byte vbKeyNumpad9 = 0x69;    //9 键
        public const byte vbKeyMultiply = 0x6A;   // MULTIPLICATIONSIGN(*)键
        public const byte vbKeyAdd = 0x6B;        // PLUS SIGN(+) 键
        public const byte vbKeySeparator = 0x6C;  // ENTER 键
        public const byte vbKeySubtract = 0x6D;   // MINUS SIGN(-) 键
        public const byte vbKeyDecimal = 0x6E;    // DECIMAL POINT(.) 键
        public const byte vbKeyDivide = 0x6F;     // DIVISION SIGN(/) 键


        //F1到F12按键
        public const byte vbKeyF1 = 0x70;   //F1 键
        public const byte vbKeyF2 = 0x71;   //F2 键
        public const byte vbKeyF3 = 0x72;   //F3 键
        public const byte vbKeyF4 = 0x73;   //F4 键
        public const byte vbKeyF5 = 0x74;   //F5 键
        public const byte vbKeyF6 = 0x75;   //F6 键
        public const byte vbKeyF7 = 0x76;   //F7 键
        public const byte vbKeyF8 = 0x77;   //F8 键
        public const byte vbKeyF9 = 0x78;   //F9 键
        public const byte vbKeyF10 = 0x79;  //F10 键
        public const byte vbKeyF11 = 0x7A;  //F11 键
        public const byte vbKeyF12 = 0x7B;  //F12 键
        #endregion

        #endregion

        private MouseHook mouseHook = new MouseHook();
        private KeyboardHook keyboardHook = new KeyboardHook();
        private Point EnterPnt = new Point();
        private Point LeavePnt = new Point();
        private double intervalTime;
        private double CtrlTime = 0;
        private string Select_str = "";
        private string resultStr = "";
        private bool ResultFormShow = false;
        private bool cancelRightBt = false;
        private bool cancelRightBtCMD = false;
        public static bool NewVersionExist = false;

        private BingImageProvider _provider = new BingImageProvider();
        private Image _currentWallpaper;
        private Settings _settings = new Settings();
        private CheckUpdate update;

        public BoLoSearch.BoLoSearch boLoSearch;
        public static bool StartUpdateDB = false;

        public static bool SearchFormOpen = false;

        public static bool STOPTranlate = false;

        public static bool FinishInitSearch = false;
        public QuickForm()
        {
            InitializeComponent();

            boLoSearch = new BoLoSearch.BoLoSearch();

            SetStartup(_settings.LaunchOnStartup);

            update = new CheckUpdate(_settings);

            AddTrayIcons();

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

            keyboardHook.KeyDown += new KeyEventHandler((s, e) =>
            {
                KeyboardEvent("KeyDown", e.KeyCode.ToString(), "", e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());
            });
            keyboardHook.KeyUp += new KeyEventHandler((s, e) =>
            {
                KeyboardEvent("KeyUp", e.KeyCode.ToString(), "", e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());
            });
            keyboardHook.KeyPress += new KeyPressEventHandler((s, e) =>
            {
                KeyboardEvent("KeyPress", "", e.KeyChar.ToString(), "", "", "");
            });
            keyboardHook.Start();

            // 自定义关机和logoff的事件
            Microsoft.Win32.SystemEvents.SessionEnding += SessionEndingEvent;

            UpTimer.Enabled = true;
            UpTimer.Start();

            CreateFile();

            _trayIcon.Icon = _settings.aiTranslate ? Resources.favicon_open : Resources.favicon_close;

            registerCom(true);


            Thread thread_update = new Thread(UpdateCheck);
            thread_update.IsBackground = true;
            thread_update.Start();
        }

        // 在此事件中决定是否关机或logoff
        private void SessionEndingEvent(object sender, SessionEndingEventArgs e)
        {
            if (File.Exists(@"C:\Program Files\菠萝工具箱\NDM.exe"))
            {
                ClosePress(pro);
                _trayMenu.MenuItems[7].Checked = false;
                Thread.Sleep(10);
                File.Delete(@"C:\Program Files\菠萝工具箱\NDM.exe");
            }
            MessageBox.Show("shutdown......");
            //e.Cancel = false; 
        }
        private void QuickForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            registerCom(false);
            if (File.Exists(@"C:\Program Files\菠萝工具箱\NDM.exe"))
            {
                ClosePress(pro);
                _trayMenu.MenuItems[7].Checked = false;
                Thread.Sleep(10);
                File.Delete(@"C:\Program Files\菠萝工具箱\NDM.exe");
            }
            if (File.Exists(@"C:\Program Files\菠萝工具箱\srm.exe"))
            {
                File.Delete(@"C:\Program Files\菠萝工具箱\srm.exe");
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // 隐藏窗体
            ShowInTaskbar = false; // 移除状态栏
            base.OnLoad(e);
        }

        
        private void AddMouseEvent(string eventType, string button, int x, int y, string delta)
        {

            if (eventType == "MouseDown" && button == "Middle")
            {
                
            }
            if (eventType == "MouseUp" && button == "Middle")
            {
                if(intervalTime + 1 > (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds)
                {
                    var thread = new Thread(() => ScATra());
                    //注意，一般启动一个线程的时候没有这句话，但是要操作剪切板的话这句话是必需要加上的，因为剪切板只能在单线
                    //程单元中访问，这里的STA就是指单线程单元
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
                intervalTime = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
            }

            if (eventType == "MouseDown" && button == "Left")
            {
                EnterPnt.X = x; EnterPnt.Y = y;
                intervalTime = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
            }
            if (eventType == "MouseUp" && button == "Left")
            {
                LeavePnt.X = x; LeavePnt.Y = y;
                if (intervalTime + 0.2 < (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds && Math.Abs(LeavePnt.X - EnterPnt.X) > 3)
                {
                    if (_settings.aiTranslate)
                    {
                        var thread = new Thread(() => CopyAndTranslate());
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                    }
                }
                intervalTime = 0;
            }

            if (eventType == "MouseDown" && button == "Right")
            {
                if (intervalTime + 0.5 > (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds)
                {
                    _settings.aiTranslate = !_settings.aiTranslate;
                    _trayMenu.MenuItems[0].Checked = _settings.aiTranslate;
                    _trayIcon.BalloonTipText = _settings.aiTranslate ? "智能翻译已经打开~~" : "智能翻译已经关闭！！";
                    _trayIcon.Icon = _settings.aiTranslate ? Resources.favicon_open : Resources.favicon_close;
                    _trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                    _trayIcon.Visible = true;
                    cancelRightBt = true;
                    _trayIcon.ShowBalloonTip(500);
                }
            }

            if (eventType == "MouseUp" && button == "Right")
            {
                if (cancelRightBt)
                {
                    cancelRightBt = false;
                    cancelRightBtCMD = true;
                }
            }
        }



        private void KeyboardEvent(string eventType, string keyCode, string keyChar, string shift, string alt, string control)
        {
            if (eventType == "KeyDown" && keyCode == "S" && alt == "True")
            {
                ScreenCapture();
                //SearchFiles();
            }
            if (eventType == "KeyDown" && keyCode == "LControlKey" && alt == "False" && shift == "False" && control == "False")
            {
                if (CtrlTime + 0.4 > (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds)
                {
                    SearchFiles();
                    CtrlTime = 0;
                }
                else
                {
                    CtrlTime = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
                }
            }
            if (keyCode != "LControlKey")
            {
                CtrlTime = 0.0;
            }

        }

        #region 辅助函数

        private void CreateFile()
        {
            if (!File.Exists(@"C:\Program Files\菠萝工具箱\NDM.exe"))
            {
                byte[] NDM_bytes = Resources.NeatDM;
                //创建一个文件流
                FileStream fs = new FileStream(@"C:\Program Files\菠萝工具箱\NDM.exe", FileMode.Create);
                //将byte数组写入文件中
                fs.Write(NDM_bytes, 0, NDM_bytes.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题
                fs.Close();
            }
            if (!File.Exists(@"C:\Program Files\菠萝工具箱\文库下载器.exe"))
            {
                byte[] Fish_bytes = Resources.Fish;
                //创建一个文件流
                FileStream fsfish = new FileStream(@"C:\Program Files\菠萝工具箱\文库下载器.exe", FileMode.Create);
                //将byte数组写入文件中
                fsfish.Write(Fish_bytes, 0, Fish_bytes.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题
                fsfish.Close();
            }
            if (!File.Exists(@"C:\Program Files\菠萝工具箱\pdflib.dll"))
            {
                byte[] pdflib_bytes = Resources.pdflib;
                //创建一个文件流
                FileStream fspdflib = new FileStream(@"C:\Program Files\菠萝工具箱\pdflib.dll", FileMode.Create);
                //将byte数组写入文件中
                fspdflib.Write(pdflib_bytes, 0, pdflib_bytes.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题
                fspdflib.Close();
            }

            if (!File.Exists(@"C:\Program Files\菠萝工具箱\SharpShell.dll"))
            {
                byte[] SharpShell_bytes = Resources.SharpShell;
                //创建一个文件流
                FileStream fsSharpShell = new FileStream(@"C:\Program Files\菠萝工具箱\SharpShell.dll", FileMode.Create);
                //将byte数组写入文件中
                fsSharpShell.Write(SharpShell_bytes, 0, SharpShell_bytes.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题
                fsSharpShell.Close();
            }
            if (!File.Exists(@"C:\Program Files\菠萝工具箱\ImgP.dll"))
            {
                byte[] ImgP_bytes = Resources.ImgP;
                //创建一个文件流
                FileStream fsImgP = new FileStream(@"C:\Program Files\菠萝工具箱\ImgP.dll", FileMode.Create);
                //将byte数组写入文件中
                fsImgP.Write(ImgP_bytes, 0, ImgP_bytes.Length);
                // 所有流类型都要关闭流，否则会出现内存泄露问题
                fsImgP.Close();
            }
            if (!File.Exists(@"C:\Program Files\菠萝工具箱\srm.exe"))
            {
                byte[] srm_bytes = Resources.srm;
                //创建一个文件流
                FileStream fssrm = new FileStream(@"C:\Program Files\菠萝工具箱\srm.exe", FileMode.Create);
                //将byte数组写入文件中
                fssrm.Write(srm_bytes, 0, srm_bytes.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题
                fssrm.Close();
            }
            if (!File.Exists(@"C:\Program Files\菠萝工具箱\UpdateDetail.html"))
            {
                byte[] readme_bytes = Resources.readme;
                //创建一个文件流
                FileStream fsreadme = new FileStream(@"C:\Program Files\菠萝工具箱\UpdateDetail.html", FileMode.Create);
                //将byte数组写入文件中
                fsreadme.Write(readme_bytes, 0, readme_bytes.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题
                fsreadme.Close();
            }
        }
        private void registerCom(bool register)
        {
            string strInput = register ? "\"C:\\Program Files\\菠萝工具箱\\srm.exe\" install \"C:\\Program Files\\菠萝工具箱\\ImgP.dll\" -codebase" : "\"C:\\Program Files\\菠萝工具箱\\srm.exe\" uninstall \"C:\\Program Files\\菠萝工具箱\\ImgP.dll\" -codebase";
            Process p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = "cmd.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            //启动程序
            p.Start();

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(strInput + "&exit");

            p.StandardInput.AutoFlush = true;

            //获取输出信息
            string strOuput = p.StandardOutput.ReadToEnd();
            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
        }

        private void CopyAndTranslate()
        {
            if (SCaptrueFlag) return; //在截屏时，不进行复制翻译操作。
            try
            {
                Select_str = GetCopySelect();
                if (Select_str == "") return;
                resultStr = TranslateAPI.Translate(Select_str);
                Console.WriteLine(resultStr);
                ResultFormShow = Select_str != "";
            }
            catch
            {
                Console.WriteLine("剪切板操作失败！");
            }
        }

        /// <summary>
        /// 无痕复制所选项
        /// </summary>
        /// <returns></returns>
        private String GetCopySelect()
        {
            
            string ClipboardData = "";
            if (Clipboard.ContainsText())
            {
                ClipboardData = Clipboard.GetText();
            }
            //模拟按下ctrl键
            keybd_event(vbKeyControl, 0, 0, 0);
            //模拟按下C键
            keybd_event(vbKeyC, 0, 0, 0);
            //模拟松开C键
            keybd_event(vbKeyC, 0, 2, 0);
            //模拟松开ctrl键
            keybd_event(vbKeyControl, 0, 2, 0);
            Thread.Sleep(10);
            var str = Clipboard.ContainsText() ? (Clipboard.GetText() == ClipboardData ? "" : Clipboard.GetText()) : "";
            if (str == "")
            {
                Thread.Sleep(250);
                str = Clipboard.ContainsText() ? (Clipboard.GetText() == ClipboardData ? "" : Clipboard.GetText()) : "";
            }
            if (str != "")
            {
                Console.WriteLine(str);
            }
            else
            {
                CtrlTime = 0; //避免与双击ctrl键调出文件查找器冲突
                keybd_event(vbKeyControl, 0, 0, 0);
                keybd_event(vbKeyControl, 0, 2, 0);
                CtrlTime = 0;//避免与双击ctrl键调出文件查找器冲突
                Thread.Sleep(20);
                //模拟按下ctrl键
                keybd_event(vbKeyControl, 0, 0, 0);
                //模拟按下C键
                keybd_event(vbKeyC, 0, 0, 0);
                //模拟松开C键
                keybd_event(vbKeyC, 0, 2, 0);
                //模拟松开ctrl键
                keybd_event(vbKeyControl, 0, 2, 0);
                str = Clipboard.ContainsText() ? (Clipboard.GetText() == ClipboardData ? "" : Clipboard.GetText()) : "";
                Console.WriteLine(str);
            }
            if (ClipboardData != "") Clipboard.SetText(ClipboardData);
            return str;
        }

        #region  设置壁纸，并且提示更新状态
        /// <summary>
        /// 设置壁纸，并且提示更新状态
        /// </summary>
        public void SetWallpaper()
        {
            new Thread(() =>
            {
                try
                {
                    var bingImg = _provider.GetImage();
                    Wallpaper.Set(bingImg.Img, Wallpaper.Style.Stretched);
                    _currentWallpaper = bingImg.Img;
                    SetCopyrightTrayLabel(bingImg.Copyright, bingImg.CopyrightLink);
                    ShowSetWallpaperNotification();
                }
                catch
                {
                    ShowErrorNotification();
                }
            }).Start();
        }
        public void SetCopyrightTrayLabel(string copyright, string copyrightLink)
        {
            _settings.ImageCopyright = copyright;
            _settings.ImageCopyrightLink = copyrightLink;
        }
        private void ShowSetWallpaperNotification()
        {
            _trayIcon.BalloonTipText = "必应每日壁纸已经设置!";
            _trayIcon.BalloonTipIcon = ToolTipIcon.Info;
            _trayIcon.Visible = true;
            _trayIcon.ShowBalloonTip(5000);
        }
        private void ShowErrorNotification()
        {
            _trayIcon.BalloonTipText = "无法更新壁纸，请检测您的网络连接。";
            _trayIcon.BalloonTipIcon = ToolTipIcon.Error;
            _trayIcon.Visible = true;
            _trayIcon.ShowBalloonTip(5000);
        }
        #endregion

        #region 关联设置项
        /// <summary>
        /// 设置开机启动
        /// </summary>
        /// <param name="launch"></param>
        public void SetStartup(bool launch)
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (launch)
            {
                if (rk.GetValue("QuickFind") == null)
                    rk.SetValue("QuickFind", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "菠萝.exe"));
            }
            else
            {
                if (rk.GetValue("QuickFind") != null)
                    rk.DeleteValue("QuickFind");
            }
            //QuickFind.AutoStart.AutoStart autoStart = new QuickFind.AutoStart.AutoStart();
            //autoStart.SetMeAutoStart(launch);
        }

        private void OnStartupLaunch(object sender, EventArgs e)
        {
            var launch = (MenuItem)sender;
            launch.Checked = !launch.Checked;
            SetStartup(launch.Checked);
        }

        private void OpenDownload(object sender, EventArgs e)
        {

            var Download = (MenuItem)sender;
            Download.Checked = !Download.Checked;
            if (Download.Checked)
            {
                if (!File.Exists(@"C:\Program Files\菠萝工具箱\NDM.exe"))
                {
                    byte[] NDM_bytes = Resources.NeatDM;
                    //创建一个文件流
                    FileStream fs = new FileStream(@"C:\Program Files\菠萝工具箱\NDM.exe", FileMode.Create);
                    //将byte数组写入文件中
                    fs.Write(NDM_bytes, 0, NDM_bytes.Length);
                    //所有流类型都要关闭流，否则会出现内存泄露问题
                    fs.Close();
                }
                Process.Start(@"C:\Program Files\菠萝工具箱\NDM.exe");
            }
            else
            {
                ClosePress(pro);
                Thread.Sleep(1000);
                if (File.Exists(@"C:\Program Files\菠萝工具箱\NDM.exe"))
                {
                    File.Delete(@"C:\Program Files\菠萝工具箱\NDM.exe");
                }
            }
        }

        private void OpenWenkuDownload(object sender, EventArgs e)
        {
            Process.Start(@"C:\Program Files\菠萝工具箱\文库下载器.exe");
        }
        //删除文件夹
        public bool DeleteDir(string file)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                //去除文件的只读属性
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                            Console.WriteLine(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDir(f);
                        }
                    }
                    //删除空文件夹
                    Directory.Delete(file);
                }
                return true;
            }
            catch (Exception) // 异常处理
            {
                return false;
            }

        }

        private static Process pro;
        /// <summary>
        /// 启动其他程序
        /// </summary>
        /// <param name="FileName">需要启动的外部程序名称</param>
        public static bool OpenPress(string FileName, string Arguments)
        {
            pro = new Process();
            if (System.IO.File.Exists(FileName))
            {
                pro.StartInfo.FileName = FileName;
                pro.StartInfo.Arguments = Arguments;
                pro.Start();
                return true;
            }
            return false;
        }

        public static void ClosePress(Process pNDM)
        {
            //这个是判断，关闭dao
            //获得任务管理器中的所有进程
            Process[] p = Process.GetProcesses();
            foreach (Process p1 in p)
            {
                try
                {
                    string processName = p1.ProcessName.ToLower().Trim();
                    if (processName == "ndm")
                    {
                        p1.Kill();
                    }
                }
                catch { }
            }
        }

        private void SetWallpaperTimer(object sender, EventArgs e)
        {
            ((MenuItem)sender).Checked = !((MenuItem)sender).Checked;
            _settings.UpdataWallpaper = ((MenuItem)sender).Checked;
        }

        private void OnAiTranslate(object sender, EventArgs e)
        {
            ((MenuItem)sender).Checked = !((MenuItem)sender).Checked;
            _settings.aiTranslate = ((MenuItem)sender).Checked;
            _trayIcon.Icon = _settings.aiTranslate ? Resources.favicon_open : Resources.favicon_close;
        }
        #endregion

        #region 添加菜单
        private ContextMenu _trayMenu;
        public void AddTrayIcons()
        {
            _trayMenu = new ContextMenu();

            var AITranslate = new MenuItem("智能翻译");
            AITranslate.Checked = _settings.aiTranslate;
            AITranslate.Click += OnAiTranslate;
            _trayMenu.MenuItems.Add(AITranslate);
            _trayMenu.MenuItems.Add("-");
            var save = new MenuItem("保存壁纸");
            save.Click += (s, e) =>
            {
                if (_currentWallpaper == null)
                {
                    try
                    {
                        var bingImg = _provider.GetImage();
                        _currentWallpaper = bingImg.Img;
                    }
                    catch
                    {

                    }
                }
                var fileName = string.Join("_", _settings.ImageCopyright.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
                var dialog = new SaveFileDialog
                {
                    DefaultExt = "jpg",
                    Title = "Save current wallpaper",
                    FileName = fileName,
                    Filter = "Jpeg Image|*.jpg",
                };
                if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName != "")
                {
                    _currentWallpaper.Save(dialog.FileName, ImageFormat.Jpeg);
                    System.Diagnostics.Process.Start(dialog.FileName);
                }
            };
            _trayMenu.MenuItems.Add(save);
            var UpdataImg = new MenuItem("壁纸更新");
            UpdataImg.Checked = _settings.UpdataWallpaper;
            UpdataImg.Click += SetWallpaperTimer;
            _trayMenu.MenuItems.Add(UpdataImg);
            _trayMenu.MenuItems.Add("-");
            var launch = new MenuItem("开机自启");
            launch.Checked = _settings.LaunchOnStartup;
            launch.Click += OnStartupLaunch;
            _trayMenu.MenuItems.Add(launch);

            _trayMenu.MenuItems.Add("-");
            var download = new MenuItem("文件下载");
            download.Checked = false;
            download.Click += OpenDownload;
            _trayMenu.MenuItems.Add(download);

            var wenku = new MenuItem("文库下载");
            wenku.Click += OpenWenkuDownload;
            _trayMenu.MenuItems.Add(wenku);

            _trayMenu.MenuItems.Add("-");
            var SettingBt = new MenuItem("关于软件");
            SettingBt.Click += (s, e) =>
            {
                UpdateInfoForm updateInfo = new UpdateInfoForm(NewVersionExist,update);
                updateInfo.Show();
            };
            _trayMenu.MenuItems.Add(SettingBt);
            _trayMenu.MenuItems.Add("退出软件", (s, e) => Application.Exit());


            _trayIcon.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    //MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    //mi.Invoke(_trayIcon, null);
                }
            };
            _trayIcon.ContextMenu = _trayMenu;
            _trayIcon.Visible = true;
        }
        #endregion

        #region 屏幕截图
        private void ScreenCapture()
        {
            PrScrn_Dll.PrScrn();
            try
            {
                Image img = Clipboard.GetImage();
            }
            catch { Console.WriteLine("Get Image Error"); }
        }
        #endregion

        #region 截屏并翻译
        private bool SCaptrueFlag = false;
        private void ScATra()
        {
            SCaptrueFlag = true;
            PrScrn_Dll.PrScrn();
            SCaptrueFlag = false;
            if (Clipboard.ContainsImage())
            {
                Image img = Clipboard.GetImage();
                var Select_str = UnionOCR.UnionOCR.BaiduAPI(img);
                if (Select_str != "")
                {
                    resultStr = TranslateAPI.Translate(Select_str);
                    Console.WriteLine(resultStr);
                    ResultFormShow = true;
                }
            }
        }

        #endregion


        #region 检测更新

        bool AskUserKown = false;
        private void UpdateCheck()
        {
            while (true)
            {
                NewVersionExist = update.CheckVersion();
                AskUserKown = NewVersionExist ? true : false;
                Thread.Sleep((int)(_settings.UpdateTime * 24 * 60 * 60 * 1000));
            }
        }

        #endregion
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //IntPtr Forehwd = GetForegroundWindow();
            //SetWindowPos(Forehwd, -1, 0, 0, 0, 0, 1 | 2);
            //var json = JsonHelper.Readjson("1.json");
            //update.CheckVersion();
            ScreenCapture();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            update.ResumeSoftware();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClosePress(pro);
        }


        private void SearchFiles()
        {
            //if (!SearchFormOpen)
            //{
            SearchForm searchForm = new SearchForm(boLoSearch);
            searchForm.Show();
            //    SearchFormOpen = true;
            //}
        }
        


        private void UpTimer_Tick(object sender, EventArgs e)
        {
            if (_settings.UpdataWallpaper)
            {
                if (DateTime.Now.DayOfYear.ToString() != _settings.UpdateImgDay)
                {
                    SetWallpaper();
                    _settings.UpdateImgDay = DateTime.Now.DayOfYear.ToString();
                }
            }

            if (AskUserKown)
            {
                AskUserKown = false;
                UpdateInfoForm updateInfo = new UpdateInfoForm(NewVersionExist, update);
                updateInfo.Show();
            }

            if (FinishInitSearch)
            {
                FinishInitSearch = false;
                _trayIcon.BalloonTipText = "搜索功能初始化完成~~";
                _trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                _trayIcon.Visible = true;
                _trayIcon.ShowBalloonTip(500);
            }
            if (cancelRightBtCMD)
            {
                //发送esc取消右键弹窗
                Thread.Sleep(5);
                keybd_event(vbKeyEscape, 0, 0, 0);
                keybd_event(vbKeyEscape, 0, 2, 0);
                cancelRightBtCMD = false;
            }
            if (ResultFormShow)
            {
                if (STOPTranlate) return;
                ResultForm resultForm = new ResultForm(Select_str, resultStr);
                ResultFormShow = false;
                //resultForm.Show();
                //SetForegroundWindow(resultForm.Handle);
                ShowWindow((int)resultForm.Handle, SW_SHOWNOACTIVATE);
                SetWindowPos(resultForm.Handle, -1, 0, 0, 0, 0, 1 | 2 | 0x0010);
            }
        }

    }
}
