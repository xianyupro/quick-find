using QuickFind.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickFind
{
    public partial class LoadingForm : Form
    {
        Image LoadImg;
        public LoadingForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 关闭命令
        /// </summary>
        public void closeOrder()
        {
            if (this.InvokeRequired)
            {
                //这里利用委托进行窗体的操作，避免跨线程调用时抛异常，后面给出具体定义
                CONSTANTDEFINE.SetUISomeInfo UIinfo = new CONSTANTDEFINE.SetUISomeInfo(new Action(() =>
                {
                    while (!this.IsHandleCreated)
                    {
                        ;
                    }
                    if (this.IsDisposed)
                        return;
                    if (!this.IsDisposed)
                    {
                        this.Dispose();
                    }

                }));
                this.Invoke(UIinfo);
            }
            else
            {
                if (this.IsDisposed)
                    return;
                if (!this.IsDisposed)
                {
                    this.Dispose();
                }
            }
        }

        private void LoaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.IsDisposed)
            {
                this.Dispose(true);
            }

        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            LoadImg = Resources.loading;
            timer1.Start();
        }
        float angle = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            angle += 5;
            if (angle >= 359) angle = 0;
            RotateImage(LoadImg, angle);
            timer1.Start();
        }

        public  void RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            float dx = image.Width / 2.0f;
            float dy = image.Height / 2.0f;

            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            //rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                g.TranslateTransform(dx, dy);
                g.RotateTransform(angle);
                g.TranslateTransform(-dx, -dy);
                g.DrawImage(image, new PointF(0, 0));
            }
            var img_old = pictureBox1.Image;
            pictureBox1.Image = rotatedBmp;
            img_old?.Dispose();
            //using (Graphics g1 = pictureBox1.CreateGraphics())
            //{
            //    g1.DrawImage(rotatedBmp, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            //}
            //rotatedBmp.Dispose();

            return;
        }
    }
    class CONSTANTDEFINE
    {
        public delegate void SetUISomeInfo();
    }
    public class LoadingHelper
    {
        #region 相关变量定义
        /// <summary>
        /// 定义委托进行窗口关闭
        /// </summary>
        private delegate void CloseDelegate();
        private static LoadingForm loadingForm;
        private static readonly Object syncLock = new Object();  //加锁使用

        #endregion

        //private LoadingHelper()
        //{

        //}

        /// <summary>
        /// 显示loading框
        /// </summary>
        public static void ShowLoadingScreen()
        {
            // Make sure it is only launched once.
            if (loadingForm != null)
                return;
            Thread thread = new Thread(new ThreadStart(LoadingHelper.ShowForm));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        private static void ShowForm()
        {
            if (loadingForm != null)
            {
                loadingForm.closeOrder();
                loadingForm = null;
            }
            loadingForm = new LoadingForm();
            loadingForm.TopMost = true;
            loadingForm.ShowDialog();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public static void CloseForm()
        {
            Thread.Sleep(50); //可能到这里线程还未起来，所以进行延时，可以确保线程起来，彻底关闭窗口
            if (loadingForm != null)
            {
                lock (syncLock)
                {
                    Thread.Sleep(50);
                    if (loadingForm != null)
                    {
                        Thread.Sleep(50);  //通过三次延时，确保可以彻底关闭窗口
                        loadingForm.Invoke(new CloseDelegate(LoadingHelper.CloseFormInternal));
                    }
                }
            }
        }

        /// <summary>
        /// 关闭窗口，委托中使用
        /// </summary>
        private static void CloseFormInternal()
        {

            loadingForm.closeOrder();
            loadingForm = null;

        }

    }
}
