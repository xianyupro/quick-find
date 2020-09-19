#define SHOW_TEST_MSG
using QuickFind;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using TranslateApi;
using VideoAnalysis.Common;

namespace ScreenShoter
{
    public partial class FormShot : Form
    {
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        //根据坐标获取窗口句柄
        [DllImport("user32")]
        private static extern IntPtr WindowFromPoint(Point Point);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left; //最左坐标
            public int Top; //最上坐标
            public int Right; //最右坐标
            public int Bottom; //最下坐标
        }
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point lpPoint);


        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        public struct WindowInfo
        {
            public IntPtr hWnd;
            public string szWindowName;
            public string szClassName;
        }

        public FormShot()
        {
            InitializeComponent();


            _toolBtn = new ScreenShoter.ToolButtons();
            _toolBtn.Parent = this;            

            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.KeyPreview = true;
            //this.TopMost = true;

            this.KeyDown += Form1_KeyDown;
            this.Paint += Form1_Paint;

            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;  

            this.MouseDoubleClick += Form1_MouseDoubleClick;

            this.Load += Form1_Load;

            Init(); 
            
        }
        
        private ToolButtons _toolBtn;
        private Image _toolBtnImg;
        private bool showToolBt = false;
        private Rectangle _rect;
        private Dictionary<string,Rectangle> _dicRects
        {
            get
            {
                return new Dictionary<string, Rectangle> {
                    {"NW", new Rectangle(_rect.X-2,_rect.Y-2,5,5)                           },
                    {"N" , new Rectangle(_rect.X+_rect.Width/2-2,_rect.Y-2,5,5)             },
                    {"NE", new Rectangle(_rect.X+_rect.Width-2,_rect.Y-2,5,5)               },
                    {"W" , new Rectangle(_rect.X-2,_rect.Y+_rect.Height/2-2,5,5)            },
                    {"E" , new Rectangle(_rect.X+_rect.Width-2,_rect.Y+_rect.Height/2-2,5,5)},
                    {"SW", new Rectangle(_rect.X-2,_rect.Y+_rect.Height-2,5,5)              },
                    {"S" , new Rectangle(_rect.X+_rect.Width/2-2,_rect.Y+_rect.Height-2,5,5)},
                    {"SE", new Rectangle(_rect.X+_rect.Width-2,_rect.Y+_rect.Height-2,5,5)  },
                };
            }
        }
        private bool _mouseDown = false;
        private Point? _startPos = null;
        private Rectangle? _rectBeforeMove = null;
        private string _corner = "";

        public string _TEST { get; set; }

        Bitmap ScreenBitmap;
        private void Init()
        {
            var img = PrintScreen();
            this.BackgroundImage = img;
            ScreenBitmap = new Bitmap(img);
            Cursor = Cursors.Arrow;


        }



        private Image PrintScreen()
        {
            Image img = new Bitmap(this.Size.Width, this.Size.Height);
            var g = Graphics.FromImage(img);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), this.Size);
            g.Dispose();
            return img;
        }

        private bool RectContainsPt(Rectangle rect, Point point)
        {
            return point.X >= rect.X && point.Y >= rect.Y && point.X <= (rect.X + rect.Width) && point.Y <= (rect.Y + rect.Height);
        }

        private void Draw(Graphics g)
        {
            Region reg = new Region(new RectangleF(new Point(0, 0), this.Size));         
            if (_rect.Width > 0 || _rect.Height > 0)
            {
                g.DrawRectangle(new Pen(Color.FromArgb(0, 197, 205), 3), _rect);
                reg.Xor(_rect);
                if (_toolBtn.Visible)
                    reg.Xor(new Rectangle(_toolBtn.Location, _toolBtn.Size));
                
                g.FillRegion(new SolidBrush(Color.FromArgb(100, 40, 40, 40)), reg);
                g.FillRectangles(new SolidBrush(Color.Blue), _dicRects.Values.Select(x => x).ToArray());
                
                if(showToolBt) DrawToolBtn(g);
                DrawTip(g);
            }
            else
            {
                _toolBtn.Hide();
                g.FillRegion(new SolidBrush(Color.FromArgb(0, 200, 200, 200)), reg);
                g.DrawRectangle(new Pen(Color.FromArgb(0, 197 ,205),8),new Rectangle(new Point(0, 0),new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)));
            }
            if (!showToolBt&& Cursor != Cursors.SizeAll)
            {
                
                var lpPoint = new Point();
                GetCursorPos(ref lpPoint);
                var B_W = 30; var B_H = 22; var B_Spoint = new Point(lpPoint.X, lpPoint.Y + 25);
                var L_start_p = lpPoint.X - B_W / 2 < 0 ? 0 : lpPoint.X + B_W / 2 > this.Width ? this.Width - B_W : lpPoint.X - B_W / 2;
                var T_start_p = lpPoint.Y - B_H / 2 < 0 ? 0 : lpPoint.Y + B_H / 2 > this.Height ? this.Height - B_H : lpPoint.Y - B_H / 2;
                Rectangle cropRect = new Rectangle(L_start_p, T_start_p, B_W, B_H);
                Bitmap target = new Bitmap(B_W * 4, B_H * 4);
                using (Graphics gx = Graphics.FromImage(target))
                {
                    gx.DrawImage(ScreenBitmap, new Rectangle(0, 0, target.Width, target.Height),
                          cropRect,
                          GraphicsUnit.Pixel);
                }
                g.DrawImage(target, B_Spoint);
                g.DrawLine(new Pen(Color.FromArgb(0, 197, 205), 2), new PointF(B_Spoint.X + target.Width / 2, B_Spoint.Y), new PointF(B_Spoint.X + target.Width / 2, B_Spoint.Y + target.Height));
                g.DrawLine(new Pen(Color.FromArgb(0, 197, 205), 2), new PointF(B_Spoint.X, B_Spoint.Y + target.Height / 2), new PointF(B_Spoint.X + target.Width, B_Spoint.Y + target.Height / 2));
                g.DrawRectangle(new Pen(Color.FromArgb(255, 255, 255), 3), new Rectangle(B_Spoint, new Size(target.Width, target.Height)));
                g.FillRegion(new SolidBrush(Color.FromArgb(180, 40, 40, 40)), new Region(new Rectangle(new Point(B_Spoint.X-2, B_Spoint.Y+target.Height ), new Size(target.Width+4, 78))));
                g.DrawString(string.Format("({0} X {1})",lpPoint.X, lpPoint.Y), new Font("微软雅黑", 9), new SolidBrush(Color.White), new Point(B_Spoint.X + 2, B_Spoint.Y + target.Height + 4));
                g.DrawString(string.Format("RGB:({0},{1},{2})", ScreenBitmap.GetPixel(lpPoint.X,lpPoint.Y).R, ScreenBitmap.GetPixel(lpPoint.X, lpPoint.Y).G, ScreenBitmap.GetPixel(lpPoint.X, lpPoint.Y).B),new Font("微软雅黑", 9), new SolidBrush(Color.White), new Point(B_Spoint.X+2, B_Spoint.Y + target.Height+22));
                g.DrawString(string.Format("按C复制色号"), new Font("微软雅黑", 9), new SolidBrush(Color.White), new Point(B_Spoint.X + 2, B_Spoint.Y + target.Height + 40));
                g.DrawString(string.Format("鼠标左键自由截图"), new Font("微软雅黑", 9), new SolidBrush(Color.White), new Point(B_Spoint.X + 2, B_Spoint.Y + target.Height + 58));
            }
            
        }


        private void DrawToolBtn(Graphics g)
        {
            _toolBtn.Hide();
            if (_rect.Width > 0 || _rect.Height > 0)
            {
                var margin = 5;
                var tw = _toolBtn.Width;
                var th = _toolBtn.Height;
                if (th + margin + _rect.Location.Y + _rect.Height <= this.Height)
                {
                    //toolbar 在底部
                    var p = new Point(_rect.X+ _rect.Width - tw, _rect.Y + _rect.Height + margin);
                    if (p.X < 0)
                    {
                        p = new Point(0, p.Y);
                    }
                    if (_mouseDown)
                    {
                        g.DrawImage(_toolBtnImg, p);
                    }
                    else
                    {
                        _toolBtn.Show();
                        _toolBtn.Location = p;
                    }
                }
                else
                {
                    var p = new Point(_rect.X + _rect.Width - tw, th + margin > _rect.Y ? 0 : _rect.Y - margin - th);
                    if (p.X < 0)
                    {
                        p = new Point(0, p.Y);
                    }
                    if (_mouseDown)
                    {
                        g.DrawImage(_toolBtnImg, p);
                    }  
                    else
                    {
                        _toolBtn.Show();
                        _toolBtn.Location = p;
                    }
                }
            }
        }

        private void DrawTip(Graphics g)
        {
            if (_rect.Height == 0 && _rect.Width == 0)
            {
                return;
            }
            var rect = new Rectangle(_rect.X , _rect.Y - 22, 70, 20);
            if (_rect.Width < rect.Width || _rect.Height < rect.Height)
            {
                if (_rect.Y > 23)
                {
                    rect = new Rectangle(_rect.X, _rect.Y - 3 - rect.Height, 70, 20);
                }
            }
            g.FillRectangle(new SolidBrush(Color.FromArgb(120, 0, 0, 0)), rect);
            g.DrawString(string.Format("{0}×{1}", _rect.Width + 1, _rect.Height + 1),
                new Font("微软雅黑", 8),
                new SolidBrush(Color.White),
                rect,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }

        public void ShowParent()
        {
            this.Hide();
            //this.Visible = false;
            //(this.Owner as FormMain).Show();
            this.Close();
            this.Dispose();
        }

        #region - EVENTS -

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_toolBtnImg == null)
            {
                _toolBtnImg = new Bitmap(_toolBtn.Width, _toolBtn.Height);
                _toolBtn.DrawToBitmap((Bitmap)_toolBtnImg, new Rectangle(_toolBtn.Location, _toolBtn.Size));
                _toolBtn.Hide();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ShowParent();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.Invalidate();
            if (_toolBtn.Visible)
            {
                _toolBtn.Refresh();
            }
            Draw(e.Graphics);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseDown = false;
                _startPos = null;
                _rectBeforeMove = null;
                showToolBt = true;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseDown = true;
                _startPos = new Point(e.X, e.Y);
                showToolBt = false;
            }
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((_rect.Width > 0 || _rect.Height > 0) && e.Button == MouseButtons.Left)
            {
                SaveToClipboard();
                ShowParent();
            }
        }

        public void SaveToClipboard(string fileName = "")
        {
            var rect = new Rectangle(_rect.X, _rect.Y, _rect.Width + 1, _rect.Height + 1);
            using (var img = new Bitmap(rect.Width, rect.Height))
            {
                using (var g = Graphics.FromImage(img))
                {
                    g.DrawImage(this.BackgroundImage,
                        new Rectangle(0, 0, rect.Width, rect.Height),
                        rect,
                        GraphicsUnit.Pixel);
                    if (string.IsNullOrEmpty(fileName))
                    {
                        Clipboard.SetImage(img);
                    }
                    else
                    {
                        img.Save(fileName);
                    }
                }
            }
        }

        public string OCR_Text()
        {
            LoadingHelper.ShowLoadingScreen();//显示
            
            var OCRresult = "";
            string token = "24.5142d6d4850be2c098165f677b8fe68b.2592000.1603087490.282335-22701901";
            //string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token=" + token;
            string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/accurate_basic?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.Timeout = 4000;
            request.KeepAlive = true;

            var rect = new Rectangle(_rect.X, _rect.Y, _rect.Width + 1, _rect.Height + 1);
            using (var img = new Bitmap(rect.Width, rect.Height))
            {
                using (var g = Graphics.FromImage(img))
                {
                    g.DrawImage(this.BackgroundImage,
                        new Rectangle(0, 0, rect.Width, rect.Height),
                        rect,
                        GraphicsUnit.Pixel);
                    try
                    {
                        // 图片的base64编码
                        string base64 = ImageToBase64(img);
                        String str = "image=" + HttpUtility.UrlEncode(base64);
                        byte[] buffer = encoding.GetBytes(str);
                        request.ContentLength = buffer.Length;
                        request.GetRequestStream().Write(buffer, 0, buffer.Length);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                        string result = reader.ReadToEnd();
                        var ocr_result = JsonHelper.ConvertJson(result);
                        for (int i = 0; i < ocr_result["words_result"].Count(); i++)
                        {
                            OCRresult = OCRresult + ocr_result["words_result"][i]["words"].ToString() + "\r\n";
                        }
                        new OCR_Form(img, OCRresult).Show();
                    }
                    catch
                    {
                        Console.WriteLine("网络异常");
                    }
                    finally
                    {
                        LoadingHelper.CloseForm();//关闭
                    }  
                }
            }
            return OCRresult;
        }

        public string OCR_Latx()
        {
            string LaTeXRresult = "";
            try
            {
                LoadingHelper.ShowLoadingScreen();//显示

                var rect = new Rectangle(_rect.X, _rect.Y, _rect.Width + 1, _rect.Height + 1);
                using (var img = new Bitmap(rect.Width, rect.Height))
                {
                    using (var g = Graphics.FromImage(img))
                    {
                        g.DrawImage(this.BackgroundImage,
                            new Rectangle(0, 0, rect.Width, rect.Height),
                            rect,
                            GraphicsUnit.Pixel);

                        Dictionary<string, string> headers = new Dictionary<string, string>();
                        headers.Add("Timeout", "6000");
                        headers.Add("ContentType", "application/json");
                        headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
                        headers.Add("Method", "POST");
                        string LaTeX_MMS_url = "https://www.latexlive.com:5001/api/mathpix/posttomathpix?";
                        var postData = "{\"src\":\"data:image/png;base64," + ImageToBase64(img) + "\"}";

                        Request Response = new Request(LaTeX_MMS_url, headers, postData);
                        var text = Response.GetText();
                        var json_LaTeX_MMS = JsonHelper.ConvertJson(text);
                        LaTeXRresult = json_LaTeX_MMS["latex_styled"]?.ToString();
                        //var Text = json_LaTeX_MMS["text"]?.ToString();
                        
                        if (LaTeXRresult != ""&&!String.IsNullOrEmpty(LaTeXRresult))
                        {
                            Clipboard.SetText(LaTeXRresult);
                            new OCR_Form(img, "公式已经复制到剪切板，使用CTRL+V黏贴至MathType中使用：\r\n-------------\r\n" + LaTeXRresult).Show();
                        }
                        else
                        {
                            new OCR_Form(img, "选中区域无法检测到公式，请重新框定公式。" ).Show();
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            LoadingHelper.CloseForm();//关闭
            return LaTeXRresult;
        }

        public string OCR_Trans()
        {
            LoadingHelper.ShowLoadingScreen();//显示
            var OCRresult = "";
            string token = "24.5142d6d4850be2c098165f677b8fe68b.2592000.1603087490.282335-22701901";
            //string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic?access_token=" + token;
            string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/accurate_basic?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.Timeout = 4000;
            request.KeepAlive = true;

            var rect = new Rectangle(_rect.X, _rect.Y, _rect.Width + 1, _rect.Height + 1);
            using (var img = new Bitmap(rect.Width, rect.Height))
            {
                using (var g = Graphics.FromImage(img))
                {
                    g.DrawImage(this.BackgroundImage,
                        new Rectangle(0, 0, rect.Width, rect.Height),
                        rect,
                        GraphicsUnit.Pixel);
                    try
                    {
                        // 图片的base64编码
                        string base64 = ImageToBase64(img);
                        String str = "image=" + HttpUtility.UrlEncode(base64);
                        byte[] buffer = encoding.GetBytes(str);
                        request.ContentLength = buffer.Length;
                        request.GetRequestStream().Write(buffer, 0, buffer.Length);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                        string result = reader.ReadToEnd();
                        var ocr_result = JsonHelper.ConvertJson(result);
                        for (int i = 0; i < ocr_result["words_result"].Count(); i++)
                        {
                            OCRresult = OCRresult + ocr_result["words_result"][i]["words"].ToString() + "\r\n";
                        }
                        OCRresult = TranslateAPISingle.Translate(OCRresult);
                        new OCR_Form(img, OCRresult).Show();
                    }
                    catch
                    {
                        Console.WriteLine("网络异常");
                    }
                    finally
                    {
                        LoadingHelper.CloseForm();//关闭
                    }
                }
            }
            return OCRresult;
        }

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

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                var setRect = new Action(() =>
                {
                    _rect = new Rectangle(
                        _startPos.Value.X > e.X ? e.X : _startPos.Value.X,
                        _startPos.Value.Y > e.Y ? e.Y : _startPos.Value.Y,
                        Math.Abs(_startPos.Value.X - e.X),
                        Math.Abs(_startPos.Value.Y - e.Y));
                });

                if (Cursor == Cursors.SizeAll)
                {
                    showToolBt = true;
                    if (_rectBeforeMove == null)
                        _rectBeforeMove = _rect;

                    var x = _rectBeforeMove.Value.X + (e.X - _startPos.Value.X);
                    var y = _rectBeforeMove.Value.Y + (e.Y - _startPos.Value.Y);
                    if (x < 0)
                        x = 0;
                    if (y < 0)
                        y = 0;
                    if (x > this.Size.Width - _rect.Width)
                        x = this.Size.Width - _rect.Width;
                    if (y > this.Size.Height - _rect.Height)
                        y = this.Size.Height - _rect.Height;
                    _rect.Location = new Point(x, y);
                }
                else if (Cursor == Cursors.SizeNESW)
                {
                    showToolBt = true;
                    if (_rectBeforeMove == null)
                    {
                        _rectBeforeMove = _rect;
                        _startPos = null;
                    }
                    if (_startPos == null)
                    {
                        var cornerRects = _dicRects.Where(x => RectContainsPt(x.Value, e.Location));
                        if (cornerRects.Any() && cornerRects.First().Key == "NE")
                            _startPos = new Point(_rectBeforeMove.Value.X, _rectBeforeMove.Value.Y + _rectBeforeMove.Value.Height);
                        else
                            _startPos = new Point(_rectBeforeMove.Value.X + _rectBeforeMove.Value.Width, _rectBeforeMove.Value.Y);
                    }
                    setRect();
                }
                else if (Cursor == Cursors.SizeNWSE)
                {
                    showToolBt = true;
                    if (_rectBeforeMove == null)
                    {
                        _rectBeforeMove = _rect;
                        _startPos = null;
                    }
                    if (_startPos == null)
                    {
                        var cornerRects = _dicRects.Where(x => RectContainsPt(x.Value, e.Location));
                        if (cornerRects.Any() && cornerRects.First().Key == "NW")
                            _startPos = new Point(_rectBeforeMove.Value.X + _rectBeforeMove.Value.Width, _rectBeforeMove.Value.Y + _rectBeforeMove.Value.Height);
                        else
                            _startPos = new Point(_rectBeforeMove.Value.X, _rectBeforeMove.Value.Y);
                    }
                    setRect();
                }
                else if (Cursor == Cursors.SizeNS)
                {
                    showToolBt = true;
                    if (_rectBeforeMove == null)
                    {
                        _rectBeforeMove = _rect;
                        _startPos = null;
                    }
                    if (_startPos == null)
                    {
                        _startPos = e.Location;
                    }
                    if (_corner == "N")
                    {
                        _rect = new Rectangle(
                            _rectBeforeMove.Value.X,
                            e.Y < _rectBeforeMove.Value.Y + _rectBeforeMove.Value.Height ?
                                e.Y :
                                _rectBeforeMove.Value.Y + _rectBeforeMove.Value.Height,
                            _rectBeforeMove.Value.Width,
                            e.Y < _rectBeforeMove.Value.Y + _rectBeforeMove.Value.Height ?
                                _rectBeforeMove.Value.Height + _rectBeforeMove.Value.Y - e.Y :
                                e.Y - _rectBeforeMove.Value.Y - _rectBeforeMove.Value.Height);
                    }
                    else
                    {
                        _rect = new Rectangle(
                            _rectBeforeMove.Value.X,
                            e.Y < _rectBeforeMove.Value.Y ? e.Y : _rectBeforeMove.Value.Y,
                            _rectBeforeMove.Value.Width,
                            Math.Abs(e.Y - _rectBeforeMove.Value.Y));
                    }
                }
                else if (Cursor == Cursors.SizeWE)
                {
                    showToolBt = true;
                    if (_rectBeforeMove == null)
                    {
                        _rectBeforeMove = _rect;
                        _startPos = null;
                    }
                    if (_startPos == null)
                    {
                        _startPos = e.Location;
                    }
                    if (_corner == "W")
                    {
                        _rect = new Rectangle(
                           e.X < _rectBeforeMove.Value.X + _rectBeforeMove.Value.Width ?
                               e.X :
                               _rectBeforeMove.Value.X + _rectBeforeMove.Value.Width,
                           _rectBeforeMove.Value.Y,
                           e.X < _rectBeforeMove.Value.X + _rectBeforeMove.Value.Width ?
                               _rectBeforeMove.Value.Width + _rectBeforeMove.Value.X - e.X :
                               e.X - _rectBeforeMove.Value.X - _rectBeforeMove.Value.Width,
                           _rectBeforeMove.Value.Height);
                    }
                    else
                    {
                        _rect = new Rectangle(
                            e.X < _rectBeforeMove.Value.X ? e.X : _rectBeforeMove.Value.X,
                            _rectBeforeMove.Value.Y,
                            Math.Abs(e.X - _rectBeforeMove.Value.X),
                            _rectBeforeMove.Value.Height);
                    }
                }
                else
                {
                    Cursor = Cursors.Cross;
                    _rect = new Rectangle(
                        _startPos.Value.X > e.X ? e.X : _startPos.Value.X,
                        _startPos.Value.Y > e.Y ? e.Y : _startPos.Value.Y,
                        Math.Abs(_startPos.Value.X - e.X),
                        Math.Abs(_startPos.Value.Y - e.Y));
                }
            }
            else
            {
                if (_rect.Width > 0 && _rect.Height > 0)
                {
                    Cursor = Cursors.Arrow;
                    var showSizeAllCursor = true;
                    foreach (var r in _dicRects.Select(x => x.Value))
                    {
                        showSizeAllCursor = showSizeAllCursor && !RectContainsPt(r, e.Location);
                        if (!showSizeAllCursor)
                            break;
                    }
                    if (RectContainsPt(_rect, e.Location) && showSizeAllCursor)
                    {
                        Cursor = Cursors.SizeAll;
                    }
                    else
                    {
                        foreach (KeyValuePair<string, Rectangle> kv in _dicRects)
                        {
                            if (RectContainsPt(kv.Value, e.Location))
                            {
                                _corner = kv.Key;
                                if (kv.Key == "N" || kv.Key == "S")
                                    Cursor = Cursors.SizeNS;
                                if (kv.Key == "W" || kv.Key == "E")
                                    Cursor = Cursors.SizeWE;
                                if (kv.Key == "NW" || kv.Key == "SE")
                                    Cursor = Cursors.SizeNWSE;
                                if (kv.Key == "NE" || kv.Key == "SW")
                                    Cursor = Cursors.SizeNESW;
                            }
                        }
                    }
                }
            }
        }

        #endregion
        RECT Formrect = new RECT();

        List<RECT> FormRects;
        public List<RECT> GetAllDesktopWindows()
        {
            List<WindowInfo> wndList = new List<WindowInfo>();
            List<RECT> wndRect = new List<RECT>();
            //enum all desktop windows
            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                RECT rect = new RECT();
                StringBuilder sb = new StringBuilder(256);
                //get hwnd
                wnd.hWnd = hWnd;
                //get window name
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                //get window class
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                //add it into list
                wndList.Add(wnd);
                GetWindowRect(hWnd, ref rect);
                if((rect.Right-rect.Left)*(rect.Bottom-rect.Top)>1000 && wnd.szClassName!= "ForegroundStaging" && wnd.szWindowName != ""&& IsWindowVisible(hWnd)) wndRect.Add(rect);
                return true;
            }, 0);

            return wndRect;
        }

        private static bool isoverlapped(IntPtr preWin, Form form)
        {
            var ScreenRectangle = Screen.PrimaryScreen.WorkingArea;
            if (preWin == null || preWin == IntPtr.Zero)
                return false;

            if (!IsWindowVisible(preWin))
                return isoverlapped(preWin, form);

            RECT rect = new RECT();
            if (GetWindowRect(preWin, ref rect)) //获取窗体矩形
            {
                Rectangle winrect = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

                if (winrect.Width == ScreenRectangle.Width && winrect.Y == ScreenRectangle.Height) //菜单栏。不判断遮挡（可略过）
                    return isoverlapped(preWin, form);

                if (winrect.X == 0 && winrect.Width == 54 && winrect.Height == 54) //开始按钮。不判断遮挡（可略过）
                    return isoverlapped(preWin, form);

                Rectangle formRect = new Rectangle(form.Location, form.Size); //Form窗体矩形
                if (formRect.IntersectsWith(winrect)) //判断是否遮挡
                    return true;
            }

            return isoverlapped(preWin, form);
        }
    }
}
