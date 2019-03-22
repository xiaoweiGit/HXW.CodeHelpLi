using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXW.CodeHelpLi
{
    /// <summary>
    /// 异步操作
    /// </summary>
    public class CWorker
    {
        /// <summary>
        /// 对象
        /// </summary>
        private BackgroundWorker backgroundWorker;

        /// <summary>
        /// 后台执行的操作
        /// </summary>
        public Action BackgroundWork { get; set; }

        /// <summary>
        /// 后台任务执行完毕后事件
        /// </summary>
        public event EventHandler<BackgroundWorkerEventArgs> BackgroundWorkerCompleted;

        private BackgroundWorkerEventArgs _eventArgs;//异常参数

        /// <summary>
        /// 构造
        /// </summary>
        public CWorker()
        {
            _eventArgs = new BackgroundWorkerEventArgs();
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
        }

        /// <summary>
        /// 开始工作
        /// </summary>
        public void BegionWork()
        {
            if (backgroundWorker.IsBusy)
                return;
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// 工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (BackgroundWork != null)
            {
                try
                {
                    BackgroundWork();
                }
                catch (Exception ex)
                {
                    _eventArgs.BackGroundException = ex;
                }
            }
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.BackgroundWorkerCompleted != null)
            {
                this.BackgroundWorkerCompleted(sender, _eventArgs);
            }
        }
    }

    /// <summary>
    /// 事件
    /// </summary>
    public class BackgroundWorkerEventArgs : EventArgs
    {
        /// <summary>
        /// 后台程序运行时抛出的异常
        /// </summary>
        public Exception BackGroundException { get; set; }
    }
}
