using SharpShell;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ImgP
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".jpg", ".png", ".bmp", ".ico")]
    public class CountLinesExtension : SharpContextMenu
    {
        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();
            var itemCountLines = new ToolStripMenuItem
            {
                Text = "图像处理",
                Image = Resource.CountLines
            };
            var itemEnlarge = new ToolStripMenuItem
            {
                Text = "图像无损放大",
                Image = Resource.CountLines
            };
            itemEnlarge.Click += (sender, args) => ImageQualityEnhance();
            itemCountLines.DropDownItems.Add(itemEnlarge);
            var itemDehaze = new ToolStripMenuItem
            {
                Text = "图像去雾",
                Image = Resource.CountLines
            };
            itemDehaze.Click += (sender, args) => Dehaze();
            itemCountLines.DropDownItems.Add(itemDehaze);
            var itemContrastEnhance = new ToolStripMenuItem
            {
                Text = "图像对比度增强",
                Image = Resource.CountLines
            };
            itemContrastEnhance.Click += (sender, args) => ContrastEnhance();
            itemCountLines.DropDownItems.Add(itemContrastEnhance);
            var itemColourize = new ToolStripMenuItem
            {
                Text = "黑白图像上色",
                Image = Resource.CountLines
            };
            itemColourize.Click += (sender, args) => Colourize();
            itemCountLines.DropDownItems.Add(itemColourize);
            var itemStretchRestore = new ToolStripMenuItem
            {
                Text = "拉伸图像恢复",
                Image = Resource.CountLines
            };
            itemStretchRestore.Click += (sender, args) => StretchRestore();
            itemCountLines.DropDownItems.Add(itemStretchRestore);
            var itemtransformateIco = new ToolStripMenuItem
            {
                Text = "图像转ICO图标",
                Image = Resource.CountLines
            };
            itemtransformateIco.Click += (sender, args) => transformateIco();
            itemCountLines.DropDownItems.Add(itemtransformateIco);
            var itemCompressImageWithSize = new ToolStripMenuItem
            {
                Text = "图像尺寸压缩",
                Image = Resource.CountLines
            };
            itemCompressImageWithSize.Click += (sender, args) => CompressImageWithSize();
            itemCountLines.DropDownItems.Add(itemCompressImageWithSize);
            var itemCompressImageWithQuality = new ToolStripMenuItem
            {
                Text = "图像质量无损压缩",
                Image = Resource.CountLines
            };
            itemCompressImageWithQuality.Click += (sender, args) => CompressImageWithQuality();
            itemCountLines.DropDownItems.Add(itemCompressImageWithQuality);
            var itemPicGray = new ToolStripMenuItem
            {
                Text = "图像灰度化",
                Image = Resource.CountLines
            };
            itemPicGray.Click += (sender, args) => PicGray();
            itemCountLines.DropDownItems.Add(itemPicGray);
            menu.Items.Add(itemCountLines);
            return menu;
        }

        private void ImageQualityEnhance()
        {
            var builder = new StringBuilder();
            new Thread(() => {
                try
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        ImgPro.ImageQualityEnhanceDemo(filePath);
                    }
                    MessageBox.Show("图像处理完成！");
                }catch
                {
                    MessageBox.Show("网络异常！");
                }
                
            }).Start();
        }
        private void Dehaze()
        {
            var builder = new StringBuilder();
            new Thread(() => {
                try
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        ImgPro.DehazeDemo(filePath);
                    }
                    MessageBox.Show("图像处理完成！");
                }
                catch
                {
                    MessageBox.Show("网络异常！");
                }
                
            }).Start();
        }
        private void ContrastEnhance()
        {
            var builder = new StringBuilder();
            new Thread(() => {
                try
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        ImgPro.ContrastEnhanceDemo(filePath);
                    }
                    MessageBox.Show("图像处理完成！");
                }
                catch
                {
                    MessageBox.Show("网络异常！");
                }
                
            }).Start();
        }
        private void Colourize()
        {
            var builder = new StringBuilder();
            new Thread(() => {
                try
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        ImgPro.ColourizeDemo(filePath);
                    }
                    MessageBox.Show("图像处理完成！");
                }
                catch
                {
                    MessageBox.Show("网络异常！");
                }
                
            }).Start();
        }
        private void StretchRestore()
        {
            var builder = new StringBuilder();
            new Thread(() => {
                try
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        ImgPro.StretchRestoreDemo(filePath);
                    }
                    MessageBox.Show("图像处理完成！");
                }
                catch
                {
                    MessageBox.Show("网络异常！");
                }
                
            }).Start();
        }
        private void transformateIco()
        {
            var builder = new StringBuilder();
            new Thread(() => {
                try
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        ImgPro.transformateIcoDemo(filePath);
                    }
                    MessageBox.Show("图像处理完成！");
                }
                catch
                {
                    MessageBox.Show("网络异常！");
                }
                
            }).Start();
        }
        private void CompressImageWithSize()
        {
            var builder = new StringBuilder();
            new Thread(() => {
                try
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        ImgPro.CompressImageWithSizeDemo(filePath);
                    }
                    MessageBox.Show("图像处理完成！");
                }
                catch
                {
                    MessageBox.Show("网络异常！");
                }
                
            }).Start();
        }
        private void CompressImageWithQuality()
        {
            var builder = new StringBuilder();
            new Thread(() => {
                try
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        ImgPro.CompressImageWithQualityDemo(filePath);
                    }
                    MessageBox.Show("图像处理完成！");
                }
                catch
                {
                    MessageBox.Show("网络异常！");
                }
                
            }).Start();
        }
        private void PicGray()
        {
            var builder = new StringBuilder();
            new Thread(() => {
                try
                {
                    foreach (var filePath in SelectedItemPaths)
                    {
                        ImgPro.PicGrayDemo(filePath);
                    }
                    MessageBox.Show("图像处理完成！");
                }
                catch
                {
                    MessageBox.Show("网络异常！");
                }

            }).Start();
        }
    }
}
