using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOCOM.IVX.Controls
{
    /*
     1、在解决方案中添加新类AniWindow，复制以下代码；
2、在需要使用特效的窗体OnLoad事件中使用AniWindow的构造函数New一个AniWindow对象，不同的参数显示不同的效果；
如：AniWindow a=new AniWindow(this.Handle,5,1,this);//以透明渐变显示窗口
3、参数说明：
AniWindow(窗口句柄,动画样式,打开或关闭标志,实例表单);

△窗口句柄: this.Handle 
△画样式: 0 -> 普通显示
   1 -> 从左向右显示
   2 -> 从右向左显示
   3 -> 从上到下显示
   4 -> 从下到上显示
   5 -> 透明渐变显示
   6 -> 从中间向四周
   7 -> 左上角伸展
   8 -> 左下角伸展
   9 -> 右上角伸展
   10 -> 右下角伸展
△开关标志: 0为关闭窗口 1为打开窗口
△ 实例表单: 为了去除Label的BUG, 取值 this

     
     */
    public class AniWindow
    {

        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        private const int AW_HOR_POSITIVE = 0x0001;
        private const int AW_HOR_NEGATIVE = 0x0002;
        private const int AW_VER_POSITIVE = 0x0004;
        private const int AW_VER_NEGATIVE = 0x0008;
        private const int AW_CENTER = 0x0010;
        private const int AW_HIDE = 0x10000;
        private const int AW_ACTIVATE = 0x20000;
        private const int AW_SLIDE = 0x40000;
        private const int AW_BLEND = 0x80000;
        private int CloseOpen = 0x20000;

        public AniWindow(IntPtr hwnd, int dwFlags, int CloseOrOpen, System.Windows.Forms.Control myform)
        {
            try
            {
                if (CloseOrOpen == 1)
                {
                    foreach (System.Windows.Forms.Control mycontrol in myform.Controls)
                    {
                        string m = mycontrol.GetType().ToString();
                        m = m.Substring(m.Length - 5);
                        if (m == "Label")
                        {
                            mycontrol.Visible = false;     //这里是在动画效果之前,把表单上可视的LABEL设为不可视
                        }
                    }
                }

                //打开or关闭 0是关闭 1是打开
                if (CloseOrOpen == 0) { CloseOpen = 0x10000; }

                if (dwFlags == 100)
                {
                    int zz = 10;
                    Random a = new Random();
                    dwFlags = (int)a.Next(zz);
                }

                switch (dwFlags)
                {
                    case 0://普通显示
                        AnimateWindow(hwnd, 200, AW_ACTIVATE);
                        break;
                    case 1://从左向右显示
                        AnimateWindow(hwnd, 200, AW_HOR_POSITIVE | CloseOpen);
                        break;
                    case 2://从右向左显示
                        AnimateWindow(hwnd, 200, AW_HOR_NEGATIVE | CloseOpen);
                        break;
                    case 3://从上到下显示
                        AnimateWindow(hwnd, 200, AW_VER_POSITIVE | CloseOpen);
                        break;
                    case 4://从下到上显示
                        AnimateWindow(hwnd, 200, AW_VER_NEGATIVE | CloseOpen);
                        break;
                    case 5://透明渐变显示
                        AnimateWindow(hwnd, 500, AW_BLEND | CloseOpen);
                        break;
                    case 6://从中间向四周
                        AnimateWindow(hwnd, 500, AW_CENTER | CloseOpen);
                        break;
                    case 7://左上角伸展
                        AnimateWindow(hwnd, 200, AW_SLIDE | AW_HOR_POSITIVE | AW_VER_POSITIVE | CloseOpen);
                        break;
                    case 8://左下角伸展
                        AnimateWindow(hwnd, 200, AW_SLIDE | AW_HOR_POSITIVE | AW_VER_NEGATIVE | CloseOpen);
                        break;
                    case 9://右上角伸展
                        AnimateWindow(hwnd, 200, AW_SLIDE | AW_HOR_NEGATIVE | AW_VER_POSITIVE | CloseOpen);
                        break;
                    case 10://右下角伸展
                        AnimateWindow(hwnd, 1000, AW_SLIDE | AW_HOR_NEGATIVE | AW_VER_NEGATIVE | CloseOpen);
                        break;
                }

                if (CloseOrOpen == 1)
                {
                    foreach (System.Windows.Forms.Control mycontrol in myform.Controls)
                    {
                        string m = mycontrol.GetType().ToString();
                        m = m.Substring(m.Length - 5);
                        if (m == "Label")
                        {
                            mycontrol.Visible = true; //这里恢复LABEL的可视.
                        }
                    }
                }
            }
            catch { }
        }
    }
}
