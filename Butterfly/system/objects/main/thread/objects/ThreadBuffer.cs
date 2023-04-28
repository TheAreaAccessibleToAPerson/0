namespace Butterfly.system.objects.main.thread.objects
{
    public class ThreadBuffer
    {
        public string Name;
        public global::System.Threading.Tasks.Task Thread;
        private global::System.Action BreakTheBlockAction;

        public ThreadBuffer(string pName, global::System.Threading.Tasks.Task pThread, global::System.Action pBreakTheBlockAction)
        {
            Name = pName;
            Thread = pThread;
            BreakTheBlockAction = pBreakTheBlockAction;
        }

        public void BreakTheBlock()
        {
            if (BreakTheBlockAction != null)
            {
                BreakTheBlockAction.Invoke();
            }
        }
    }
}
