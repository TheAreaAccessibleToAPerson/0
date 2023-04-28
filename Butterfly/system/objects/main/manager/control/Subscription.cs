namespace Butterfly.system.objects.main.manager.control
{
    public enum SubscriptionType
    {
        /// <summary>
        /// Подписка на пуллы.
        /// </summary>
        Poll = 1,

        /// <summary>
        /// Простушивание сообщений.
        /// </summary>
        ListingSending
    }

    /// <summary>
    /// Данный класс отвечает за контроль подписок и отписок.
    /// </summary>
    public class Subscription : Informing, description.ISubscription
    {
        /// <summary>
        /// Карта подписок.
        /// </summary>
        private struct Map
        {
            // Регистрация в пуллы.
            public const int POLL = 0;

            /// <summary>
            /// Регистрация на прослушку сообщений.
            /// </summary>
            public const int LISTING_SENDING = 1; 
        }

        private readonly information.State StateInformation;
        private readonly information.Node NodeInformation;

        /// <summary>
        /// Сдесь будут записаны наименования всех подписок.
        /// </summary>
        private string[] SubscribesName = new string[0];

        /// <summary>
        /// После того как все обьекты подпишутся, запустим продолжение создания обьекта.
        /// </summary>
        private readonly global::System.Action ContinueStartingNode;

        /// <summary>
        /// Продолжает остановку обьекта.
        /// </summary>
        private readonly global::System.Action ContinueStopNode;

        public Subscription(IInforming pInforming, information.State pStateInformation, information.Node pNodeInformation, global::System.Action pContinueStartingNode, 
            global::System.Action pContinueStopNode) 
            : base("SubscribtionControlManager", pInforming)
        {
            Subscribes = new SubscriptionType[1];
            SubscribeActions = new global::System.Action[2];
            UnsubscribeActions = new global::System.Action[2];

            ContinueStartingNode = pContinueStartingNode;
            ContinueStopNode = pContinueStopNode;

            StateInformation = pStateInformation;
            NodeInformation = pNodeInformation;

            Locker = new object();

            SubscribeCount = 0;
        }

        private readonly SubscriptionType[] Subscribes;
        private readonly global::System.Action[] SubscribeActions;
        private readonly global::System.Action[] UnsubscribeActions;


        /// <summary>
        /// Локальный локер.
        /// </summary>
        private object Locker;

        /// <summary>
        /// Сюда будет инкрементировано количесво доступных для данного обьекта видов подписок, 
        /// пуллы, прослушиватели глобальных рассылок ... и тд..
        /// </summary>
        private int SubscribeCount = 0;

        /// <summary>
        /// Эта значение будет инкременитироватся после каждого сообщения об подписании и начале работы.
        /// </summary>
        private int EndSubscribe = 0;

        /// <summary>
        /// Запустить подписку.
        /// </summary>
        public string[] StartSubscribe()
        {
            if (SubscribeCount == 0)
            {
                SystemInformation("Текущий обьект не на что не подписан.");

                ContinueStartingNode.Invoke();
            }
            else
            {
                lock (Locker)
                {
                    for (int i = 0; i < SubscribeActions.Length; i++)
                    {
                        if (SubscribeActions[i] != null)
                        {
                            SubscribeActions[i].Invoke();
                        }
                    }
                }
            }

            return SubscribesName;
        }

        /// <summary>
        /// Запустить процесс отписки.
        /// </summary>
        public void StartUnsubscribe()
        {
            lock(Locker)
            {
                //Если мы не куда не подписаны, то просто продожим уничтожение.
                if (SubscribeCount == 0)
                {
                    SystemInformation("Текущему обьекту неотчего отписываться, так как он не куда не подписан.");

                    ContinueStopNode.Invoke();
                }
                else
                {
                    for (int i = UnsubscribeActions.Length - 1; i >= 0; i--)
                    {
                        UnsubscribeActions[i]?.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// Добовляем Action через котороый нужно будет подписаться.
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pSubscribeAction"></param>
        void description.ISubscription.AddSubscribeAndUnsubscribe(SubscriptionType pType, global::System.Action pSubscribeAction, 
            global::System.Action pUnsubscribeAction)
        {
            // Мы можем довать обьекты на подписку только в момент состоянии создания.
            if (StateInformation.IsCreating)
            {
                if (pType.HasFlag(SubscriptionType.Poll))
                {
                    if (SubscribeActions[Map.POLL] == null)
                    {
                        SubscribesName = Hellper.ExpendArray(SubscribesName, SubscriptionType.Poll.ToString());

                        SubscribeActions[Map.POLL] = pSubscribeAction;
                        UnsubscribeActions[Map.POLL] = pUnsubscribeAction;

                        SubscribeCount++;
                    }
                    else
                        Exception(Ex.SubscibtionManager.x10002, "Poll");
                }
                else if (pType.HasFlag(SubscriptionType.ListingSending))
                {
                    if (SubscribeActions[Map.LISTING_SENDING] == null)
                    {
                        SubscribesName = Hellper.ExpendArray(SubscribesName, SubscriptionType.ListingSending.ToString());

                        SubscribeActions[Map.LISTING_SENDING] = pSubscribeAction;
                        UnsubscribeActions[Map.LISTING_SENDING] = pUnsubscribeAction;
                        
                        SubscribeCount++;
                    }
                    else
                        Exception(Ex.SubscibtionManager.x10002, "ListingSending");
                }
            }
            else
                Exception(Ex.SubscibtionManager.x10001, information.State.Data.CREATING, StateInformation.Get());
        }

        void description.ISubscription.EndSubscribe(SubscriptionType pType)
        {
            lock(Locker)
            {
                // Все обьекты подписались.
                // Продолжаем создание обьека.
                if ((++EndSubscribe) == SubscribeCount)
                {
                    NodeInformation.SystemAccess.AddActionInvoke(ContinueStartingNode.Invoke);
                }
                else if (EndSubscribe > SubscribeCount)
                    Exception(Ex.SubscibtionManager.x10005);
            }
        }

        void description.ISubscription.EndUnsubscribe(SubscriptionType pType)
        {
            lock(Locker)
            {
                SystemInformation("Тещий обьект закончил процедуру отписки.");

                if (--SubscribeCount == 0)
                {
                    NodeInformation.SystemAccess.AddActionInvoke(ContinueStopNode.Invoke);
                }
                else if (SubscribeCount < 0)
                    Exception(Ex.SubscibtionManager.x10006);
            }
        }
    }
}
