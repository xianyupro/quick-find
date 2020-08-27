namespace QuickFind
{
    partial class ResultForm
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
            this.SEbt = new System.Windows.Forms.Button();
            this.ResultLabel = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // SEbt
            // 
            this.SEbt.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.SEbt.Font = new System.Drawing.Font("华文中宋", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SEbt.ForeColor = System.Drawing.Color.Firebrick;
            this.SEbt.Location = new System.Drawing.Point(297, 28);
            this.SEbt.Name = "SEbt";
            this.SEbt.Size = new System.Drawing.Size(75, 31);
            this.SEbt.TabIndex = 0;
            this.SEbt.Text = "谷歌翻译";
            this.SEbt.UseVisualStyleBackColor = false;
            this.SEbt.Click += new System.EventHandler(this.SEbt_Click);
            // 
            // ResultLabel
            // 
            this.ResultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultLabel.AutoSize = true;
            this.ResultLabel.Depth = 0;
            this.ResultLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.ResultLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ResultLabel.Location = new System.Drawing.Point(10, 74);
            this.ResultLabel.MaximumSize = new System.Drawing.Size(360, 1800);
            this.ResultLabel.MinimumSize = new System.Drawing.Size(360, 0);
            this.ResultLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.ResultLabel.Name = "ResultLabel";
            this.ResultLabel.Size = new System.Drawing.Size(360, 18);
            this.ResultLabel.TabIndex = 1;
            this.ResultLabel.Text = "他说中国对南中国对南中国海他说中国对南中国海";
            this.ResultLabel.DoubleClick += new System.EventHandler(this.ResultLabel_DoubleClick);
            // 
            // ResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 110);
            this.ControlBox = false;
            this.Controls.Add(this.ResultLabel);
            this.Controls.Add(this.SEbt);
            this.MaximumSize = new System.Drawing.Size(382, 1800);
            this.MinimumSize = new System.Drawing.Size(382, 103);
            this.Name = "ResultForm";
            this.Opacity = 0.93D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ResultForm";
            this.DoubleClick += new System.EventHandler(this.ResultForm_DoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SEbt;
        private MaterialSkin.Controls.MaterialLabel ResultLabel;
    }
}