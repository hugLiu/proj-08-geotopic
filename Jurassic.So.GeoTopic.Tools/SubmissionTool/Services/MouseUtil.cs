using Jurassic.AppCenter;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
using Microsoft.Practices.CompositeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Jurassic.AppCenter.Logs;
using System.Xml.Serialization;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Jurassic.So.GeoTopic.SubmissionTool.Services
{
    /// <summary>鼠标工具</summary>
    public static class MouseUtil
    {
        [DllImport("user32")]
        private static extern int mouse_event(
            int dwFlags,// 下表中标志之一或它们的组合 
            int dx,
            int dy, //指定x，y方向的绝对位置或相对位置 
            int cButtons,//没有使用 
            int dwExtraInfo//没有使用 
            );
        const int MOUSEEVENTF_MOVE = 0x0001;     // 移动鼠标 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002; //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;// 模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000; //标示是否采用绝对坐标        
        const int XDown = 0x0080;
        const int XUp = 0x0100;
        /// <summary>上移</summary>
        public static void MoveUp(int height)
        {
            var flags = MOUSEEVENTF_MOVE;
            mouse_event(flags, 0, -height, 0, 0);
        }
        /// <summary>下移</summary>
        public static void MoveDn(int height)
        {
            var flags = MOUSEEVENTF_MOVE;
            mouse_event(flags, 0, height, 0, 0);
        }
    }
}
