namespace Butterfly.system.objects.main.manager.system.polls
{
    public class Access : Informing
    {
        public struct StateType
        {
            /// <summary>
            /// Создание билетов.
            /// </summary>
            public const string CREATING_TICKET = "CreatingTicket";

            /// <summary>
            /// Регистрируем подписки.
            /// </summary>
            public const string REGISTER_SUBSCRIBE = "RegisterSubscribe";

            /// <summary>
            /// Все подписки зарегистрировались.
            /// </summary>
            public const string END_SUBSCRIBE = "EndSubscribe";

            /// <summary>
            /// Регистрируем все отписки.
            /// </summary>
            public const string REGISTER_UNSUBSCRIBE = "RegisterUnsubscribe";

            /// <summary>
            /// Мы отписались ото всех отписок.
            /// </summary>
            public const string END_UNSUBSCRIBE = "EndUnsubscribe";
        }

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        private string State = StateType.CREATING_TICKET;

        private readonly information.Header HeaderInformation;
        private readonly information.State StateInformation;
        private readonly information.Node NodeInformation;

        private poll.data.ticket.Struct[] StructSubscribeTickets = new poll.data.ticket.Struct[0];

        /// <summary>
        /// Сдесь хранятся индексы пуллов в которых работают наши Action.
        /// </summary>
        private ulong[] PointerToThePollID;
        /// <summary>
        /// Указан индекс в массиве в менеджере пуллов где хранится наш обрабатываемый Action.
        /// </summary>
        private int[] PointerIndexInArrayToThePoll;

        /// <summary>
        /// Количесво подписаных билетов. Это значение нужно для того что бы при уничтожении
        /// обьекта дождатся пока мы отключимся от всех пулов на которые подписались.
        /// </summary>
        private int SubscribeTicketCount;

        private readonly System.Lazy<object> Locker = new System.Lazy<object>();

        /// <summary>
        /// Подписываем регистрационый метод.
        /// </summary>
        private readonly control.description.ISubscription Subscription;

