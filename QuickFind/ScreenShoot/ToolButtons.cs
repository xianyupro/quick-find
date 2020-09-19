﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ScreenShoter
{
    public partial class ToolButtons : UserControl
    {
        public ToolButtons()
        {
            InitializeComponent();
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);

            this.Cursor = Cursors.Default;
            picOk.Cursor = Cursors.Hand;
            picClose.Cursor = Cursors.Hand;

            this.picOk.Click += (s, e) => 
            {
                SaveImg();
            };
            this.picLatx.Click += (s, e) =>
            {
                OcrLatx();
            };
            this.picClose.Click += (s, e) =>
            {
                (this.ParentForm as FormShot).ShowParent();
            };
            this.picOcrT.Click += (s, e) =>
            {
                OcrT();
            };
            this.picTrans.Click += (s, e) =>
            {
                Trans();
            };
            this.picSave.Click += (s, e) =>
            {
                var sfd = new SaveFileDialog();
                sfd.FileName = $"screenshot_{DateTime.Now.ToString("yyyyMMddHHmmssff")}.png";
                sfd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
                var dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    SaveImg(Path.Combine(sfd.InitialDirectory, sfd.FileName));
                }
            };
        }

        private void SaveImg(string fileName = "")
        {
            (this.ParentForm as FormShot).SaveToClipboard(fileName);
            (this.ParentForm as FormShot).ShowParent();
        }
        private void OcrT()
        {
            (this.ParentForm as FormShot).OCR_Text();
            (this.ParentForm as FormShot).ShowParent();
        }
        private void Trans()
        {
            (this.ParentForm as FormShot).OCR_Trans();
            (this.ParentForm as FormShot).ShowParent();
        }
        private void OcrLatx()
        { 
            (this.ParentForm as FormShot).OCR_Latx();
            (this.ParentForm as FormShot).ShowParent();
        }
    }
}
