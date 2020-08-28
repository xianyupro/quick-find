using QuickFind.Properties;
using QuickFind.Update;
using System;
using System.ComponentModel;
using System.IO;
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
            if (!File.Exists(@"C:\Program Files\菠萝工具箱\UpdateDetail.html"))
            {
                byte[] readme_bytes = Resources.readme;
                //创建一个文件流
                FileStream fsreadme = new FileStream(@"C:\Program Files\菠萝工具箱\UpdateDetail.html", FileMode.Create);
                //将byte数组写入文件中
                fsreadme.Write(readme_bytes, 0, readme_bytes.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题
                fsreadme.Close();
            }
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
                CheckAUpdateBt.Text = "正在检测更新...";
                CheckAUpdateBt.Enabled = false;
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
                            CheckAUpdateBt.Enabled = true;
                            CheckAUpdateBt.Text = NewVersion ? "立即更新" : "检测新版本";
                            webBrowser1.Navigate(@"C:\Program Files\菠萝工具箱\UpdateDetail.html");
                            cheackLoading = false;
                        }), null);
                        MessageBox.Show("当前存在新版本，请及时更新。");
                    }
                    else
                    {
                        BeginInvoke(new Action(() =>
                        {
                            CheckAUpdateBt.Enabled = true;
                            CheckAUpdateBt.Text = NewVersion ? "立即更新" : "检测新版本";
                            cheackLoading = false;
                        }), null);
                        MessageBox.Show("当前已经是最新版本！");
                    }
                    
                }).Start();
                
            }
        }

    }
}
