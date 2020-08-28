using QuickFind.Update;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace QuickFind
{
    public partial class UpdateInfoForm : MaterialSkin.Controls.MaterialForm
    {
        private bool NewVersion;
        private CheckUpdate check;
        private bool cheackLoading = false;
        public UpdateInfoForm(bool _Exist , CheckUpdate _check)
        {
            InitializeComponent();
            NewVersion = _Exist;
            check = _check;
        }
        private void UpdateInfoForm_Load(object sender, EventArgs e)
        {
            QuickForm.STOPTranlate = true;

            UpdateLabel.Visible = NewVersion;
            CheckAUpdateBt.Text = NewVersion ? "立即更新" : "检测新版本";

            webBrowser1.ScriptErrorsSuppressed = true; //禁用错误脚本提示  
            webBrowser1.IsWebBrowserContextMenuEnabled = true; // 禁用右键菜单  
            webBrowser1.WebBrowserShortcutsEnabled = true; //禁用快捷键  
            webBrowser1.AllowWebBrowserDrop = false; // 禁止文件拖动  
            webBrowser1.Navigate(@"C:\Program Files\菠萝工具箱\UpdateDetail.html");

        }
        private void UpdateInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cheackLoading)
            {
                MessageBox.Show("正在检测更新中，请勿关闭窗口！");
                e.Cancel = true;//拦截，不响应操作
            }
            QuickForm.STOPTranlate = false;
        }
        private void CheckAUpdateBt_Click(object sender, EventArgs e)
        {
            if (CheckAUpdateBt.Text == "立即更新")
            {
                check.ResumeSoftware();
                QuickForm.NewVersionExist = false;
            }
            else
            {
                new Thread(() =>
                {
                    cheackLoading = true;
                    QuickForm.NewVersionExist = check.CheckVersion();
                    NewVersion = QuickForm.NewVersionExist;
                    if (NewVersion)
                    {
                        BeginInvoke(new Action(() =>
                        {
                            UpdateLabel.Visible = NewVersion;
                            CheckAUpdateBt.Text = NewVersion ? "立即更新" : "检测新版本";
                            webBrowser1.Navigate(@"C:\Program Files\菠萝工具箱\UpdateDetail.html");
                        }), null);
                        MessageBox.Show("当前存在新版本，请及时更新。");
                    }
                    else
                    {
                        MessageBox.Show("当前已经是最新版本！");
                    }
                    cheackLoading = false;
                }).Start();
                
            }
        }

    }
}