        public Access(IInforming pInforming, information.Header pHeaderInformation, information.Node pNodeInformation, 
            information.State pStateInformation, control.description.ISubscription pSubscription)
            : base("PollsSystemAccess", pInforming)
        {
            HeaderInformation = pHeaderInformation;
            StateInformation = pStateInformation;
            NodeInformation = pNodeInformation;

            Subscription = pSubscription;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pInformingType">Тип информирования</param>
        /// <param name="pIndexTicketInSubscribeManager">Индекс в массиве в текущем менеджере подписки в пулы где 
        /// хранится информация о место положении обрабатывемого Action.</param>
        /// <param name="pIDPoll">ID пулла в котором мы работаем.</param>
        /// <param name="pIndexInPoll">Номер индекса в пулле хде хранится на Action.</param>
        private void ToInforming(poll.objects.poll.InformingType pInformingType, int pIndexTicketInSubscribeManager, ulong pIDPoll, int pIndexInPoll)
        {
            lock (Locker)
            {
                // Сообщает что мы отовсюду отписались;
                if (pInformingType.HasFlag(poll.objects.poll.InformingType.EndUnsubscribe))
                {
                    //lock(Locker)
                    {
                        if (StateInformation.IsStop)
                        {
                            if (State == StateType.REGISTER_UNSUBSCRIBE)
                            {
                                if (PointerToThePollID[pIndexTicketInSubscribeManager] != 0)
                                {
                                    if (PointerToThePollID[pIndexTicketInSubscribeManager] == pIDPoll && 
                                        PointerIndexInArrayToThePoll[pIndexTicketInSubscribeManager] == pIndexInPoll)
                                    {
                                        if ((SubscribeTicketCount - 1) >= 0)
                                        {
                                            // Если мы отписались отовсех пулов, то сообщим об этом
                                            if (--SubscribeTicketCount == 0)
                                            {
                                                State = StateType.END_UNSUBSCRIBE;
                                                Subscription.EndUnsubscribe(control.SubscriptionType.Poll);
                                            }
                                        }
                                        else
                                            Exception(Ex.SubscribeToPoll.x10012);
                                    }
                                }
                                else
                                    Exception(Ex.SubscribeToPoll.x10001, StructSubscribeTickets[pIndexTicketInSubscribeManager].Name);
                            }
                            else
                                Exception(Ex.SubscribeToPoll.x10011, StateType.REGISTER_UNSUBSCRIBE, State);
                        }
                        else
                            Exception(Ex.SubscribeToPoll.x10010, information.State.Data.STOP, StateInformation.Get());
                    }
                }
                else if (pInformingType.HasFlag(poll.objects.poll.InformingType.ChangeOfIndex))
                {
                    PointerToThePollID[pIndexTicketInSubscribeManager] = pIDPoll;
                    PointerIndexInArrayToThePoll[pIndexTicketInSubscribeManager] = pIndexInPoll;
                    
                    // В момент отписки наше место положение изменилось, повторное информирование
                    // с новым местом лежит на нас.
                    if (State == StateType.REGISTER_UNSUBSCRIBE)
                    {
                        NodeInformation.SystemAccess.AddActionInvoke(() => RepeetUnsubscribe(pIndexTicketInSubscribeManager));
                    }
                }
                else if (pInformingType.HasFlag(poll.objects.poll.InformingType.EndSubscribe))
                {
                    if (StateInformation.IsCreating)
                    {
                        //lock(Locker)
                        {
                            if ((SubscribeTicketCount + 1) <= PointerToThePollID.Length)
                            {
                                if (State == StateType.REGISTER_SUBSCRIBE)
                                {
                                    // Запишем куда зарегестрировался наш тикет.
                                    PointerToThePollID[pIndexTicketInSubscribeManager] = pIDPoll;
                                    PointerIndexInArrayToThePoll[pIndexTicketInSubscribeManager] = pIndexInPoll;

                                    SystemInformation("Мы получили сообщение об окончании регистрации пулле с id:" + pIDPoll + " на место " + pIndexInPoll);

                                    // Дожидаемся пока все билеты откликрутся о завершении регистрации.
                                    if (++SubscribeTicketCount == PointerToThePollID.Length)
                                    {
                                        State = StateType.END_SUBSCRIBE;

                                        //Сообщаем в менеджер подписок что мы на все подписались.
                                        Subscription.EndSubscribe(control.SubscriptionType.Poll);
                                    }
                                }
                                else
                                    Exception(Ex.SubscribeToPoll.x10009, StateType.REGISTER_SUBSCRIBE, State);
                            }
                            else
                                Exception(Ex.SubscribeToPoll.x10007, PointerToThePollID.Length.ToString());
                        }
                    }
                    else
                        Exception(Ex.SubscribeToPoll.x10008, information.State.Data.CREATING, StateInformation.Get());
                }
                else
                    Exception(Ex.SubscribeToPoll.x10002, pInformingType.ToString());
            }
        }

        private void Subscribe()
        {
            if (StateInformation.IsCreating)
            {
                if (State == StateType.CREATING_TICKET)
                {
                    State = StateType.REGISTER_SUBSCRIBE;

                    // Сюда запишутся данные о том в каком пуле зарегестрирован наш билет.
                    // Если в последсвии билет будет передан в другой пулл, то
                    // он обязан будет сообщить о том где он в данный момент работает.
                    PointerToThePollID = new ulong[StructSubscribeTickets.Length];

                    PointerIndexInArrayToThePoll = new int[StructSubscribeTickets.Length];
                    // Поставим в очередь на регистрацию.
                    NodeInformation.SystemAccess.AddTicketsToThePoll(StructSubscribeTickets);
                }
                else
                    Exception(Ex.SubscribeToPoll.x10006, StateType.CREATING_TICKET, State);
            }
            else
                Exception(Ex.SubscribeToPoll.x10005, information.State.Data.CREATING, StateInformation.Get());
        }

        /// <summary>
        /// Регистрируемся на отписку из пуллов.
        /// </summary>
        private void Unsubscribe()
        {
            if (SubscribeTicketCount > 0)
            {
                State = StateType.REGISTER_UNSUBSCRIBE;

                poll.data.ticket.Struct[] structUnsubscribeTickets
                = new poll.data.ticket.Struct[StructSubscribeTickets.Length];

                for (int i = 0; i < StructSubscribeTickets.Length; i++)
                {
                    structUnsubscribeTickets[i] = new poll.data.ticket.Struct
                        (poll.data.ticket.Struct.Type.UNSUBSCRIBE, i, PointerToThePollID[i], PointerIndexInArrayToThePoll[i], NodeInformation.ID);
                }

                NodeInformation.SystemAccess.AddTicketsToThePoll(structUnsubscribeTickets);
            }
        }

        /// <summary>
        /// Если вовремя отписки, место положения нашего тикета изменилось, то нужно продублировать регистрацию, но уже в новое место.
        /// </summary>
        private void RepeetUnsubscribe(int pIndexTicketInSubscribeManager)
        {
            poll.data.ticket.Struct structUnsubscribeTicket = new poll.data.ticket.Struct
                                        (poll.data.ticket.Struct.Type.UNSUBSCRIBE, pIndexTicketInSubscribeManager, PointerToThePollID[pIndexTicketInSubscribeManager],
                                            PointerIndexInArrayToThePoll[pIndexTicketInSubscribeManager], NodeInformation.ID);

            NodeInformation.SystemAccess.AddTicketsToThePoll(new poll.data.ticket.Struct[] { structUnsubscribeTicket });
        }

        public void Add(global::System.Action pAction, int pSize, int pTimeDelay, string pName)
        {
            lock(Locker)
            {
                if (State == StateType.CREATING_TICKET)
                {
                    if (StateInformation.IsCreating || StateInformation.IsOccurrence)
                    {
                        if (HeaderInformation.IsNodeObject())
                        {
                            // Если это первый билет, значит нужно подписать
                            // регистрационый метод в SubscribeControlManager.
                            if (StructSubscribeTickets.Length == 0)
                            {
                                Subscription.AddSubscribeAndUnsubscribe(control.SubscriptionType.Poll, Subscribe, Unsubscribe);
                            }

                            if (pName == "") pName = HeaderInformation.Explorer + Hellper.GetName(pAction.Method);

                            poll.data.ticket.Struct[] structSubscribeTickets
                                = new poll.data.ticket.Struct[StructSubscribeTickets.Length + 1];

                            //Создаем новый билет.
                            poll.data.ticket.Struct newSubsribeTicket =
                                    new poll.data.ticket.Struct(poll.data.ticket.Struct.Type.SUBSCRIBE, pAction,
                                        pSize, (int)pTimeDelay, pName, ToInforming, NodeInformation.ID,
                                            structSubscribeTickets.Length - 1);

                            // Если имя совпадает с уже добавленым тикетом, то мы записываем следом за ним текущий тикет и
                            // выставляем шаг в единицу.
                            int step = 0;
                            for (int i = 0; i < StructSubscribeTickets.Length; i++)
                            {
                                // Начинаем пересобирать массив.
                                structSubscribeTickets[i + step] = StructSubscribeTickets[i];

                                // Если шаг равен нулю, значит совподений еще небыло.
                                if (step == 0)
                                {
                                    // Если имя совподает с новым билетом, то запишим его следом.
                                    if (structSubscribeTickets[i + step].Name == newSubsribeTicket.Name)
                                    {
                                        // Было найдено совподение по имени, запишим новый билет следом за ним.
                                        // Увеличим шаг на один что бы последующий билет на записался на это место.
                                        structSubscribeTickets[i + (++step)] = newSubsribeTicket;
                                    }
                                }
                            }

                            // И если небыло совподений, то запишем в конец.
                            if (step == 0)
                            {
                                structSubscribeTickets[structSubscribeTickets.Length - 1] = newSubsribeTicket;
                            }

                            //После чего перезаписываем массив билетов.
                            StructSubscribeTickets = structSubscribeTickets;
                        }
                        else
                        {
                            // Регистрируем пулы через node object.
                            ((poll.description.access.add.IPoll)
                                NodeInformation.NodeObject).Add(pAction, pSize, pTimeDelay, pName);
                        }
                    }
                    else
                        Exception(Ex.SubscribeToPoll.x10004, information.State.Data.CREATING, information.State.Data.OCCURRENCE, StateInformation.Get());
                }
                else
                    Exception(Ex.SubscribeToPoll.x10003, StateType.CREATING_TICKET, State);
            }
        }          
    }
}
