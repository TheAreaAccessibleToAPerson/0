namespace Butterfly.system.objects.SYSTEM.objects.handler.node
{
    /// <summary>
    /// Вызывает Action.
    /// </summary>
    public sealed class ActionInvoke : Handler<global::System.Action>
    {
        private readonly collections.safe.Values<global::System.Action> Values 
            = new collections.safe.Values<global::System.Action>();

        protected override void Construction()
        {
            add_tegs(GlobalData.TASK_STOPPING_THREAD);

            input_to(Values.Add);
        }

        void Start()
        {
            add_thread("ActionInvoke", Update, GlobalData.SYSTEM_ACTION_INVOKE_TIME_DELAY, Thread.Priority.Highest);
        }

        private void Update()
        {
            if (Values.ExtractAll(out global::System.Action[] oActions))
            {
                foreach (global::System.Action action in oActions)
                {
                    action.Invoke();
                }
            }
        }
    }
}
