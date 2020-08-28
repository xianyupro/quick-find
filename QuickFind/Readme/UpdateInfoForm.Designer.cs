namespace QuickFind
{
    partial class UpdateInfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.CheckAUpdateBt = new System.Windows.Forms.Button();
            this.UpdateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(3, 67);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(776, 521);
            this.webBrowser1.TabIndex = 0;
            // 
            // CheckAUpdateBt
            // 
            this.CheckAUpdateBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckAUpdateBt.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.CheckAUpdateBt.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CheckAUpdateBt.ForeColor = System.Drawing.Color.DarkGreen;
            this.CheckAUpdateBt.Location = new System.Drawing.Point(677, 28);
            this.CheckAUpdateBt.Name = "CheckAUpdateBt";
            this.CheckAUpdateBt.Size = new System.Drawing.Size(102, 30);
            this.CheckAUpdateBt.TabIndex = 1;
            this.CheckAUpdateBt.Text = "检查更新";
            this.CheckAUpdateBt.UseVisualStyleBackColor = false;
            this.CheckAUpdateBt.Click += new System.EventHandler(this.CheckAUpdateBt_Click);
            // 
            // UpdateLabel
            // 
            this.UpdateLabel.AutoSize = true;
            this.UpdateLabel.BackColor = System.Drawing.Color.Transparent;
            this.UpdateLabel.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UpdateLabel.ForeColor = System.Drawing.Color.SpringGreen;
            this.UpdateLabel.Location = new System.Drawing.Point(187, 31);
            this.UpdateLabel.Name = "UpdateLabel";
            this.UpdateLabel.Size = new System.Drawing.Size(373, 25);
            this.UpdateLabel.TabIndex = 2;
            this.UpdateLabel.Text = "当前软件存在新版本，点击右侧可进行更新";
            // 
            // UpdateInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 593);
            this.Controls.Add(this.UpdateLabel);
            this.Controls.Add(this.CheckAUpdateBt);
            this.Controls.Add(this.webBrowser1);
            this.MaximumSize = new System.Drawing.Size(788, 593);
            this.MinimumSize = new System.Drawing.Size(788, 593);
            this.Name = "UpdateInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "更新日志";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdateInfoForm_FormClosing);
            this.Load += new System.EventHandler(this.UpdateInfoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button CheckAUpdateBt;
        private System.Windows.Forms.Label UpdateLabel;
    }
}