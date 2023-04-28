namespace Butterfly.system.objects.poll.objects.poll
{
    public enum InformingType
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

    public class Object : Controller.Thread.Local.value<data.setting.Struct>.Independent
    {
        /// <summary>
        /// Локер блокирующий доступ к Run.
        /// </summary>
        private readonly object ActionRunLocker = new object();

        /// <summary>
        /// Предназначен для получения информации о том начал ли текущий поток регистрацию
        /// обьектов внутри своего цикла добавленых в хранилища Subscribe и Unsubscribe.
        /// </summary>
        private bool IsProcessSubscribeUnsubscribe = false;
        private readonly object ProcessSubscribeUnsubscribeLocker = new object();

        /// <summary>
        /// Блокириует допуп к регистрации.
        /// </summary>
        private readonly object SubscribeUnsubscribeLocker = new object();

        /// <summary>
        /// Уникальный номер пула.
        /// </summary>~
        public ulong PollID { get { return localValue.PollID; } }

        /// <summary>
        /// Имя пула.
        /// </summary>
        public string Name { protected set; get; } = null;

        protected override sealed void Construction() { }

        public manager.Clients ClientsManager { private set; get; }

        private readonly collections.safe.Values<system.objects.poll.data.ticket.Struct> RegisterSubscribeValues
            = new collections.safe.Values<system.objects.poll.data.ticket.Struct>();

        private readonly collections.safe.Values<system.objects.poll.data.ticket.Struct> RegisterUnsubscribeValues
            = new collections.safe.Values<system.objects.poll.data.ticket.Struct>();

        public bool TrySubscribe(system.objects.poll.data.ticket.Struct pTicket)
        {
            // Для запуска обьекта требуется время. А зарегестрировать Action билета на выполенние 
            // нужно сейчас поэтому.
            if (Name == null)
            {
                Name = localValue.Name; // Name делаем указателем первого обращения.

                ClientsManager = new manager.Clients(this, localValue.Name, localValue.PollID, localValue.Size);
            }

            // Колчичесво выполняющихся System.Action + готовившихся к работе 
            // - количесво желающих отписаться не должно превышать максимально допустимое количесво.
            if ((ClientsManager.Count + RegisterSubscribeValues.Count - RegisterUnsubscribeValues.Count) < localValue.Size)
            {
                if (global::System.Threading.Monitor.TryEnter(ActionRunLocker))
                {
                    // Нам удалось захватить локер рассылающий сообщения.

                    // Попробуем захватить локер отвечающий за регестрацию клинтов...
                    if (global::System.Threading.Monitor.TryEnter(SubscribeUnsubscribeLocker))
                    {
                        ClientsManager.Subscribe(pTicket);

                        global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);
                    }
                    else
                    {
                        // Уже происходит регистрация клинтов из потока которые передает сюда данные.
                        // Дождемся пока она закончится и зарегестрируемя из системного потока.
                        lock (SubscribeUnsubscribeLocker)
                        {
                            ClientsManager.Subscribe(pTicket);
                        }
                    }

                    global::System.Threading.Monitor.Exit(ActionRunLocker);
                }
                else if (global::System.Threading.Monitor.TryEnter(SubscribeUnsubscribeLocker))
                {
                    lock (ProcessSubscribeUnsubscribeLocker)
                    {
                        if (IsProcessSubscribeUnsubscribe == false)
                        {
                            RegisterSubscribeValues.Add(pTicket);

                            global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);

                            return true;
                        }
                        else
                        {
                            lock (SubscribeUnsubscribeLocker)
                            {
                                ClientsManager.Subscribe(pTicket);
                            }
                        }
                    }

                    global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);
                }
                else
                {
                    lock (SubscribeUnsubscribeLocker)
                    {
                        ClientsManager.Subscribe(pTicket);
                    }
                }

                return true;
            }

            return false;
        }

        public void Unsubscribe(system.objects.poll.data.ticket.Struct pTicket)
        {
            if (global::System.Threading.Monitor.TryEnter(ActionRunLocker))
            {
                // Нам удалось захватить локер рассылающий сообщения.

                // Попробуем захватить локер отвечающий за регестрацию клинтов...
                if (global::System.Threading.Monitor.TryEnter(SubscribeUnsubscribeLocker))
                {
                    ClientsManager.Unsubscribe(pTicket);

                    global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);
                }
                else
                {
                    // Это означает что уже происходит регистрация клинтов из потока которые передает сюда данные.
                    // Дождемся пока она закончится и зарегестрируемя из системного потока.
                    lock (SubscribeUnsubscribeLocker)
                    {
                        ClientsManager.Unsubscribe(pTicket);
                    }
                }

                global::System.Threading.Monitor.Exit(ActionRunLocker);
            }
            else if (global::System.Threading.Monitor.TryEnter(SubscribeUnsubscribeLocker))
            {
                lock (ProcessSubscribeUnsubscribeLocker)
                {
                    if (IsProcessSubscribeUnsubscribe == false)
                    {
                        RegisterUnsubscribeValues.Add(pTicket);

                        global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);

                        return;
                    }
                    else
                    {
                        lock (SubscribeUnsubscribeLocker)
                        {
                            ClientsManager.Unsubscribe(pTicket);
                        }
                    }
                }

                global::System.Threading.Monitor.Exit(SubscribeUnsubscribeLocker);
            }
            else
            {
                lock (SubscribeUnsubscribeLocker)
                {
                    ClientsManager.Unsubscribe(pTicket);
                }
            }
        }

        // Каждые n времени обращаемся к хранилищю пулов для того что бы проверить нету ли у нас схожых по имени.
        // Проверяем количесво учасников, после чего пробуем захватить локер PollHandler для уточнения информации
        // и если необходимо разпределяем клинтов между ними или соединяем пуллы.
        private int CheckPollObjectsTimeDelay = 0;


        void Start()
        {
            add_thread($"Poll[Name:{localValue.Name}, ID:{localValue.PollID}, Size:{localValue.Size}, TimeDelay:{localValue.TimeDelay}",
            () =>
            {
                //CheckPollObjectsTimeDelay += step_timer();

                //if (CheckPollObjectsTimeDelay >= GlobalData.CHECK_POLL_OBJECTS_TIME_DELAY)
                //{
                    // Записываем количесво совпавших по имени пуллов и их загруженость.
                    //int[] matchesPolls = new int[0];

                    

                    //start_timer();
                //}

                lock (ProcessSubscribeUnsubscribeLocker) IsProcessSubscribeUnsubscribe = false;

                lock(ActionRunLocker)
                {
                    ClientsManager.ActionRun();
                }

                lock(SubscribeUnsubscribeLocker)
                {
                    lock (ProcessSubscribeUnsubscribeLocker) IsProcessSubscribeUnsubscribe = true;

                    if (RegisterUnsubscribeValues.ExtractAll(out system.objects.poll.data.ticket.Struct[] oUnsubscribeTickets))
                    {
                        ClientsManager.Unsubscribe(oUnsubscribeTickets);
                    }

                    if (RegisterSubscribeValues.ExtractAll(out system.objects.poll.data.ticket.Struct[] oSubscribeTickets))
                    {
                        ClientsManager.Subscribe(oSubscribeTickets);
                    }

                    if (ClientsManager.Count == 0)
                    {
                        localValue.DestroyPoll(this);
                    }
                }
            },
            (uint)localValue.TimeDelay, Thread.Priority.Normal);
        }
    }
}

