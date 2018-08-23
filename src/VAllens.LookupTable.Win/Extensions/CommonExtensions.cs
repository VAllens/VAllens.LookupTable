using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VAllens.LookupTable
{
    public static class CommonExtensions
    {
        /// <summary>
        /// 测量耗时并且输出日志
        /// </summary>
        public static void Watch(this Stopwatch stopwatch, Action action, ListBox lbx, string actionName)
        {
            stopwatch.Reset();
            stopwatch.Start();
            action();
            stopwatch.Stop();

            stopwatch.WriteElapsedLog(lbx, actionName);
        }

        /// <summary>
        /// 输出耗时日志
        /// </summary>
        public static void WriteElapsedLog(this Stopwatch stopwatch, ListBox lbx, string actionName)
        {
            TimeSpan timeSpan = stopwatch.Elapsed; // 获取当前实例测量得出的总时间
            string milliseconds = timeSpan.TotalMilliseconds.ToString("#0.00000000"); // 总毫秒数
            lbx.AddItem(string.Format("{0} 总耗时 {1} 毫秒", actionName, milliseconds));
            lbx.AddItem(string.Empty);
        }
    }
}