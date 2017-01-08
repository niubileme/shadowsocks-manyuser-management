using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSMM.Services.Core
{
    public class BackgroundThread
    {
        private Thread innerThread;
        public BackgroundThread()
        {

        }
        public void Start()
        {
            innerThread = new Thread(new ParameterizedThreadStart(Work));
            innerThread.Priority = ThreadPriority.Lowest;
            innerThread.Start();
        }
        public void Stop()
        {
            innerThread.Abort();
        }

        private void Work(object obj)
        {
            while (true)
            {
                Thread.Sleep(60 * 60 * 1000);
            }
        }
    }
}
