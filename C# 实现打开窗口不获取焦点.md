# Windows窗体之ShowWindow函数分析

# 一、窗体创建过程

  在文章《Windows消息机制的逆向分析》中详细分析了Windows的消息机制以及窗体的创建过程，文章中表述Windows窗体的创建分为三步：声明WNDCLASS实例、窗体注册、创建窗体。如果继续细分可以分为四步：**声明WNDCLASS实例、窗体注册、创建窗体、显示窗体**。

  其中窗体注册函数是RegisterClass()或RegisterClassEx()，创建窗体则是CreateWindow()或CreatewindowEx()函数，显示窗体则是利用ShowWindow()函数。

  本文重点描述ShowWindow()函数。

# 二、ShowWindow函数

  CreateWindowEx 建立窗口以后，这时候，窗口虽已建立，但还没有在屏幕上显示出来。

  **ShowWindow函数**

  **函数功能：**该函数设置指定窗口的显示状态。

  **函数原型：BOOL \*ShowWindow\*（HWND \*hWnd\*, int \*nCmdShow\*）**

  其中***hWnd***指窗口句柄；***nCmdShow***指定窗口如何显示。如果发送应用程序的程序提供了STARTUPINFO结构，则应用程序第一次调用ShowWindow时该参数被忽略。否则，在第一次调用ShowWindow函数时，该值应为在函数WinMain中nCmdShow参数。在随后的调用中，该参数可以为下列值之一：

| **预定义值**       | **等 效 值**                                                 | **nCmdShow值** |
| :----------------- | :----------------------------------------------------------- | -------------- |
| SW_FORCEMINIMIZE   | 在WindowNT5.0中最小化窗口，即使拥有窗口的线程被挂起也会最小化。在从其他线程最小化窗口时才使用这个参数 | 11             |
| SW_HIDE            | 隐藏窗口，大小不变，激活状态不变                             | 0              |
| SW_MAXIMIZE        | 最大化窗口，显示状态不变，激活状态不变                       | 3              |
| SW_MINIMIZE        | 最小化指定的窗口并且激活在Z序中的下一个顶层窗口              | 6              |
| SW_RESTORE         | 激活并显示窗口。如果窗口最小化或最大化，则系统将窗口恢复到原来的尺寸和位置。在恢复最小化窗口时，应用程序应该指定这个标志 | 9              |
| SW_SHOW            | 在窗口原来的位置以原来的尺寸激活和显示窗口                   | 5              |
| SW_SHOWMAXIMIZED   | 显示并激活窗口，以最大化显示                                 | 3              |
| SW_SHOWMINIMIZED   | 显示并激活窗口，以最小化显示                                 | 2              |
| SW_SHOWMINNOACTIVE | 显示窗口并最小化，激活窗口仍然维持激活状态                   | 7              |
| SW_SHOWNA          | 以窗口原来的状态显示窗口。激活窗口仍然维持激活状态           | 8              |
| SW_SHOWNOACTIVATE  | 以窗口最近一次的大小和状态显示窗口。激活窗口仍然维持激活状态 | 4              |
| SW_SHOWDEFAULT     | 依据在STARTUPINFO结构中指定的SW_FLAG标志设定显示状态，STARTUPINFO 结构是由启动应用程序的程序传递给CreateProcess函数的 | 10             |
| SW_SHOWNORMAL      | 激活并显示一个窗口。如果窗口被最小化或最大化，系统将其恢复到原来的尺寸和大小。应用程序在第一次显示窗口的时候应该指定此标志 | 1              |

# 三、函数过程分析

  本次分析样本仍以https://blog.csdn.net/weixin_39768541/article/details/85010183 文章中的CM。在RegisterClass、CreateWindow以及NtUserShowWindow（模块间调用无ShowWindow函数）处下断点。

