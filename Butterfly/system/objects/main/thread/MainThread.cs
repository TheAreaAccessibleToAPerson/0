namespace Butterfly.Thread
{
    public enum Priority
    {
        Lowest = 0,
        BelowNormal = 1,
        Normal = 2,
        AboveNormal = 3,
        Highest = 4
    }
}

namespace Butterfly.system.objects.main.thread
{
    public abstract class Object : main.Object, description.activity.IThread
    {
        private global::System.Threading.Thread[] Threads = new global::System.Threading.Thread[0];
        private int[] TimesDelay = new int[0];
        private string[] Prioritys = new string[0];

        private Bool[] IsRuns = new Bool[0];

        protected void add_thread(string pName, global::System.Action pAction, uint pTimeDelay, Thread.Priority pPriority)
        {
            if (StateInformation.IsStarting)
            {
                IsRuns = Hellper.ExpendArray(IsRuns, new Bool(true));
                Prioritys = Hellper.ExpendArray(Prioritys, pPriority.ToString());
                TimesDelay = Hellper.ExpendArray(TimesDelay, (int)pTimeDelay);

                if (pTimeDelay > 0)
                {
                    Threads = Hellper.ExpendArray(Threads, new global::System.Threading.Thread(() =>
                    {
                        int timeDelay = TimesDelay[TimesDelay.Length - 1];
                        Bool isRun = IsRuns[IsRuns.Length - 1];

                        while (true)
                        {
                            if (isRun.Value)
                            {
                                pAction.Invoke();

                                global::System.Threading.Thread.Sleep(timeDelay);
                            }
                            else
                            {
                                return;
                            }

                        }
                    }));
                }
                else
                {
                    Threads = Hellper.ExpendArray(Threads, new global::System.Threading.Thread(() =>
                    {
                        Bool isRun = IsRuns[IsRuns.Length - 1];

                        while (true)
                        {
                            if (isRun.Value)
                            {
                                pAction.Invoke();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }));
                }



                Threads[Threads.Length - 1].Name = pName;
                Threads[Threads.Length - 1].Priority = (global::System.Threading.ThreadPriority)pPriority;
            }
            else
                Exception(Ex.Thread.x10001);
        }

        void description.activity.IThread.Start()
        {
            foreach (var thread in Threads)
                thread.Start();
        }

        void description.activity.IThread.Stop()
        {
            if (Threads.Length == 0) return;

            string nameThreads = "";
            foreach(var nameThread in Threads)
                nameThreads += nameThread.Name + " ";


            SystemInformation($"StoppingThreads:{nameThreads}");

            for (int i = 0; i < IsRuns.Length; i++)
                IsRuns[i].False();

            bool[] isStopThreads = new bool[Threads.Length];

            int stopThreadsCount = 0;
            while (true)
            {
                for (int i = 0; i < Threads.Length; i++)
                {
                    if (isStopThreads[i] == false)
                    {
                        if (Threads[i].IsAlive)
                        {
                            //...
                        }
                        else
                        {
                            isStopThreads[i] = true;
                            stopThreadsCount++;

                            SystemInformation($"StopThread:" + Threads[i].Name);

                            if (stopThreadsCount == Threads.Length) break;
                        }
                    }
                }
                
                if (stopThreadsCount == Threads.Length) break;
            }

            SystemInformation($"StopThread.");
        }

        void description.activity.IThread.TaskStop()
        {
            System.Threading.Tasks.Task.Run(((description.activity.IThread)this).Stop);
        }
    }
}
