using HZH_Controls.Controls;
using System;

namespace QuickFind
{
    partial class SearchForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
            this.ilstIcons = new System.Windows.Forms.ImageList(this.components);
            this.tInputChecker = new System.Windows.Forms.Timer(this.components);
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tssLine3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiDelete1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.tssLine4 = new System.Windows.Forms.ToolStripSeparator();
            this.复制地址ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tssLine5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPrivilege1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProperties1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.AllFiles = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ucSplitLabel1 = new HZH_Controls.Controls.UCSplitLabel();
            this.InputTx = new HZH_Controls.Controls.UCTextBoxEx();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ClosePB = new System.Windows.Forms.PictureBox();
            this.statusBar.SuspendLayout();
            this.cmsMain.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePB)).BeginInit();
            this.SuspendLayout();
            // 
            // ilstIcons
            // 
            this.ilstIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilstIcons.ImageStream")));
            this.ilstIcons.TransparentColor = System.Drawing.SystemColors.Control;
            this.ilstIcons.Images.SetKeyName(0, "disk.png");
            this.ilstIcons.Images.SetKeyName(1, "CD_ROM.png");
            this.ilstIcons.Images.SetKeyName(2, "u_pan.png");
            this.ilstIcons.Images.SetKeyName(3, "folder.png");
            this.ilstIcons.Images.SetKeyName(4, "recent.png");
            // 
            // tInputChecker
            // 
            this.tInputChecker.Interval = 10;
            this.tInputChecker.Tick += new System.EventHandler(this.tInputChecker_Tick);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 618);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(790, 22);
            this.statusBar.TabIndex = 8;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(131, 17);
            this.statusLabel.Text = "toolStripStatusLabel1";
            // 
            // cmsMain
            // 
            this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpen,
            this.复制ToolStripMenuItem,
            this.打开文件夹ToolStripMenuItem,
            this.tssLine3,
            this.tsmiDelete1,
            this.tsmiRename,
            this.tssLine4,
            this.复制地址ToolStripMenuItem,
            this.tssLine5,
            this.tsmiPrivilege1,
            this.tsmiProperties1,
            this.toolStripMenuItem1});
            this.cmsMain.Name = "contextMenuStrip1";
            this.cmsMain.Size = new System.Drawing.Size(137, 204);
            // 
            // tsmiOpen
            // 
            this.tsmiOpen.Name = "tsmiOpen";
            this.tsmiOpen.Size = new System.Drawing.Size(136, 22);
            this.tsmiOpen.Text = "打开";
            this.tsmiOpen.Click += new System.EventHandler(this.tsmiOpen_Click);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 打开文件夹ToolStripMenuItem
            // 
            this.打开文件夹ToolStripMenuItem.Name = "打开文件夹ToolStripMenuItem";
            this.打开文件夹ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.打开文件夹ToolStripMenuItem.Text = "打开文件夹";
            this.打开文件夹ToolStripMenuItem.Click += new System.EventHandler(this.打开文件夹ToolStripMenuItem_Click);
            // 
            // tssLine3
            // 
            this.tssLine3.Name = "tssLine3";
            this.tssLine3.Size = new System.Drawing.Size(133, 6);
            // 
            // tsmiDelete1
            // 
            this.tsmiDelete1.Name = "tsmiDelete1";
            this.tsmiDelete1.Size = new System.Drawing.Size(136, 22);
            this.tsmiDelete1.Text = "删除";
            this.tsmiDelete1.Click += new System.EventHandler(this.tsmiDelete1_Click);
            // 
            // tsmiRename
            // 
            this.tsmiRename.Name = "tsmiRename";
            this.tsmiRename.Size = new System.Drawing.Size(136, 22);
            this.tsmiRename.Text = "重命名";
            this.tsmiRename.Click += new System.EventHandler(this.tsmiRename_Click);
            // 
            // tssLine4
            // 
            this.tssLine4.Name = "tssLine4";
            this.tssLine4.Size = new System.Drawing.Size(133, 6);
            // 
            // 复制地址ToolStripMenuItem
            // 
            this.复制地址ToolStripMenuItem.Name = "复制地址ToolStripMenuItem";
            this.复制地址ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.复制地址ToolStripMenuItem.Text = "复制地址";
            this.复制地址ToolStripMenuItem.Click += new System.EventHandler(this.复制地址ToolStripMenuItem_Click);
            // 
            // tssLine5
            // 
            this.tssLine5.Name = "tssLine5";
            this.tssLine5.Size = new System.Drawing.Size(133, 6);
            // 
            // tsmiPrivilege1
            // 
            this.tsmiPrivilege1.Name = "tsmiPrivilege1";
            this.tsmiPrivilege1.Size = new System.Drawing.Size(136, 22);
            this.tsmiPrivilege1.Text = "权限管理";
            this.tsmiPrivilege1.Click += new System.EventHandler(this.tsmiPrivilege1_Click);
            // 
            // tsmiProperties1
            // 
            this.tsmiProperties1.Name = "tsmiProperties1";
            this.tsmiProperties1.Size = new System.Drawing.Size(136, 22);
            this.tsmiProperties1.Text = "属性";
            this.tsmiProperties1.Click += new System.EventHandler(this.tsmiProperties1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(133, 6);
            // 
            // AllFiles
            // 
            this.AllFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15});
            this.AllFiles.ContextMenuStrip = this.cmsMain;
            this.AllFiles.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AllFiles.FullRowSelect = true;
            this.AllFiles.HideSelection = false;
            this.AllFiles.LabelEdit = true;
            this.AllFiles.Location = new System.Drawing.Point(0, 94);
            this.AllFiles.Name = "AllFiles";
            this.AllFiles.Size = new System.Drawing.Size(790, 521);
            this.AllFiles.SmallImageList = this.ilstIcons;
            this.AllFiles.TabIndex = 0;
            this.AllFiles.UseCompatibleStateImageBehavior = false;
            this.AllFiles.View = System.Windows.Forms.View.Details;
            this.AllFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.AllFiles_ColumnClick);
            this.AllFiles.ItemActivate += new System.EventHandler(this.AllFiles_ItemActivate);
            // 
            // columnHeader11
            // 
            this.columnHeader11.Tag = "T";
            this.columnHeader11.Text = "名称";
            this.columnHeader11.Width = 180;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Tag = "T";
            this.columnHeader12.Text = "位置";
            this.columnHeader12.Width = 280;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Tag = "D";
            this.columnHeader13.Text = "修改日期";
            this.columnHeader13.Width = 120;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Tag = "T";
            this.columnHeader14.Text = "类型";
            this.columnHeader14.Width = 100;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Tag = "T";
            this.columnHeader15.Text = "大小";
            this.columnHeader15.Width = 80;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.panel1.Location = new System.Drawing.Point(0, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(90, 46);
            this.panel1.TabIndex = 14;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(26, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // ucSplitLabel1
            // 
            this.ucSplitLabel1.AutoSize = true;
            this.ucSplitLabel1.Font = new System.Drawing.Font("华文中宋", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucSplitLabel1.ForeColor = System.Drawing.Color.Red;
            this.ucSplitLabel1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(238)))), ((int)(((byte)(245)))));
            this.ucSplitLabel1.Location = new System.Drawing.Point(323, 10);
            this.ucSplitLabel1.MaximumSize = new System.Drawing.Size(0, 30);
            this.ucSplitLabel1.MinimumSize = new System.Drawing.Size(150, 30);
            this.ucSplitLabel1.Name = "ucSplitLabel1";
            this.ucSplitLabel1.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.ucSplitLabel1.Size = new System.Drawing.Size(150, 30);
            this.ucSplitLabel1.TabIndex = 12;
            this.ucSplitLabel1.Text = "文件查找器";
            // 
            // InputTx
            // 
            this.InputTx.BackColor = System.Drawing.Color.Transparent;
            this.InputTx.ConerRadius = 5;
            this.InputTx.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.InputTx.DecLength = 2;
            this.InputTx.FillColor = System.Drawing.Color.Empty;
            this.InputTx.FocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.InputTx.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.InputTx.InputText = "";
            this.InputTx.InputType = HZH_Controls.TextInputType.NotControl;
            this.InputTx.IsFocusColor = true;
            this.InputTx.IsRadius = true;
            this.InputTx.IsShowClearBtn = true;
            this.InputTx.IsShowKeyboard = false;
            this.InputTx.IsShowRect = true;
            this.InputTx.IsShowSearchBtn = false;
            this.InputTx.KeyBoardType = HZH_Controls.Controls.KeyBoardType.UCKeyBorderAll_EN;
            this.InputTx.Location = new System.Drawing.Point(58, 48);
            this.InputTx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InputTx.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.InputTx.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.InputTx.Name = "InputTx";
            this.InputTx.Padding = new System.Windows.Forms.Padding(5);
            this.InputTx.PasswordChar = '\0';
            this.InputTx.PromptColor = System.Drawing.Color.Gray;
            this.InputTx.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.InputTx.PromptText = "";
            this.InputTx.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.InputTx.RectWidth = 1;
            this.InputTx.RegexPattern = "";
            this.InputTx.Size = new System.Drawing.Size(678, 42);
            this.InputTx.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(255)))), ((int)(((byte)(152)))));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel2.Controls.Add(this.ClosePB);
            this.panel2.Controls.Add(this.ucSplitLabel1);
            this.panel2.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.panel2.Location = new System.Drawing.Point(0, -1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(790, 46);
            this.panel2.TabIndex = 14;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // ClosePB
            // 
            this.ClosePB.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ClosePB.Image = global::QuickFind.Properties.Resources.关闭;
            this.ClosePB.Location = new System.Drawing.Point(749, 7);
            this.ClosePB.Name = "ClosePB";
            this.ClosePB.Size = new System.Drawing.Size(30, 29);
            this.ClosePB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ClosePB.TabIndex = 15;
            this.ClosePB.TabStop = false;
            this.ClosePB.Click += new System.EventHandler(this.ClosePB_Click);
            // 
            // SearchForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(790, 640);
            this.ControlBox = false;
            this.Controls.Add(this.InputTx);
            this.Controls.Add(this.AllFiles);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.SearchForm_Activated);
            this.Deactivate += new System.EventHandler(this.SearchForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SearchForm_FormClosed);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.cmsMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClosePB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList ilstIcons;
        private System.Windows.Forms.Timer tInputChecker;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ListView AllFiles;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ContextMenuStrip cmsMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpen;
        private System.Windows.Forms.ToolStripMenuItem 打开文件夹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator tssLine3;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete1;
        private System.Windows.Forms.ToolStripMenuItem tsmiRename;
        private System.Windows.Forms.ToolStripSeparator tssLine4;
        private System.Windows.Forms.ToolStripSeparator tssLine5;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrivilege1;
        private System.Windows.Forms.ToolStripMenuItem tsmiProperties1;
        private System.Windows.Forms.ToolStripMenuItem 复制地址ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private UCTextBoxEx InputTx;
        private UCSplitLabel ucSplitLabel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox ClosePB;
    }
}