using HtmlAgilityPack;
using MouseKeyboardLibrary;
using MyFileManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickFind.Helper;
using QuickFind.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TranslateApi;
using VideoAnalysis.Common;

namespace QuickFind
{
    public partial class SearchForm : MaterialSkin.Controls.MaterialForm
    {
        private BoLoSearch.BoLoSearch boLoSearch;
        private string previousFilterString;
        private bool lock_Form = false;
        private bool FisrtOpen = true;
        private string focus = "Recent";

        //存储窗体左边目录区的图标在ImageList（具体是ilstDirectoryIcons）中的索引
        private MouseHook mouseHook = new MouseHook();
        private KeyboardHook keyboardHook = new KeyboardHook();


        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);//设置此窗体为活动窗体


        private string FilterString
        {
            get
            {
                return InputTx.Text.Trim();
            }
        }

        public SearchForm(BoLoSearch.BoLoSearch boLoSearchX)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;///使窗体不显示在任务栏
            QuickForm.StartUpdateDB = true;
            boLoSearch = boLoSearchX;
            InputTx.Enabled = true;
            InputTx.Focus();
            tInputChecker.Start();
            statusLabel.Text = "    Ready";

            #region 电影查找
            headers.Add("Timeout", "3000");
            headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
            headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            S = Resources._2018Movie;
            #endregion
            //SetForegroundWindow(this.Handle);
            #region 注册钩子
            mouseHook.MouseDown += new MouseEventHandler((s, e) =>
            {
                AddMouseEvent("MouseDown", e.Button.ToString(), e.X, e.Y, "");
            });
            mouseHook.MouseUp += new MouseEventHandler((s, e) =>
            {
                AddMouseEvent("MouseUp", e.Button.ToString(), e.X, e.Y, "");
            });
            mouseHook.MouseWheel += new MouseEventHandler((s, e) =>
            {
                AddMouseEvent("MouseWheel", "", -1, -1, e.Delta.ToString());
            });
            mouseHook.Start();

