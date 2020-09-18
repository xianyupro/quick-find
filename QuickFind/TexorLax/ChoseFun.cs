using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickFind.TexorLax
{
    public partial class ChoseFun : MaterialSkin.Controls.MaterialForm
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point lpPoint);

        private Point localP = new Point();
        public ChoseFun()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Red400, Primary.Red300, Primary.Yellow200, Accent.LightBlue200, TextShade.WHITE);
            #region 设置窗体出现位置
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;
            GetCursorPos(ref localP);
            this.Left = localP.X - this.Width;
            this.Top = localP.Y - this.Height;
            #endregion
        }

        private void ChoseFun_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            QuickForm.OCR_Tex_flag = true;
            this.Close();
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            QuickForm.OCR_Lax_flag = true;
            this.Close();
        }
    }
}
