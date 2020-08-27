using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace QuickFind
{
    public partial class UpdateInfoForm : MaterialSkin.Controls.MaterialForm
    {
        public UpdateInfoForm()
        {
            InitializeComponent();
        }
        private void UpdateInfoForm_Load(object sender, EventArgs e)
        {
            QuickForm.STOPTranlate = true;
            //VersionClass.SetIE(VersionClass.IeVersion.强制ie9);
            webBrowser1.ScriptErrorsSuppressed = true; //禁用错误脚本提示  
            webBrowser1.IsWebBrowserContextMenuEnabled = true; // 禁用右键菜单  
            webBrowser1.WebBrowserShortcutsEnabled = true; //禁用快捷键  
            webBrowser1.AllowWebBrowserDrop = false; // 禁止文件拖动  
            webBrowser1.Navigate(@"C:\Program Files\菠萝工具箱\UpdateDetail.html");
            //webBrowser1.Navigated += WebBrowserNavigatedEventHandler;
            //webBrowser1.DocumentCompleted += WebBrowserDocumentCompletedEventHandler;
            //webBrowser1.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);
            //webBrowser1.NewWindow += CancelEventHandler;
        }
        //禁用新窗口打开  
        public void CancelEventHandler(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            e.Handled = true;
        }

        //后发生  
        public void WebBrowserDocumentCompletedEventHandler(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
        //先发生  
        public void WebBrowserNavigatedEventHandler(object sender, WebBrowserNavigatedEventArgs e)
        {

        }

        private void UpdateInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            QuickForm.STOPTranlate = false;
        }
    }
}
