using QuickFind.Properties;

namespace ScreenShoter
{
    partial class ToolButtons
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.picOk = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picTrans = new System.Windows.Forms.PictureBox();
            this.picLatx = new System.Windows.Forms.PictureBox();
            this.picOcrT = new System.Windows.Forms.PictureBox();
            this.picSave = new System.Windows.Forms.PictureBox();
            this.picClose = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLatx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOcrT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.SuspendLayout();
            // 
            // picOk
            // 
            this.picOk.BackColor = System.Drawing.Color.Transparent;
            this.picOk.Image = Resources.完成;
            this.picOk.Location = new System.Drawing.Point(163, 5);
            this.picOk.Name = "picOk";
            this.picOk.Size = new System.Drawing.Size(55, 20);
            this.picOk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOk.TabIndex = 3;
            this.picOk.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = Resources.分割线;
            this.pictureBox1.Location = new System.Drawing.Point(120, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(10, 21);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // picTrans
            // 
            this.picTrans.BackColor = System.Drawing.Color.Transparent;
            this.picTrans.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picTrans.Image = Resources.翻译;
            this.picTrans.Location = new System.Drawing.Point(33, 4);
            this.picTrans.Name = "picTrans";
            this.picTrans.Size = new System.Drawing.Size(24, 21);
            this.picTrans.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTrans.TabIndex = 3;
            this.picTrans.TabStop = false;
            // 
            // picLatx
            // 
            this.picLatx.BackColor = System.Drawing.Color.Transparent;
            this.picLatx.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picLatx.Image = Resources.公式;
            this.picLatx.Location = new System.Drawing.Point(5, 4);
            this.picLatx.Name = "picLatx";
            this.picLatx.Size = new System.Drawing.Size(24, 21);
            this.picLatx.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLatx.TabIndex = 3;
            this.picLatx.TabStop = false;
            // 
            // picOcrT
            // 
            this.picOcrT.BackColor = System.Drawing.Color.Transparent;
            this.picOcrT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picOcrT.Image = Resources.ocr;
            this.picOcrT.Location = new System.Drawing.Point(63, 4);
            this.picOcrT.Name = "picOcrT";
            this.picOcrT.Size = new System.Drawing.Size(24, 21);
            this.picOcrT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picOcrT.TabIndex = 3;
            this.picOcrT.TabStop = false;
            // 
            // picSave
            // 
            this.picSave.BackColor = System.Drawing.Color.Transparent;
            this.picSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSave.Image = Resources.保存;
            this.picSave.Location = new System.Drawing.Point(95, 5);
            this.picSave.Name = "picSave";
            this.picSave.Size = new System.Drawing.Size(22, 19);
            this.picSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSave.TabIndex = 3;
            this.picSave.TabStop = false;
            // 
            // picClose
            // 
            this.picClose.BackColor = System.Drawing.Color.Transparent;
            this.picClose.Image = Resources.错;
            this.picClose.Location = new System.Drawing.Point(136, 5);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(21, 19);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picClose.TabIndex = 3;
            this.picClose.TabStop = false;
            // 
            // ToolButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.picOk);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.picTrans);
            this.Controls.Add(this.picLatx);
            this.Controls.Add(this.picOcrT);
            this.Controls.Add(this.picSave);
            this.Controls.Add(this.picClose);
            this.Name = "ToolButtons";
            this.Size = new System.Drawing.Size(224, 28);
            ((System.ComponentModel.ISupportInitialize)(this.picOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTrans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLatx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOcrT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox picClose;
        private System.Windows.Forms.PictureBox picOk;
        private System.Windows.Forms.PictureBox picSave;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox picOcrT;
        private System.Windows.Forms.PictureBox picLatx;
        private System.Windows.Forms.PictureBox picTrans;
    }
}
