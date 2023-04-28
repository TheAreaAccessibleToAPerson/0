namespace Butterfly.system.objects.main.manager.subscribe.objects
{
    public class Global : main.Informing
    {
        private struct StateData
        {
            public const string REGISTER_SUBSCRIBE = "RegisterSubscribe";
            public const string END_SUBSCRIBE = "EndSubscribe";
            public const string REGISTER_UNSUBSCRIBE = "RegisterUnsubscribe";
            public const string END_UNSUBSCRIBE = "EndUnsubscribe";
        }

        private readonly information.Header HeaderInformation;
        private readonly information.State StateInformation;
        private readonly information.Node NodeInformation;

        private readonly IInforming Informing;

        private readonly control.description.ISubscription Subscription;

        public Global(IInforming pInforming, information.Header pHeaderInformation, information.Node pNodeInformation,
            information.State pStateInformation, control.description.ISubscription pSubscription)
            : base("GlobalObjectsSubscribeManager", pInforming)
        {
            Informing = pInforming;

            HeaderInformation = pHeaderInformation;
            NodeInformation = pNodeInformation;
            StateInformation = pStateInformation;

            Subscription = pSubscription;
        }

        #region Listener

        private object Locker;

        /// <summary>
        /// Сдесь хранятся ID обьектов Sending на которые вы подписались, обезательно проверить
        /// что бы не подписатся дважды в одно место.
        /// </summary>
        private ulong[] IDSendingMessage = new ulong[0];
        /// <summary>
        /// Сдесь описан способ подписки и отписки от SendingMessage обькта.
        /// </summary>
        private main.objects.sending.description.registration.ISending[] RegisterSubscribeAndUnsubscribeSending
            = new main.objects.sending.description.registration.ISending[0];
        /// <summary>
        /// Сдесь хранятся обьекты Listener которое нужно зарегестрировать в SendingMessage.
        /// </summary>
        private object[] Listiners = new object[0];

        /// <summary>
        /// Наш прослушиватель будет хранится в многомерном массиве в нутри обькта рассылки сообщений.
        /// В данном массиве будет хранится точная информация о место нахождении нашего прослушивателя.
        /// При каждом обновлении места положения нам будет сообщатся новое место.
        /// При отключении от прослушивания может получится так, что в тот момент когда мы отправили данные на
        /// отключение его переместили. Для этого нужно будет повторно запросить его отключение.
        /// </summary>
        private int[] ListenerIndexArrayInSending;
        private int[] ListenerRangeArrayInSending;

        /// <summary>
        /// Количесво выставленых на подписку обьектов.
        /// Когда будет приходить сообщение об окончании регистрации
        /// будем дикрементировать данное значение.
        /// После того как это значение станет равно 0, сообщим что везде регистрация завершена.
        /// </summary>
        private int RegisterSubscribeCount = 0;

        /// <summary>
        /// Когда будет приходить сообщение об окончании регистрации на подписку будем это значение
        /// инкрементировать. После того как обьект начнет свое уничтожение, данное значение 
        /// будет декриментироватся, после того как оно станет равно 0 сообщим что мы отовсюду
        /// отписались.
        /// </summary>
        private int RegisterUnsubscribeCount = 0;

        /// <summary>
        /// Сдесь будет хранится текущая информация о Listener'ах.
        /// </summary>
        private main.objects.sending.InformingType[] ListenerCurrentInformation;

        private string ListenerSendingState = StateData.REGISTER_SUBSCRIBE;

        private void Subscribe()
        {
            lock(Locker)
            {
                if (ListenerSendingState == StateData.REGISTER_SUBSCRIBE)
                {
                    // Установим размеры массивы куда будет записыватся приходящая
                    // информация из Sending о текущем месте нахождения Listener.
                    ListenerIndexArrayInSending = new int[IDSendingMessage.Length];
                    ListenerRangeArrayInSending = new int[IDSendingMessage.Length];
                    ListenerCurrentInformation = new main.objects.sending.InformingType[IDSendingMessage.Length];

                    //Запишем колчесво мест где нужно зарегестрировать подписку.
                    RegisterSubscribeCount = RegisterSubscribeAndUnsubscribeSending.Length;
                    RegisterUnsubscribeCount = RegisterSubscribeAndUnsubscribeSending.Length;

                    for (int i = 0; i < RegisterSubscribeAndUnsubscribeSending.Length; i++)
                    {
                        RegisterSubscribeAndUnsubscribeSending[i].Subscribe(NodeInformation.ID, i, Listiners[i], ToSendingInformation);
                    }
                }
                else
                    Exception(Ex.SubscribeObjectsGlobal.x10003, StateData.REGISTER_SUBSCRIBE, ListenerSendingState);
            }
        }

        private void Unsubscribe()
        {
            lock(Locker)
            {
                if (ListenerSendingState == StateData.END_SUBSCRIBE)
                {
                    ListenerSendingState = StateData.REGISTER_UNSUBSCRIBE;

                    for (int i = 0; i < RegisterSubscribeAndUnsubscribeSending.Length; i++)
                    {
                        RegisterSubscribeAndUnsubscribeSending[i].Unsubscribe
                            (NodeInformation.ID, i, ListenerRangeArrayInSending[i], ListenerIndexArrayInSending[i]);
                    }
                }
                else
                    Exception(Ex.SubscribeObjectsGlobal.x10005, StateData.END_SUBSCRIBE, ListenerSendingState);
            }
        }

        /// <summary>
        /// С помощью данного метода SendingMessage отбащается с нами.
        /// </summary>
        /// <param name="pIndexInArrayStorage">Индекс созданого Listener обьекта.(Используется для хранения в массивах в текущем классе.)</param>
        /// <param name="pIndexRangeInSending">Индекс ранга в многомерном массиве. По номеру которого хранится Listener обьект в Sending на который он подписался.</param>
        /// <param name="pIndexArrayInSending">Индекс в массиве.По номеру которого хранится Listener обьект в Sending, на который подписался. </param>
        /// <param name="pInformingType">Тип информирования от Sending.</param>
        private void ToSendingInformation(ulong pIDObject, main.objects.sending.InformingType pInformingType, int pIndexInArrayStorage, 
            int pIndexRangeInSending, int pIndexArrayInSending)
        {
            lock (Locker)
            {
                if (pInformingType.HasFlag(main.objects.sending.InformingType.EndSubscribe))
                {
                    if (ListenerSendingState == StateData.REGISTER_SUBSCRIBE)
                    {
                        if (pIDObject == NodeInformation.ID)
                        {
                            if ((RegisterSubscribeCount - 1) >= 0)
                            {
                                ListenerRangeArrayInSending[pIndexInArrayStorage] = pIndexRangeInSending;
                                ListenerIndexArrayInSending[pIndexInArrayStorage] = pIndexArrayInSending;

                                // Мы везде подписались, сообщим об этом в manager.control.Subscription.
                                if (--RegisterSubscribeCount == 0)
                                {
                                    Subscription.EndSubscribe(control.SubscriptionType.ListingSending);

                                    ListenerSendingState = StateData.END_SUBSCRIBE;
                                }
                            }
                            else
                                Exception(Ex.SubscribeObjectsGlobal.x10007, pInformingType.ToString());
                        }
                        else
                            Exception("DKJF!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!DKJF");
                    }
                    else
                        Exception(Ex.SubscribeObjectsGlobal.x10004, StateData.REGISTER_SUBSCRIBE, ListenerSendingState);
                }
                else if (pInformingType.HasFlag(main.objects.sending.InformingType.ChangeOfIndex))
                {
                    // Запишим новое место положение.
                    ListenerRangeArrayInSending[pIndexInArrayStorage] = pIndexRangeInSending;
                    ListenerIndexArrayInSending[pIndexInArrayStorage] = pIndexArrayInSending;

                    // Если вы находитесь в состоянии ожидания сообщения об окончании отписки
                    // и пришло сообщение о том что место положение было изменено, то сообщение
                    // нужно продублировать.
                    if (ListenerSendingState == StateData.REGISTER_UNSUBSCRIBE)
                    {
                        NodeInformation.SystemAccess.AddActionInvoke(() => RepeatUnsubscribe(pIndexInArrayStorage));
                    }
                }
                else if (pInformingType.HasFlag(main.objects.sending.InformingType.EndUnsubscribe))
                {
                    if (ListenerSendingState == StateData.REGISTER_UNSUBSCRIBE)
                    {
                        if ((RegisterUnsubscribeCount - 1) >= 0)
                        {
                            if (--RegisterUnsubscribeCount == 0)
                            {
                                ListenerSendingState = StateData.END_UNSUBSCRIBE;

                                Subscription.EndUnsubscribe(control.SubscriptionType.ListingSending);
                            }
                        }
                        else
                            Exception(Ex.SubscribeObjectsGlobal.x10002);
                    }
                    else
                        Exception(Ex.SubscribeObjectsGlobal.x10006, StateData.REGISTER_UNSUBSCRIBE, ListenerSendingState);
                }
                else
                    Exception("KDFJKDJFKDJFKDJFKDJFK");
            }
        }

        /// <summary>
        /// Дублируем место сообщение об регистрации на отписку.
        /// </summary>
        /// <param name="pIndexInArrayStorage">Индекс где хранятся данные для повторной отправки.</param>
        private void RepeatUnsubscribe(int pIndexInArrayStorage)
        {
            RegisterSubscribeAndUnsubscribeSending[pIndexInArrayStorage].Unsubscribe
                        (NodeInformation.ID, pIndexInArrayStorage, ListenerRangeArrayInSending[pIndexInArrayStorage], ListenerIndexArrayInSending[pIndexInArrayStorage]);
        }

        /// <summary>
        /// Передаем в метод глобальный обьект который рассылает сообщения и 
        /// обьект который нужно подписать на прослушку этих сообщений.
        /// За подписания на обьект отвечает Node обьект.
        /// </summary>
        /// <typeparam name="ValueMessage">Тип рассылаемых данных</typeparam>
        /// <param name="pGlobalSendingMessageObject">Глабальный SendingMessage.</param>
        /// <param name="pListenerSendingMessageObject">Обьект который должен получать сообщения.</param>
        public void RegisterListenerObjectInSendingMessageObject<ValueMessage>
            (main.objects.sending.Object<ValueMessage> pGlobalSendingMessageObject,
                main.objects.sending.listener.Object<ValueMessage> pListenerSendingMessageObject)
        {
            if (HeaderInformation.IsNodeObject())
            {
                // При первом добавлении обьекта зарегетрируем методы отвечающие за подписки и отписку,
                // а также инициализируем Locker.
                if (IDSendingMessage.Length == 0)
                {
                    Subscription.AddSubscribeAndUnsubscribe(control.SubscriptionType.ListingSending, Subscribe, Unsubscribe);
                    Locker = new object();
                }
                    
                // Проверяем не подписались ли мы из текущего узла уже на эту рассылку сообщений.
                foreach(ulong id in IDSendingMessage)
                    if (id == pGlobalSendingMessageObject.ID)
                    {
                        Exception(Ex.SubscribeObjectsGlobal.x10001, 
                            $"{pGlobalSendingMessageObject.GetExplorerObject()}[{pGlobalSendingMessageObject.Name}]");

                        return;
                    }

                IDSendingMessage = Hellper.ExpendArray(IDSendingMessage, pGlobalSendingMessageObject.ID);
                RegisterSubscribeAndUnsubscribeSending = Hellper.ExpendArray(RegisterSubscribeAndUnsubscribeSending, pGlobalSendingMessageObject);
                Listiners = Hellper.ExpendArray(Listiners, pListenerSendingMessageObject);
            }
            else if (HeaderInformation.IsBranchObject())
            {
                // Если эта ветка, происведем регистрацию через NodeObject.
                ((description.add.IGlobal)NodeInformation.NodeObject).RegisterListenerObjectInSendingMessageObject
                    (pGlobalSendingMessageObject, pListenerSendingMessageObject);
            }
        }

        #endregion
    }
}
