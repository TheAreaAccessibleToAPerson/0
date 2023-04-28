namespace Butterfly.system.objects.SYSTEM.objects.main
{
    public class SystemObject<ObjectType> : Controller.Independent, description.activity.ISystem,
        description.access.ISystem where ObjectType : system.objects.main.Object, new()
    {
        private IInput<global::System.Action> ActionInvokeHandler;
        private IInput<poll.data.ticket.Struct[]> PollsHandler;

        protected sealed override void Construction()
        {
            PollsHandler = add_handler<handler.polls.PollsHandler, poll.data.ticket.Struct[]>();
            ActionInvokeHandler = add_handler<handler.node.ActionInvoke, global::System.Action>(); // ВСЕГДА В КОНЦЕ!!!!!!!...
        }

        void description.access.ISystem.AddActionInvoke(global::System.Action pAction)
        {
            ActionInvokeHandler.ToInput(pAction);
        }

        void description.access.ISystem.AddTicketsToThePoll(poll.data.ticket.Struct[] pPollTickets)
        {
            PollsHandler.ToInput(pPollTickets);
        }

        void description.activity.ISystem.Start()
        {
            information._hEADErRRR年.SHdJdDkOkDoDd____FFodkjfdsodfOW();

            ((system.objects.main.description.definition.INode)this).NodeDefine(this, this,
                        this, new System.Collections.Generic.Dictionary<string, object>(), GlobalData.SYSTEM_OBJECT_NAME, 0, new ulong[] {});

            ((system.objects.main.description.activity.INode)this).Create();

            global::System.Threading.Thread.CurrentThread.Priority = global::System.Threading.ThreadPriority.Highest;

            while (true)
            {
                global::System.GC.Collect();

                if (StateInformation.IsDestroying)
                {
                    return;
                }

                sleep(5000);
            }
        }

        void description.access.ISystem.Destroy() => destroy();
    }
}
