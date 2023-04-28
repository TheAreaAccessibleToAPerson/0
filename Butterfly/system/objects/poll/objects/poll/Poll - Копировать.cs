namespace Butterfly.system.objects.poll.objects.poll
{
    public enum InformingType1
    {
        None = 0,

        /// <summary>
        /// Обьект подписан и начал свою работу.
        /// </summary>
        EndSubscribe = 1,

        /// <summary>
        /// Обькт окончил свою работу и был отписан.
        /// </summary>
        EndUnsubscribe = 2,

        /// <summary>
        /// Обьект изменил свою позицию.
        /// </summary>
        ChangeOfIndex = 4
    }

    public abstract class Object1 : Controller.Thread.Local.value<data.setting.Struct>.Independent
    {
        /// <summary>
        /// Локер для дабавления и удаления содержимого пула.
        /// </summary>
        private readonly object Locker = new object();
        /// <summary>
        /// Уникальный номер пула.
        /// </summary>
    
        /// <summary>
        /// ID Пулла.
        /// </summary>
        private readonly ulong PollID;

        /// <summary>
        /// Массив для хранения выполняющиемя System.Action.
        /// </summary>
        public global::System.Action[] Runs
            = new global::System.Action[GlobalData.ELASTIC_POLL_SIZE];

        /// <summary>
        /// Информирование клиента:
        /// 1)Тип информирования 
        /// 2) Индекс хранения билета в SubsribePollManager, 
        /// 3) ID пула, 
        /// 4)Индекс хранения клинта в SubscribeClient(this).
        /// </summary>
        public global::System.Action<objects.poll.InformingType, int, ulong, int>[] ToInformings
            = new global::System.Action<objects.poll.InformingType, int, ulong, int>[GlobalData.ELASTIC_POLL_SIZE];
        /// <summary>
        /// Уникальные ID клинтов которые используют данный пулл.
        /// </summary>
        public ulong[] IDClients = new ulong[GlobalData.ELASTIC_POLL_SIZE];
        /// <summary>
        /// Номер индекса в массиве PollSubscribeManger.
        /// </summary>
        public int[] ClientsPointerIndexInPollSubscribeManager = new int[GlobalData.ELASTIC_POLL_SIZE];

        /// <summary>
        /// Последний индекс обрабатываемого Action.
        /// </summary>
        private int IndexToEmptySlot = 0; // Количесво обрабатываемых System.Action.

        /// <summary>
        /// Количесво клинтов.
        /// </summary>
        public int Count { get { return IndexToEmptySlot; } }

        /// <summary>
        /// Имя пулла.
        /// </summary>
        public string Name { get { return localValue.Name; }}

        protected override sealed void Construction() { }
        protected abstract void ActionRun();

        private manager.Clients ClientsManager;

        private readonly collections.safe.Values<system.objects.poll.data.ticket.Struct> RegisterSubscribeValues
            = new collections.safe.Values<system.objects.poll.data.ticket.Struct>();

        private readonly collections.safe.Values<system.objects.poll.data.ticket.Struct> RegisterUnsubscribeValues
            = new collections.safe.Values<system.objects.poll.data.ticket.Struct>();

        public bool TrySubscribe(system.objects.poll.data.ticket.Struct pTicket)
        {
            if (GlobalData.ELASTIC_POLL_SIZE == IndexToEmptySlot)
            {
                
            }

            RegisterSubscribeValues.Add(pTicket);

            return false;
        }

        public void Unsubscribe(system.objects.poll.data.ticket.Struct pTicket)
        {
            RegisterUnsubscribeValues.Add(pTicket);
        }

        void Start()
        {
            add_thread($"Poll[Name:{localValue.Name}, ID:{localValue.PollID}, Size:Elastic, TimeDelay:{localValue.TimeDelay}",
            () =>
            {
                if (RegisterSubscribeValues.Count > 0 || RegisterUnsubscribeValues.Count > 0 || ClientsManager.Count == 0)
                {
                    if (RegisterUnsubscribeValues.TryExtractAll(out system.objects.poll.data.ticket.Struct[] oUnsubscribeTickets))
                    {
                        ClientsManager.Unsubscribe(oUnsubscribeTickets);
                    }

                    if (RegisterSubscribeValues.TryExtractAll(out system.objects.poll.data.ticket.Struct[] oSubscribeTickets))
                    {
                        ClientsManager.Subscribe(oSubscribeTickets);
                    }

                    // Отписываем зарегетрированых на отписку.
                    if (RegisterUnsubscribeValues.Count > 0)
                    {
                        /*
                        if (RegisterUnsubscribeValues.ExtractAll(out system.objects.poll.data.ticket.Struct[] oUnsubscribeTickets))
                        {
                            ClientsManager.Unsubscribe(oUnsubscribeTickets);
                        }
                        */
                    }
                    else if (RegisterSubscribeValues.Count > 0)
                    {
                        /*
                        if (RegisterSubscribeValues.ExtractAll(out system.objects.poll.data.ticket.Struct[] oSubscribeTickets))
                        {
                            ClientsManager.Subscribe(oSubscribeTickets);
                        }
                        */
                    }
                    else if (ClientsManager.Count == 0)
                    {
                        if (RegisterSubscribeValues.ExtractAll(out system.objects.poll.data.ticket.Struct[] oSubscribe1Tickets))
                        {
                            ClientsManager.Subscribe(oSubscribe1Tickets);
                        }
                        else
                        {
                            //localValue.DestroyPoll(this);
                        }
                    }
                }

                //Console("PollID:" + PollID + " " + ClientsManager.Count.ToString() + " " + PollID);

                ClientsManager.ActionRun();
            },
            (uint)localValue.TimeDelay, Thread.Priority.Normal);
        }
    }
}

