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
    public partial class SearchForm : Form
    {
        private BoLoSearch.BoLoSearch boLoSearch;
        private bool lock_Form = false;

        private string FilterString
        {
            get
            {
                return InputTx.txtInput.Text.Trim();
            }
        }

        public SearchForm(BoLoSearch.BoLoSearch boLoSearchX)
        {
            InitializeComponent();
            
            QuickForm.StartUpdateDB = true;
            boLoSearch = boLoSearchX;
            InputTx.TextChanged += InputTx_TextChanged;
            statusLabel.Text = "    Ready";
            //QuickForm.STOPTranlate = true;
        }

        private void SearchForm_Activated(object sender, EventArgs e)
        {
            //InputTx.Focus();
            tInputChecker.Start();
        }

        #region 文件搜索模块

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


        List<string[]> SFlists = new List<string[]>();
        List<string[]> SDirs = new List<string[]>();
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
     
            int fileCount = SDirs.Count() + SFlists.Count();

            //列出所有文件夹
            foreach (String[] dir in SDirs)
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
            foreach (string[] file in SFlists)
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
            if (e.Column != 2) return;
            new Thread(() =>
            {
                SFlists = !sort ? SFlists.OrderByDescending(item => new FileInfo(item[1]).LastWriteTime).ToList() : SFlists.OrderBy(item => new FileInfo(item[1]).LastWriteTime).ToList();
                updatefile = true;
            }).Start();
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
        
        private void tInputChecker_Tick(object sender, EventArgs e)
        {
            CheckAndSearch();
        }


        double previousFilterTime = 0;
        private void InputTx_TextChanged(object sender, EventArgs e)
        {
            previousFilterTime = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
            if (string.IsNullOrWhiteSpace(this.FilterString))
            {
                new Thread(() =>
                {
                    SDirs.Clear();
                    SFlists.Clear();
                    var Flist = RecentlyFileHelper.GetRecentlyFiles();
                    SDirs = Flist.Where(item => Directory.Exists(item)).Select(item => new string[] { new DirectoryInfo(item).Name, item }).ToList();
                    SFlists = Flist.Where(item => File.Exists(item)).Select(item => new string[] { new FileInfo(item).Name, item }).ToList();
                    SFlists = !sort ? SFlists.OrderByDescending(item => new FileInfo(item[1]).LastWriteTime).ToList() : SFlists.OrderBy(item => new FileInfo(item[1]).LastWriteTime).ToList();
                    updatefile = true;
                }).Start();
            }    
        }

        bool updatefile = false;
        bool firstOpen = true;
        private void CheckAndSearch()
        {
            tInputChecker.Stop();

            if (firstOpen)
            {
                firstOpen = false;
                new Thread(() =>
                {
                    SDirs.Clear();
                    SFlists.Clear();
                    var Flist = RecentlyFileHelper.GetRecentlyFiles();
                    SDirs = Flist.Where(item => Directory.Exists(item)).Select(item => new string[] { new DirectoryInfo(item).Name, item }).ToList();
                    SFlists = Flist.Where(item => File.Exists(item)).Select(item => new string[] { new FileInfo(item).Name, item }).ToList();
                    SFlists = !sort ? SFlists.OrderByDescending(item => new FileInfo(item[1]).LastWriteTime).ToList() : SFlists.OrderBy(item => new FileInfo(item[1]).LastWriteTime).ToList();
                    updatefile = true;
                }).Start();
            }

            var timeNow = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
            if (!string.IsNullOrWhiteSpace(this.FilterString))
            {
                if (previousFilterTime != 0 && timeNow > previousFilterTime + 0.1)
                {
                    new Thread(() =>
                    {
                        SDirs.Clear();
                        SFlists.Clear();
                        (SDirs, SFlists) = boLoSearch.getRet(FilterString);
                        SFlists = !sort ? SFlists.OrderByDescending(item => new FileInfo(item[1]).LastWriteTime).ToList() : SFlists.OrderBy(item => new FileInfo(item[1]).LastWriteTime).ToList();
                        updatefile = true;
                    }).Start();
                    previousFilterTime = 0;
                }
            }
            if (updatefile)
            {
                ShowSearchResult();
                updatefile = false;
            }
            tInputChecker.Start();
        }

        private void SearchForm_Deactivate(object sender, EventArgs e)
        {
            
        }

        private void SearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            QuickForm.SearchFormOpen = false;
            QuickForm.StartUpdateDB = false;
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            var lvwFiles =  AllFiles;
            OpenAllFiles();

        }
        private void tsmiRename_Click(object sender, EventArgs e)
        {
            //文件重命名
            var lvwFiles =  AllFiles;

            if (lvwFiles.SelectedItems.Count > 0)
            {
                //模拟进行编辑标签，实质是为了通过代码触发LabelEdit事件
                lvwFiles.SelectedItems[0].BeginEdit();
            }
        }

        private void 打开文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //打开文件位置
            var lvwFiles =  AllFiles;
            if (lvwFiles.SelectedItems.Count > 0)
            {
                string path = lvwFiles.SelectedItems[0].Tag.ToString();
                try
                {
                    Process.Start("Explorer.exe", @"/select,"+ path);
                    this.Close();
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
            var lvwFiles =  AllFiles;
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
            var lvwFiles =  AllFiles;
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
            var lvwFiles = AllFiles;
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
            var lvwFiles = AllFiles;
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
        #endregion
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //打开文件位置
            var lvwFiles = AllFiles;
            if (lvwFiles.SelectedItems.Count > 0)
            {
                string path = lvwFiles.SelectedItems[0].Tag.ToString();
                //path = Directory.Exists(path) ? path : new FileInfo(path).Directory.FullName;
                try
                {
                    System.Collections.Specialized.StringCollection strcoll = new System.Collections.Specialized.StringCollection();
                    strcoll.Add(path);
                    Clipboard.SetFileDropList(strcoll);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        #region 拖动主界面事件的编写
        Point mouseOff;
        bool leftFlag = false;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);
                leftFlag = true;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);
                this.Location = mouseSet;
            }
        }


        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;
            }
        }
        #endregion

        private void ClosePB_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
