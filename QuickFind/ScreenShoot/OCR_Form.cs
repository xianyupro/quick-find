using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenShoter
{
    public partial class OCR_Form : Form
    {
        private Bitmap xResoutB;
        private string reslut;
        public OCR_Form(Bitmap bitmap ,string _reslut)
        {
            InitializeComponent();
            xResoutB = new Bitmap(bitmap);
            reslut = _reslut;
        }

        private void OCR_Form_Load(object sender, EventArgs e)
        {
            pictureBox1.Left = 12;
            pictureBox1.Top = 12;
            pictureBox1.Width = xResoutB.Width;
            pictureBox1.Height = xResoutB.Height;

            if (xResoutB.Height * 1.2 > xResoutB.Width)
            {
                textBox1.Left = pictureBox1.Left + pictureBox1.Width + 10;
                textBox1.Top = pictureBox1.Top;
                textBox1.Height = pictureBox1.Height;
                textBox1.Width = 280;
                this.Width = textBox1.Left + 312;
                this.Height = pictureBox1.Height + 58;
            }
            else
            {
                textBox1.Top = pictureBox1.Top + pictureBox1.Height + 10;
                textBox1.Left = pictureBox1.Left;
                textBox1.Width = pictureBox1.Width;
                
                Label label = new Label();
                label.Font = new Font("微软雅黑 Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                this.Controls.Add(label);
                label.AutoSize = true;
                label.Width = pictureBox1.Width;
                label.MaximumSize = new Size(pictureBox1.Width, 10000);
                label.Text = reslut;
                label.Update();
                textBox1.Height = label.Height + 6;
                label.Visible = false;
                
                this.Width = pictureBox1.Left + pictureBox1.Width + 28;
                this.Height = textBox1.Top + textBox1.Height+ 46;
            }
            this.MaximumSize = new Size(this.Width, this.Height);
            this.MinimumSize = new Size(this.Width, this.Height);
            pictureBox1.Image = xResoutB;
            textBox1.Text = reslut;
            
        }
    }
}
