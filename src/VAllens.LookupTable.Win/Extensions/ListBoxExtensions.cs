using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms;

namespace VAllens.LookupTable
{
    public static class ListBoxExtensions
    {
        private static readonly ConcurrentQueue<string> LogQueue;

        static ListBoxExtensions()
        {
            LogQueue = new ConcurrentQueue<string>();
        }

        /// <summary>
        /// 委托添加日志到控件，避免窗体假死
        /// </summary>
        private static void AddItemToLog(ListBox lbx, string content)
        {
            lbx.Invoke((MethodInvoker) delegate { lbx.Items.Add(content); });
        }

        /// <summary>
        /// 读取日志队列并且输出屏幕
        /// </summary>
        private static void Run(this ListBox lbx)
        {
            while (true)
            {
                string item;
                if (LogQueue.TryDequeue(out item))
                {
                    AddItemToLog(lbx, item);
                }
            }
        }

        /// <summary>
        /// 添加日志到队列
        /// </summary>
        public static void AddItem(this ListBox lbx, string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                LogQueue.Enqueue(content);
            }
        }

        /// <summary>
        /// 启动子线程读取日志队列并且输出屏幕
        /// </summary>
        public static void Start(this ListBox lbx)
        {
            LogQueue.Clear();

            //启动线程
            Thread thread = new Thread(() => { Run(lbx); });

            thread.Start();
        }
    }
}