            keyboardHook.KeyDown += new KeyEventHandler((s, e) =>
            {
                KeyboardEvent("KeyDown", e.KeyCode.ToString(), "", e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());
            });
            keyboardHook.KeyUp += new KeyEventHandler((s, e) =>
            {
                KeyboardEvent("KeyUp", e.KeyCode.ToString(), "", e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());
            });
            keyboardHook.KeyPress += new KeyPressEventHandler((s, e) =>
            {
                KeyboardEvent("KeyPress", "", e.KeyChar.ToString(), "", "", "");
            });
            keyboardHook.Start();
            #endregion
            QuickForm.STOPTranlate = true;
        }

        private void SearchForm_Activated(object sender, EventArgs e)
        {

        }

        private void AddMouseEvent(string eventType, string button, int x, int y, string delta)
        {
            if (eventType == "MouseDown" && button == "Left")
            {
                //if (SetTopWindow)
                //{
                //    SetTopWindow = false;
                //    Point p = new Point(x, y);
                //    IntPtr formHandle = WindowFromPoint(p);//得到窗口句柄
                //    SetWindowPos(formHandle, -1, 0, 0, 0, 0, 1 | 2);
                //    //this.Cursor = Cursors.Default;
                //    MessageBox.Show("以设置选中窗口置顶！");
                //}
                if (x < this.Left || x > this.Left + this.Width)
                {
                    QuickForm.STOPTranlate = false;
                    DisposeForm();
                    return;
                }
                if (y < this.Top || y > this.Top + this.Height)
                {
                    QuickForm.STOPTranlate = false;
                    DisposeForm();
                    return;
                }
            }
            if (eventType == "MouseUp" && button == "Left")
            {

            }

            if (eventType == "MouseDown" && button == "Right")
            {

            }

            if (eventType == "MouseUp" && button == "Right")
            {

            }
        }

        private void KeyboardEvent(string eventType, string keyCode, string keyChar, string shift, string alt, string control)
        {

        }
        private void DisposeForm()
        {
            QuickForm.SearchFormOpen = false;
            QuickForm.StartUpdateDB = false;
            this.Close();
            this.Dispose();
        }


        #region 文件搜索模块
        //以一定格式显示文件的大小
        //Math.Round(num,2,MidpointRounding.AwayFromZero)，中国式的四舍五入，num保留2位小数
        public static string ShowFileSize(long fileSize)
        {
            string fileSizeStr = "";

            if (fileSize < 1024)
            {
                fileSizeStr = fileSize + " 字节";
            }
            else if (fileSize >= 1024 && fileSize < 1024 * 1024)
            {
                fileSizeStr = Math.Round(fileSize * 1.0 / 1024, 2, MidpointRounding.AwayFromZero) + " KB(" + fileSize + "字节)";
            }
            else if (fileSize >= 1024 * 1024 && fileSize < 1024 * 1024 * 1024)
            {
                fileSizeStr = Math.Round(fileSize * 1.0 / (1024 * 1024), 2, MidpointRounding.AwayFromZero) + " MB(" + fileSize + "字节)";
            }
            else if (fileSize >= 1024 * 1024 * 1024)
            {
                fileSizeStr = Math.Round(fileSize * 1.0 / (1024 * 1024 * 1024), 2, MidpointRounding.AwayFromZero) + " GB(" + fileSize + "字节)";
            }

            return fileSizeStr;
        }

        private void SearchBt_Click(object sender, EventArgs e)
        {
            ShowSearchResult();
        }

        private void ShowRecentFile()
        {
            lock_Form = true;
            var Flist = RecentlyFileHelper.GetRecentlyFiles();
            //RecentFilesView
            RecentFilesView.BeginUpdate();
            RecentFilesView.Items.Clear();
            ilstIcons.Images.Clear();
            ilstIcons.Images.Add(Resources.folder);

            foreach (var dir in Flist)
            {

                if (dir == "") continue;
                if (dir.Length > 400) continue;
                if (Directory.Exists(dir))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    ListViewItem item = RecentFilesView.Items.Add(dirInfo.Name, 0);
                    item.Tag = dirInfo.FullName;
                    item.SubItems.Add(dirInfo.FullName);
                    try
                    {
                        item.SubItems.Add(dirInfo.LastWriteTime.ToString());
                    }
                    catch
                    {
                        item.SubItems.Add("----");
                    }
                    item.SubItems.Add("文件夹");
                    item.SubItems.Add("");
                }
            }
            foreach (var file in Flist)
            {

                if (file == "") continue;
                if (Directory.Exists(file)) continue;
                if (!File.Exists(file)) continue;
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Extension == ".lnk") continue;
                if (fileInfo.Extension == ".LNK") continue;
                ListViewItem item = RecentFilesView.Items.Add(fileInfo.Name);

                //为exe文件或无拓展名
                if (fileInfo.Extension == ".exe" || fileInfo.Extension == "" || fileInfo.Extension == ".xml")
                {
                    //通过当前系统获得文件相应图标
                    Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);
                    if (fileInfo.Extension == "")
                    {
                        ilstIcons.Images.Add(fileInfo.Name, Resources.fileIco);
                    }
                    else if (fileInfo.Extension == ".xml")
                    {
                        ilstIcons.Images.Add(fileInfo.Name, Resources.xml);
                    }
                    else
                    {
                        //因为不同的exe文件一般图标都不相同，所以不能按拓展名存取图标，应按文件名存取图标
                        ilstIcons.Images.Add(fileInfo.Name, fileIcon);
                    }
                    item.ImageKey = fileInfo.Name;
                }
                //其他文件
                else
                {
                    if (!ilstIcons.Images.ContainsKey(fileInfo.Extension))
                    {
                        Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);
                        if (fileIcon == null) fileIcon = Resources.fileIco;
                        //因为类型（除了exe）相同的文件，图标相同，所以可以按拓展名存取图标
                        ilstIcons.Images.Add(fileInfo.Extension, fileIcon);
                    }

                    item.ImageKey = fileInfo.Extension;
                }

                item.Tag = fileInfo.FullName;
                item.SubItems.Add(fileInfo.FullName);
                item.SubItems.Add(fileInfo.LastWriteTime.ToString());
                item.SubItems.Add(fileInfo.Extension + "文件");
                try
                {
                    item.SubItems.Add(ShowFileSize(fileInfo.Length).Split('(')[0]);
                }
                catch
                {
                    item.SubItems.Add("0KB");
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            RecentFilesView.EndUpdate();
            lock_Form = false;
        }

        private void ShowSearchResult()
        {
            lock_Form = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //开始数据更新
            AllFiles.BeginUpdate();
            AllFiles.Items.Clear();
            ilstIcons.Images.Clear();
            ilstIcons.Images.Add(Resources.folder);
            List<string[]> Flists, Dirs;
            (Dirs, Flists) = boLoSearch.getRet(FilterString);
            int fileCount = Dirs.Count() + Flists.Count();

            //列出所有文件夹
            foreach (String[] dir in Dirs)
            {
                try
                {
                    if (dir[0].Length > 30) continue;
                    if (dir[1].Length > 100) continue;
                    if (!Directory.Exists(dir[1])) continue;
                    DirectoryInfo dirInfo = new DirectoryInfo(dir[1]);
                    ListViewItem item = AllFiles.Items.Add(dirInfo.Name, 0);
                    item.Tag = dirInfo.FullName;
                    item.SubItems.Add(dirInfo.FullName);
                    try
                    {
                        item.SubItems.Add(dirInfo.LastWriteTime.ToString());
                    }
                    catch
                    {
                        item.SubItems.Add("----");
                    }

                    item.SubItems.Add("文件夹");
                    item.SubItems.Add("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            //列出所有文件
            foreach (string[] file in Flists)
            {
                try
                {
                    if (file[0].Length > 200) continue;
                    if (file[1].Length > 400) continue;
                    if (!File.Exists(file[1])) continue;
                    FileInfo fileInfo = new FileInfo(file[1]);
                    if (fileInfo.Extension == ".lnk") continue;
                    ListViewItem item = AllFiles.Items.Add(fileInfo.Name);
                    //为exe文件或无拓展名
                    if (fileInfo.Extension == ".exe" || fileInfo.Extension == "" || fileInfo.Extension == ".xml")
                    {
                        if (fileInfo.Extension == "")
                        {
                            ilstIcons.Images.Add(fileInfo.Name, Resources.fileIco);
                        }
                        else if (fileInfo.Extension == ".xml")
                        {
                            ilstIcons.Images.Add(fileInfo.Name, Resources.xml);
                        }
                        else
                        {
                            //通过当前系统获得文件相应图标
                            Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);
                            //因为不同的exe文件一般图标都不相同，所以不能按拓展名存取图标，应按文件名存取图标
                            ilstIcons.Images.Add(fileInfo.Name, fileIcon);
                        }
                        item.ImageKey = fileInfo.Name;
                    }
                    //其他文件
                    else
                    {
                        if (!ilstIcons.Images.ContainsKey(fileInfo.Extension))
                        {
                            Icon fileIcon = GetSystemIcon.GetIconByFileName(fileInfo.FullName);
                            if (fileIcon == null) fileIcon = Resources.fileIco;
                            //因为类型（除了exe）相同的文件，图标相同，所以可以按拓展名存取图标
                            ilstIcons.Images.Add(fileInfo.Extension, fileIcon);
                        }

                        item.ImageKey = fileInfo.Extension;
                    }

                    item.Tag = fileInfo.FullName;
                    item.SubItems.Add(fileInfo.FullName);

                    try
                    {
                        item.SubItems.Add(fileInfo.LastWriteTime.ToString());
                    }
                    catch
                    {
                        item.SubItems.Add("----");
                    }

                    item.SubItems.Add(fileInfo.Extension + "文件");
                    try
                    {
                        item.SubItems.Add(ShowFileSize(fileInfo.Length).Split('(')[0]);
                    }
                    catch
                    {
                        item.SubItems.Add("0KB");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            Dirs.Clear();
            Flists.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //结束数据更新
            AllFiles.EndUpdate();
            stopwatch.Stop();
            long searchTime = stopwatch.ElapsedMilliseconds;
            statusLabel.Text = string.Format("    '{0}' keyWord has {1} files found spends {2} ms ", FilterString, AllFiles.Items.Count, searchTime);
            lock_Form = false;
        }

        private int currentCol = -1;
        private bool sort = false;
        private void AllFiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            AllFiles.BeginUpdate();
            string Asc = ((char)0x25bc).ToString().PadLeft(4, ' ');
            string Des = ((char)0x25b2).ToString().PadLeft(4, ' ');
            if (sort == false)
            {
                sort = true;
                string oldStr = this.AllFiles.Columns[e.Column].Text.TrimEnd((char)0x25bc, (char)0x25b2, ' ');
                this.AllFiles.Columns[e.Column].Text = oldStr + Des;
            }
            else if (sort == true)
            {
                sort = false;
                string oldStr = this.AllFiles.Columns[e.Column].Text.TrimEnd((char)0x25bc, (char)0x25b2, ' ');
                this.AllFiles.Columns[e.Column].Text = oldStr + Asc;
            }
            switch (AllFiles.Columns[e.Column].Tag.ToString())
            {
                case "N":
                    AllFiles.ListViewItemSorter = new ListViewItemComparerNum(e.Column, sort);
                    break;
                case "D":
                    AllFiles.ListViewItemSorter = new ListViewItemComparerDate(e.Column, sort);
                    break;
                case "T":
                    AllFiles.ListViewItemSorter = new ListViewItemComparer(e.Column, sort);
                    break;
                default:
                    break;
            }
            this.AllFiles.Sort();
            int rowCount = this.AllFiles.Items.Count;
            if (currentCol != -1)
            {
                if (e.Column != currentCol)
                    this.AllFiles.Columns[currentCol].Text = this.AllFiles.Columns[currentCol].Text.TrimEnd((char)0x25bc, (char)0x25b2, ' ');
            }
            currentCol = e.Column;
            AllFiles.EndUpdate();
        }

        private void AllFiles_ItemActivate(object sender, EventArgs e)
        {
            OpenAllFiles();
        }
        private void RecentFiles_ItemActivate(object sender, EventArgs e)
        {
            OpenRecentFiles();
        }
        private void OpenAllFiles()
        {
            if (AllFiles.SelectedItems.Count > 0)
            {
                string path = AllFiles.SelectedItems[0].Tag.ToString();

                try
                {
                    Process.Start(path);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void OpenRecentFiles()
        {
            if (RecentFilesView.SelectedItems.Count > 0)
            {
                string path = RecentFilesView.SelectedItems[0].Tag.ToString();

                try
                {
                    Process.Start(path);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tInputChecker_Tick(object sender, EventArgs e)
        {
            CheckAndSearch();
        }

        private void CheckAndSearch()
        {
            tInputChecker.Stop();

            if (!string.IsNullOrWhiteSpace(this.FilterString))
            {
                if (string.Compare(this.previousFilterString, this.FilterString, true) != 0)
                {
                    if (FisrtOpen)
                    {
                        ShowRecentFile();
                        FisrtOpen = false;
                    }
                    ShowSearchResult();
                }
            }
            else
            {
                ClearResult();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            this.previousFilterString = this.FilterString;

            tInputChecker.Start();
        }

        private void ClearResult()
        {
            AllFiles.BeginUpdate();
            AllFiles.Items.Clear();
            AllFiles.EndUpdate();
            statusLabel.Text = "    Ready";
        }

        private class RecentlyFileHelper
        {
            public static string GetShortcutTargetFile(string shortcutFilename)
            {
                var type = Type.GetTypeFromProgID("WScript.Shell");
                object instance = Activator.CreateInstance(type);
                var result = type.InvokeMember("CreateShortCut", BindingFlags.InvokeMethod, null, instance, new object[] { shortcutFilename });
                var targetFile = result.GetType().InvokeMember("TargetPath", BindingFlags.GetProperty, null, result, null) as string;
                return targetFile;
            }

            public static IEnumerable<string> GetRecentlyFiles()
            {
                var recentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
                return from file in Directory.EnumerateFiles(recentFolder)
                       where Path.GetExtension(file) == ".lnk"
                       select GetShortcutTargetFile(file);

            }
        }

        private void SearchForm_Deactivate(object sender, EventArgs e)
        {
            //while (lock_Form) ;
            //this.Close();
            //this.Dispose();
        }

        private void SearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            QuickForm.SearchFormOpen = false;
            QuickForm.StartUpdateDB = false;
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            var lvwFiles = focus == "Recent" ? RecentFilesView : AllFiles;
            if (lvwFiles.Name == "AllFiles")
            {
                OpenAllFiles();
            }
            else
            {
                OpenRecentFiles();
            }
        }
        private void tsmiRename_Click(object sender, EventArgs e)
        {
            //文件重命名
            var lvwFiles = focus == "Recent" ? RecentFilesView : AllFiles;

            if (lvwFiles.SelectedItems.Count > 0)
            {
                //模拟进行编辑标签，实质是为了通过代码触发LabelEdit事件
                lvwFiles.SelectedItems[0].BeginEdit();
            }
        }

        private void 打开文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //打开文件位置
            var lvwFiles = focus == "Recent" ? RecentFilesView : AllFiles;
            if (lvwFiles.SelectedItems.Count > 0)
            {
                string path = lvwFiles.SelectedItems[0].Tag.ToString();
                path = Directory.Exists(path) ? path : new FileInfo(path).Directory.FullName;
                try
                {
                    Process.Start(path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //删除文件
        private void tsmiDelete1_Click(object sender, EventArgs e)
        {
            var lvwFiles = focus == "Recent" ? RecentFilesView : AllFiles;
            if (lvwFiles.SelectedItems.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("确定要删除吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                else
                {
                    try
                    {
                        foreach (ListViewItem item in lvwFiles.SelectedItems)
                        {
                            string path = item.Tag.ToString();

                            //如果是文件
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            //如果是文件夹
                            else if (Directory.Exists(path))
                            {
                                Directory.Delete(path, true);
                            }

                            lvwFiles.Items.Remove(item);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tsmiPrivilege1_Click(object sender, EventArgs e)
        {
            //权限管理
            var lvwFiles = focus == "Recent" ? RecentFilesView : AllFiles;
            //右边窗体中没有文件/文件夹被选中
            if (lvwFiles.SelectedItems.Count == 0)
            {
                return;
            }
            //右边窗体中有文件/文件夹被选中
            else
            {
                try
                {
                    //显示被选中的第一个文件/文件夹的权限管理界面
                    PrivilegeForm privilegeForm = new PrivilegeForm(lvwFiles.SelectedItems[0].Tag.ToString());
                    privilegeForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void tsmiProperties1_Click(object sender, EventArgs e)
        {
            var lvwFiles = focus == "Recent" ? RecentFilesView : AllFiles;
            //显示属性窗口
            //右边窗体中没有文件/文件夹被选中
            if (lvwFiles.SelectedItems.Count == 0)
            {
                return;
            }
            //右边窗体中有文件/文件夹被选中
            else
            {
                try
                {
                    //显示被选中的第一个文件/文件夹的属性
                    AttributeForm attributeForm = new AttributeForm(lvwFiles.SelectedItems[0].Tag.ToString());
                    attributeForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 复制地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lvwFiles = focus == "Recent" ? RecentFilesView : AllFiles;
            //显示属性窗口
            //右边窗体中没有文件/文件夹被选中
            if (lvwFiles.SelectedItems.Count == 0)
            {
                return;
            }
            //右边窗体中有文件/文件夹被选中
            else
            {
                try
                {
                    Clipboard.SetText(lvwFiles.SelectedItems[0].Tag.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void RecentFilesView_Enter(object sender, EventArgs e)
        {
            focus = "RecentFiles";
        }

        private void AllFiles_Enter(object sender, EventArgs e)
        {
            focus = "AllFiles";
        }
        #endregion

        #region 翻译模块

        private string TranslateText
        {
            get
            {
                return TranslateInputText.Text;
            }
        }

        private string Result = "";
        private void SEbt_Click(object sender, EventArgs e)
        {
            switch (SEbt.Text)
            {
                case "百度翻译":
                    SEbt.Text = "有道翻译";
                    new Thread(() =>
                    {
                        Result = TranslateAPI.Translate(TranslateText, "Youdao"); BeginInvoke(new Action(() => { ResultText.Text = Result; }), null);
                    }).Start();
                    break;
                case "有道翻译":
                    SEbt.Text = "必应翻译";
                    new Thread(() =>
                    {
                        Result = TranslateAPI.Translate(TranslateText, "Bing"); BeginInvoke(new Action(() => { ResultText.Text = Result; }), null);
                    }).Start();
                    break;
                case "必应翻译":
                    SEbt.Text = "谷歌翻译";
                    new Thread(() =>
                    {
                        Result = TranslateAPI.Translate(TranslateText, "Google"); BeginInvoke(new Action(() => { ResultText.Text = Result; }), null);
                    }).Start();
                    break;
                case "谷歌翻译":
                    SEbt.Text = "百度翻译";
                    new Thread(() =>
                    {
                        Result = TranslateAPI.Translate(TranslateText, "Baidu"); BeginInvoke(new Action(() => { ResultText.Text = Result; }), null);
                    }).Start();
                    break;
            }
        }

        private void TranslateInputText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                switch (SEbt.Text)
                {
                    case "百度翻译":
                        new Thread(() =>
                        {
                            Result = TranslateAPI.Translate(TranslateText, "Youdao"); BeginInvoke(new Action(() => { ResultText.Text = Result; }), null);
                        }).Start();
                        break;
                    case "有道翻译":
                        new Thread(() =>
                        {
                            Result = TranslateAPI.Translate(TranslateText, "Bing"); BeginInvoke(new Action(() => { ResultText.Text = Result; }), null);
                        }).Start();
                        break;
                    case "必应翻译":
                        new Thread(() =>
                        {
                            Result = TranslateAPI.Translate(TranslateText, "Google"); BeginInvoke(new Action(() => { ResultText.Text = Result; }), null);
                        }).Start();
                        break;
                    case "谷歌翻译":
                        new Thread(() =>
                        {
                            Result = TranslateAPI.Translate(TranslateText, "Baidu"); BeginInvoke(new Action(() => { ResultText.Text = Result; }), null);
                        }).Start();
                        break;
                }
            }
        }




        #endregion

        #region 电影搜索模块
        private List<string> MovieList = new List<string>();
        private List<string> MovieNameList = new List<string>();
        public static string url = "", name;
        private string movieName;
        private int CountMovie = 0;
        private string VideoName;
        private List<string[]> FVideoList = new List<string[]>();
        private Dictionary<string, string> headers = new Dictionary<string, string>();
        private Random ShowPicRandom = new Random();
        private string S = "";
        private bool firstshowMovie = true;
        private int TabSelect = 0;

        private void FileTableControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabSelect = e.TabPageIndex;
            if (e.TabPageIndex == 2)
            {
                TabSelect = 2;
                if (firstshowMovie)
                {
                    Thread thread_Cpic = new Thread(ChangePicShow);
                    thread_Cpic.IsBackground = true;
                    thread_Cpic.Start();
                    InitMovie();
                    Bitmap InitBitmap = Resources.Init;
                    MoviePic.Image = InitBitmap;
                    pictureBox1.Image = InitBitmap;
                    pictureBox2.Image = InitBitmap;
                    pictureBox3.Image = InitBitmap;
                    pictureBox4.Image = InitBitmap;
                    firstshowMovie = false;
                }

            }
        }

        private JObject getStringResource(string name)
        {
            Stream stream = new MemoryStream(Resources.Movie);
            using (StreamReader file = new System.IO.StreamReader(stream))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject oj = (JObject)JToken.ReadFrom(reader);
                    return oj;
                }
            }
        }

        private void ChangePicShow()
        {
            while (true)
            {
                Thread.Sleep(15000);
                if (TabSelect != 2) continue;
                if (FVideoList.Count() > 4)
                {
                    int picNum = ShowPicRandom.Next(1, FVideoList.Count() - 4);
                    if (!QuickForm.StartUpdateDB) break;
                    BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            pictureBox1.Image = Image.FromStream(WebRequest.Create(FVideoList[picNum][3] + ".jpg").GetResponse().GetResponseStream());
                            pictureBox1.Tag = picNum;
                            pictureBox2.Image = Image.FromStream(WebRequest.Create(FVideoList[picNum + 1][3] + ".jpg").GetResponse().GetResponseStream());
                            pictureBox2.Tag = picNum + 1;
                            pictureBox3.Image = Image.FromStream(WebRequest.Create(FVideoList[picNum + 2][3] + ".jpg").GetResponse().GetResponseStream());
                            pictureBox3.Tag = picNum + 2;
                            pictureBox4.Image = Image.FromStream(WebRequest.Create(FVideoList[picNum + 3][3] + ".jpg").GetResponse().GetResponseStream());
                            pictureBox4.Tag = picNum + 3;
                            stopwatch.Stop();
                            var x = stopwatch.ElapsedMilliseconds;
                        }
                        catch
                        {

                        }

                    }), null);
                }
            }
        }


        private void LoginDataBase(string MovieName)
        {
            movieName = MovieName;
            CountMovie = 0;
            textBox_Info.Text = "";
            MovieList.Clear();
            MovieNameList.Clear();
            JObject spyuan = getStringResource("Movie.json");
            foreach (var MovieFindKey in spyuan["Movie"])
            {
                if (MovieNameList.Count() > 1000)
                {
                    break;
                }
                Thread thread_SeachMovie = new Thread(new ThreadStart(delegate ()
                {
                    SMovie(MovieFindKey);
                }));
                thread_SeachMovie.Start();
            }
        }
        private void SMovie(JToken MovieFindKey)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Request Response_MoviePage = new Request(MovieFindKey["searchUrl"] + movieName, headers);
                var html = Response_MoviePage.GetHtml();
                var PageMS = html[0].SelectNodes(MovieFindKey["PageListFind"]["PageList"].ToString());
                if (PageMS == null)
                {
                    return;
                }
                foreach (var MovieUrl_A in PageMS)
                {
                    String MovieUrl = MovieFindKey["baseUrl"] + MovieUrl_A.SelectSingleNode(MovieFindKey["PageListFind"]["MovieAdress"].ToString()).Attributes[MovieFindKey["PageListFind"]["href"].ToString()].Value;
                    Request Response_MovieUrl = new Request(MovieUrl, headers);
                    var html_MovieUrl = Response_MovieUrl.GetHtml()[0];
                    string MovieName = html_MovieUrl.SelectSingleNode(MovieFindKey["MovieNmae"].ToString()).InnerText;
                    //Console.WriteLine(MovieName);
                    foreach (var f3 in html_MovieUrl.SelectSingleNode(MovieFindKey["MovieListFind"]["MovieList"].ToString()).SelectNodes(MovieFindKey["MovieListFind"]["MovieUrl"].ToString()))
                    {
                        int index = f3.InnerText.IndexOf("$");
                        if (index == -1) { continue; }
                        string MovieAddress = f3.InnerText.Substring(index + 1, f3.InnerText.Length - index - 1);
                        //Console.WriteLine(MovieFindKey["title"] + "- - - - - -" + MovieAddress);
                        //MovieInfo.Add(new string[2] { MovieName, MovieAddress });
                        MovieList.Add(MovieAddress);
                        MovieNameList.Add(MovieName);
                        BeginInvoke(new Action(() =>
                        {
                            textBox_Info.AppendText("●   编号：" + CountMovie++ + "     电影名：" + MovieName + "      地址：" + MovieAddress + "\r\n");
                            textBox_Info.Update();
                        }), null);
                        if (MovieNameList.Count() > 1000)
                        {
                            return;
                        }
                    }
                }
                stopwatch.Stop();
                var xxxxx = stopwatch.ElapsedMilliseconds;
            }
            catch
            {
                BeginInvoke(new Action(() =>
                {
                    textBox_Info.AppendText("由于网络问题，暂无资源" + "\r\n");
                    textBox_Info.Update();
                }), null);
                //Console.WriteLine("网络出错");
            }
            BeginInvoke(new Action(() =>
            {
                textBox_Info.AppendText("------" + MovieFindKey["title"] + ":资源查找结束------" + "\r\n");
            }), null);
        }

        private void VideoStartF_Click(object sender, EventArgs e)
        {
            LoginDataBase(VideoNameText.Text);
            textBox_Info.Visible = true;
        }

        private void FindVideo(String Name)
        {
            VideoName = name;
            Thread InitMovieT = new Thread(delegate ()
            {
                try
                {
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    headers.Add("Timeout", "2000");
                    headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
                    headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
                    Request Response_V = new Request("https://search.douban.com/movie/subject_search?search_text=" + VideoName, headers);

                }
                catch { }
            });
        }


        private void PlayMovieButt_Click(object sender, EventArgs e)
        {
            try
            {
                url = MovieList[Convert.ToInt32(MovieID.Text)];
            }
            catch
            {
                MessageBox.Show("填写视频序号越界，请重新填写！");
                return;
            }

            if (url.Substring(url.Length - 4, 4) == "m3u8")
            {
                Process.Start(@"https://www.m3u8play.com/?play=" + url);
            }
            else
            {
                Process.Start(url);
            }
        }

        private void MovieID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    url = MovieList[Convert.ToInt32(MovieID.Text)];
                }
                catch
                {
                    MessageBox.Show("填写视频序号越界，请重新填写！");
                    return;
                }

                if (url.Substring(url.Length - 4, 4) == "m3u8")
                {
                    Process.Start(@"https://www.m3u8play.com/?play=" + url);
                }
                else
                {
                    Process.Start(url);
                }
            }
        }

        private void 推荐资源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox_Info.Text = "";
            TuiJian();
        }


        private void VideoNameText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginDataBase(VideoNameText.Text);
            }
        }



        private void 清空目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox_Info.Text = "";
        }

        private void 刷新列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginDataBase(VideoNameText.Text);
        }
        private void 复制查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox_Info.SelectedText != "")
            {
                //Clipboard.SetDataObject(textBox_Info.SelectedText);
                VideoNameText.Text = textBox_Info.SelectedText;
                LoginDataBase(VideoNameText.Text);
            }

        }

        private void VideoNameText_TextChanged(object sender, EventArgs e)
        {
            if (VideoNameText.Text == "")
            {
                textBox_Info.Text = "";
                InitMovie();
            }
        }

        private void ShowRtB_Click(object sender, EventArgs e)
        {
            textBox_Info.Visible = !textBox_Info.Visible;
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {

            PictureBox Pic = (PictureBox)sender;
            if (Pic.Tag == null)
            {
                return;
            }
            Thread UpdatePic = new Thread(delegate ()
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Timeout", "10000");
                headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
                headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
                string Intruduce = "";
                try
                {
                    Request Response = new Request(FVideoList[(int)(Pic.Tag)][2], headers);
                    HtmlNodeCollection htmlNode = Response.GetHtml();

                    Intruduce = htmlNode[0].SelectSingleNode("//div[@id='link-report']/span[@property='v:summary']").InnerText;
                }
                catch
                {
                }


                BeginInvoke(new Action(() =>
                {
                    MovieInfo.Text = Intruduce;
                    VideoNameText.Text = FVideoList[(int)(Pic.Tag)][0];
                    ratelabel.Text = FVideoList[(int)(Pic.Tag)][1];
                    MoviePic.Image = Image.FromStream(WebRequest.Create(FVideoList[(int)(Pic.Tag)][3] + ".jpg").GetResponse().GetResponseStream());
                }), null);
            });
            UpdatePic.Start();
            LoginDataBase(FVideoList[(int)(Pic.Tag)][0]);
            textBox_Info.Visible = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            // movieChart.UpdataMovie();
        }

        private void InitMovie()
        {
            textBox_Info.AppendText("**********************资源推荐*******************\r\n");
            textBox_Info.AppendText("\r\n");
            Thread InitMovieT = new Thread(delegate ()
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Timeout", "10000");
                headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
                headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
                Request Response_New = new Request("https://movie.douban.com/j/search_subjects?type=movie&tag=%E6%9C%80%E6%96%B0&page_limit=40&page_start=0", headers);
                Request Response_Hot = new Request("https://movie.douban.com/j/search_subjects?type=movie&tag=%E7%83%AD%E9%97%A8&page_limit=400&page_start=0", headers);
                JObject json_New = Response_New.GetJson();
                JObject json_Hot = Response_Hot.GetJson();

                int Number = 0;
                try
                {
                    BeginInvoke(new Action(() =>
                    {
                        textBox_Info.AppendText("*******最新电影排行榜*******" + "\r\n");
                    }), null);
                }
                catch
                {
                    return;
                }
                foreach (var Movie in json_New["subjects"])
                {
                    Number++;
                    if (Number == 5)
                    {
                        Number = 0;
                        BeginInvoke(new Action(() =>
                        {
                            textBox_Info.AppendText(Movie["title"].ToString() + "\r\n");
                        }), null);

                    }
                    else
                    {
                        BeginInvoke(new Action(() =>
                        {
                            textBox_Info.AppendText(Movie["title"].ToString() + "      ##     ");
                        }), null);

                    }
                }
                Number = 0;
                BeginInvoke(new Action(() =>
                {
                    textBox_Info.AppendText("\r\n");
                    textBox_Info.AppendText("*******最热电影排行榜*******" + "\r\n");
                }), null);
                foreach (var Movie in json_Hot["subjects"])
                {
                    Number++;
                    if (Number == 5)
                    {
                        Number = 0;
                        BeginInvoke(new Action(() =>
                        {
                            textBox_Info.AppendText(Movie["title"].ToString() + "\r\n");
                        }), null);

                    }
                    else
                    {
                        BeginInvoke(new Action(() =>
                        {
                            textBox_Info.AppendText(Movie["title"].ToString() + "      ##     ");
                        }), null);

                    }
                }
                BeginInvoke(new Action(() =>
                {
                    textBox_Info.AppendText("\r\n");
                    textBox_Info.AppendText(S);
                    textBox_Info.Update();
                    textBox_Info.Focus();//获取焦点
                    textBox_Info.Select(0, 0);//光标定位到文本最后
                    textBox_Info.ScrollToCaret();//滚动到光标处
                }), null);


                Request Response_MostHot = new Request("https://movie.douban.com/j/search_subjects?type=movie&tag=%E7%83%AD%E9%97%A8&page_limit=400&page_start=0", headers);
                JObject json_MostHot = Response_MostHot.GetJson();
                foreach (var Movie in json_MostHot["subjects"])
                {
                    FVideoList.Add(new string[4] { Movie["title"].ToString(), Movie["rate"].ToString(), Movie["url"].ToString(), Movie["cover"].ToString().Substring(0, Movie["cover"].ToString().Length - 4) });
                }
                stopwatch.Stop();
                var xxx = stopwatch.ElapsedMilliseconds;

            });
            InitMovieT.Start();

        }


        private void TuiJian()
        {
            textBox_Info.AppendText("**********************资源推荐*******************\r\n");
            textBox_Info.AppendText("\r\n");
            Thread InitMovieT = new Thread(delegate ()
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Timeout", "10000");
                headers.Add("ContentType", "application/x-www-form-urlencoded; charset=UTF-8");
                headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
                Request Response_New = new Request("https://movie.douban.com/j/search_subjects?type=movie&tag=%E6%9C%80%E6%96%B0&page_limit=40&page_start=0", headers);
                Request Response_Hot = new Request("https://movie.douban.com/j/search_subjects?type=movie&tag=%E7%83%AD%E9%97%A8&page_limit=400&page_start=0", headers);
                JObject json_New = Response_New.GetJson();
                JObject json_Hot = Response_Hot.GetJson();

                int Number = 0;
                BeginInvoke(new Action(() =>
                {
                    textBox_Info.AppendText("*******最新电影排行榜*******" + "\r\n");
                }), null);
                foreach (var Movie in json_New["subjects"])
                {
                    Number++;
                    if (Number == 5)
                    {
                        Number = 0;
                        BeginInvoke(new Action(() =>
                        {
                            textBox_Info.AppendText(Movie["title"].ToString() + "\r\n");
                        }), null);

                    }
                    else
                    {
                        BeginInvoke(new Action(() =>
                        {
                            textBox_Info.AppendText(Movie["title"].ToString() + "      ##     ");
                        }), null);

                    }
                }
                Number = 0;
                BeginInvoke(new Action(() =>
                {
                    textBox_Info.AppendText("\r\n");
                    textBox_Info.AppendText("*******最热电影排行榜*******" + "\r\n");
                }), null);
                foreach (var Movie in json_Hot["subjects"])
                {
                    Number++;
                    if (Number == 5)
                    {
                        Number = 0;
                        BeginInvoke(new Action(() =>
                        {
                            textBox_Info.AppendText(Movie["title"].ToString() + "\r\n");
                        }), null);

                    }
                    else
                    {
                        BeginInvoke(new Action(() =>
                        {
                            textBox_Info.AppendText(Movie["title"].ToString() + "      ##     ");
                        }), null);

                    }
                }
                BeginInvoke(new Action(() =>
                {
                    textBox_Info.AppendText("\r\n");
                    textBox_Info.AppendText(S);
                    textBox_Info.Update();
                    textBox_Info.Focus();//获取焦点
                    textBox_Info.Select(0, 0);//光标定位到文本最后
                    textBox_Info.ScrollToCaret();//滚动到光标处
                }), null);

            });
            InitMovieT.Start();
        }
        #endregion

        #region 小工具模块
        #region 窗口置顶
        //获取窗口标题
        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, //窗口句柄
            StringBuilder lpString, //标题
            int nMaxCount //最大值
        );
        //获取类的名字
        [DllImport("user32.dll")]
        private static extern int GetClassName(
            IntPtr hWnd, //句柄
            StringBuilder lpString, //类名
            int nMaxCount //最大值
        );
        //根据坐标获取窗口句柄
        [DllImport("user32")]
        private static extern IntPtr WindowFromPoint(
            Point Point //坐标
        );

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);


        private bool SetTopWindow = false;

        private void BWindowsTopBt_Click(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.Cross;
            SetTopWindow = true;
            //int x = Cursor.Position.X;
            //int y = Cursor.Position.Y;
            //Point p = new Point(x, y);
            //IntPtr formHandle = WindowFromPoint(p);//得到窗口句柄
            //StringBuilder title = new StringBuilder(256);
            //GetWindowText(formHandle, title, title.Capacity);//得到窗口的标题
            //StringBuilder className = new StringBuilder(256);
            //GetClassName(formHandle, className, className.Capacity);//得到窗口的类名
        }
        #endregion
        #endregion
    }
}