![img](https://img-blog.csdnimg.cn/20181231232317882.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zOTc2ODU0MQ==,size_16,color_FFFFFF,t_70)

  可以观察到函数执行流程是**先将所有的窗体注册与创建完成后，再逐一执行NtUserShowWindow函数**。

![img](https://img-blog.csdnimg.cn/20181231232529754.PNG)

  那么现在来搜索ShowWindow函数何处调用。利用RUN跟踪：

![img](https://img-blog.csdnimg.cn/2018123123305940.PNG?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3dlaXhpbl8zOTc2ODU0MQ==,size_16,color_FFFFFF,t_70)

  调用函数即为NtUserShowWindow外层函数，其中标红位置即为nCmdShow值。

# c# 使用api函数 ShowWindowAsync 控制窗体

1.需要匯入 [System.Runtime.InteropServices](http://msdn.microsoft.com/zh-tw/library/system.runtime.interopservices.aspx) 命名空間

2.宣告 ShowWindowAsync 函數

​    [DllImport("user32.dll")]

​    private static extern bool ShowWindowAsync(

​      IntPtr hWnd,

​      int nCmdShow

​    );

3.宣告 ShowWindow函數

​    [DllImport("user32.dll")]

​    public static extern int ShowWindow(

​      int hwnd,

​      int nCmdShow

​    );

4.宣告API常數定義

​    //API 常數定義

​    private const int SW_HIDE = 0;

​    private const int SW_NORMAL = 1;

​    private const int SW_MAXIMIZE = 3;

​    private const int SW_SHOWNOACTIVATE = 4;

​    private const int SW_SHOW = 5;

​    private const int SW_MINIMIZE = 6;

​    private const int SW_RESTORE = 9;

​    private const int SW_SHOWDEFAULT = 10;

5.上述函數功能相同，都是用來設定視窗大小，不同的是宣告的型態不一樣需轉型。

ShowWindowAsync(this.Handle, SW_MINIMIZE);

ShowWindow((int)this.Handle, SW_MINIMIZE);

 

6.若是把***\**int\*\**** 改成IntPtr ，使用ShowWindow就不用轉型，所以在宣告時就可以考慮資料型態，必免轉型所耗的資源。

​    [DllImport("user32.dll")]

​    public static extern int ShowWindow(

​      ***\**int\*\**** hwnd,

​      int nCmdShow

​    );

 

**C#完整範例**

```
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace CS_WindowsResize
{
  public partial class Form1 : Form
  {
    public Form1()
   {
      InitializeComponent();
   }
    //API 常數定義
   private const int SW_HIDE = 0;
   private const int SW_NORMAL = 1;
   private const int SW_MAXIMIZE = 3;
   private const int SW_SHOWNOACTIVATE = 4;
   private const int SW_SHOW = 5;
   private const int SW_MINIMIZE = 6;
   private const int SW_RESTORE = 9;
   private const int SW_SHOWDEFAULT = 10;
   
   [DllImport("user32.dll")]
   private static extern bool ShowWindowAsync(
   IntPtr hWnd,
   int nCmdShow
   );

 

​    [DllImport("user32.dll")]

​    public static extern int ShowWindow(

​      int hwnd,

​      int nCmdShow

​    );

​    private void button1_Click(object sender, EventArgs e)

​    {

​      //最小化

​      ShowWindowAsync(this.Handle, SW_MINIMIZE);

​    }

​    private void button2_Click(object sender, EventArgs e)

​    {

​      //最大化

​      ShowWindowAsync(this.Handle, SW_MAXIMIZE);

​    }

 

​    private void button3_Click(object sender, EventArgs e)

​    {

​      //還原

​      ShowWindowAsync(this.Handle, SW_RESTORE);

​    }

 

​    private void button4_Click(object sender, EventArgs e)

​    {

​      //最小化

​      ShowWindow((int)this.Handle, SW_MINIMIZE);

​    }

 

​    private void button5_Click(object sender, EventArgs e)

​    {

​      //最大化

​      ShowWindow((int)this.Handle, SW_MAXIMIZE);

​    }

 

​    private void button6_Click(object sender, EventArgs e)

​    {

​      //還原

​      ShowWindow((int)this.Handle, SW_RESTORE);

​    }

  }

}


```

 

# C# 实现打开窗口不获取焦点

![img](https://csdnimg.cn/release/phoenix/template/new_img/reprint.png)

[weixin_34061555](https://me.csdn.net/weixin_34061555) 2016-12-12 14:48:00 ![img](https://csdnimg.cn/release/phoenix/template/new_img/articleReadEyes.png) 746 ![img](https://csdnimg.cn/release/phoenix/template/new_img/tobarCollect.png) 收藏

版权

最近，在做一个tooltip窗口，鼠标移到某个控件上去，这个tooltip窗口就打开并显示一些信息（有图片和文字）。

发现如果先tooltipWindow.show()然后再this.focus()，主窗体会有闪烁。

网上搜了一下，发现有前辈说：

1. *创建窗口时去掉WS_VISIBLE属性，加上WS_DISABLED属性。*
2. *创建窗口，得到窗口句柄。*
3. *::ShowWindow(m_hWnd,SW_SHOWNOACTIVATE)显示窗口，则不会抢夺焦点。*
4. *更进一步，可以屏蔽鼠标点击时获得焦点的行为，响应WM_MOUSEACTIVATE，返回MA_NOACTIVATE。*

但是也没有给代码。所以我自己试了试：

发现不需要用到第一步。也不知道是为什么。(我的tooltip不需要第4步)

把tooltipWindow.Show()换成Win32.ShowWindow(tooltipWindow.Handle,SW_SHOWNOACTIVATE)就好了，也不用再使用this.Focus()。

注：ShowWindow的实现和SW_SHOWNOACTIVATE的值自己搜索一下就好了

 

参考资料：http://www.cnblogs.com/cartler/p/4537719.html

 

----------------------------------咯咯咯-------------------------------

后来发现主窗体最小化后再还原，鼠标再移上去的时候就不显示tooltip窗口了，应该是z轴顺序的问题，但是我设了tooltip窗口的TopMost=true之后，主窗口还是会闪烁。

我想要不要换个方式改变tooltip窗口的z轴顺序，就找到了Winapi里的SetWindowPos方法，试了一下，行了！

Win32.ShowWindow(tooltip.Handle,4);//4=SW_SHOWNOACTIVATE
Win32.SetWindowPos(tooltip.Handle, -1, MousePosition.X + 2, MousePosition.Y + 2, 0, 0, 1 | 0x10);

参考资料：

https://yq.aliyun.com/articles/53789

http://www.jb51.net/article/32718.htm

转载于:https://my.oschina.net/duoing/blog/804492