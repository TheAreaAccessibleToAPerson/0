namespace Butterfly.system.objects.poll.objects.poll.manager
{
    public class Clients : main.Informing
    {
        /// <summary>
        /// ID Пулла.
        /// </summary>
        private readonly ulong PollID;

        /// <summary>
        /// Массив для хранения выполняющиемя System.Action.
        /// </summary>
        public readonly global::System.Action[] Runs;

        /// <summary>
        /// Информирование клиента:
        /// 1)Тип информирования 
        /// 2) Индекс хранения билета в SubsribePollManager, 
        /// 3) ID пула, 
        /// 4)Индекс хранения клинта в SubscribeClient(this).
        /// </summary>
        public readonly global::System.Action<InformingType, int, ulong, int>[] ToInformings;
        /// <summary>
        /// Уникальные ID клинтов которые используют данный пулл.
        /// </summary>
        public readonly ulong[] IDClients;
        /// <summary>
        /// Номер индекса в массиве PollSubscribeManger.
        /// </summary>
        public readonly int[] ClientsPointerIndexInPollSubscribeManager;

        /// <summary>
        /// Последний индекс обрабатываемого Action.
        /// </summary>
        private int IndexToEmptySlot  = 0; // Количесво обрабатываемых System.Action.

        /// <summary>
        /// Количесво клинтов.
        /// </summary>
        public int Count { get { return IndexToEmptySlot; } }

        /// <summary>
        /// Максимальное количесво клинтов.
        /// </summary>
        public readonly int Size;

        /// <summary>
        /// Имя пулла.
        /// </summary>
        private readonly string Name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPollID">ID пулла который использует данный класс.</param>
        /// <param name="pClientCount">Максимальное количесво клинтов.</param>
        public Clients(main.IInforming pInforming, string pName, ulong pPollID, int pClientCount)
            : base($"Polls[{pName}]/ClientManager", pInforming)
        {
            PollID = pPollID;
            Size = pClientCount;
            Name = pName;

            Runs = new System.Action[pClientCount];
            IDClients = new ulong[pClientCount];
            ToInformings = new System.Action<InformingType, int, ulong, int>[pClientCount];
            ClientsPointerIndexInPollSubscribeManager = new int[pClientCount];
        }

        public void ActionRun()
        {
            for (int i = 0; i < IndexToEmptySlot; i++)
            {
                Runs[i].Invoke();
            }
        }

        public void Subscribe(system.objects.poll.data.ticket.Struct[] pTickets, int pStartIndex = 0, int pEndIndex = 0)
        {
            if (pEndIndex == 0) pEndIndex = pTickets.Length - 1;

            for (int index = pStartIndex; index <= pEndIndex; index++)
            {
                AddClient(IndexToEmptySlot++, pTickets[index].IDObject, pTickets[index].IndexTicket, pTickets[index].Action, pTickets[index].ToInforming);
            }
        }

        public void Subscribe(system.objects.poll.data.ticket.Struct pTickets, int pStartIndex = 0, int pEndIndex = 0)
        {
            AddClient(IndexToEmptySlot++, pTickets.IDObject, pTickets.IndexTicket, pTickets.Action, pTickets.ToInforming);
        }

        /// <summary>
        /// Если есть желающие подписаться, то тоже примим их и поставим на место отписавшихся.
        /// </summary>
        /// <param name="pUnsubscribeTickets"></param>
        /// <param name="pSubscribeTickets"></param>
        public void Unsubscribe(system.objects.poll.data.ticket.Struct[] pUnsubscribeTickets,
            system.objects.poll.data.ticket.Struct[] pSubscribeTickets = null)
        {
            foreach (var ticket in pUnsubscribeTickets)
            {
                Unsubscribe(ticket);
            }
        }

        public void Unsubscribe(system.objects.poll.data.ticket.Struct ticket)
        {
            // Проверим не переехал ли клиент в другой пулл.
            if (PollID == ticket.PollID)
            {
                // Проверяем не переместили ли клинта в другой слот.
                if (IDClients[ticket.IndexArrayInPoll] == ticket.IDObject)
                {
                    // Один и тот же клиент может несколько раз подписаться в один и тот же пулл.
                    // проверяем номер билета на совподение.
                    if (ClientsPointerIndexInPollSubscribeManager[ticket.IndexArrayInPoll] == ticket.IndexTicket)
                    {
                        SystemInformation($"Клиент c id {IDClients[ticket.IndexArrayInPoll]} был отписан из " +
                        $"пулла {PollID} c позиции {ticket.IndexArrayInPoll}");

                        // Проинформирем клинта что он отключон.
                        ToInformings[ticket.IndexArrayInPoll].Invoke(InformingType.EndUnsubscribe,
                            ClientsPointerIndexInPollSubscribeManager[ticket.IndexArrayInPoll],
                            PollID, ticket.IndexArrayInPoll);

                        // Очистим данные о клинте.
                        Runs[ticket.IndexArrayInPoll] = null;
                        ToInformings[ticket.IndexArrayInPoll] = null;
                        IDClients[ticket.IndexArrayInPoll] = ulong.MaxValue;
                        ClientsPointerIndexInPollSubscribeManager[ticket.IndexArrayInPoll] = -1;

                        // Если у нас был всего один подписчик, то выйдем.
                        if ((IndexToEmptySlot - 1) < 0)
                        {
                            return;
                        }

                        // Если освобожденый слот и был крайним. То инкрементируем 
                        // край массива с данными клиетов, и перейдем к обработки следующего билета.
                        if ((IndexToEmptySlot - 1) == ticket.IndexArrayInPoll)
                        {
                            IndexToEmptySlot--;
                            return;
                        }

                        // Если индекс отписаного клинта выше чем крайний клиент,
                        // то перейдем к следующему билету.
                        if ((IndexToEmptySlot - 1) < ticket.IndexArrayInPoll)
                        {
                            return;
                        }

                        // Декриментируем крайнее значение. Так как крайний клиент переедит 
                        // в освободившийся слот.
                        IndexToEmptySlot--;

                        SystemInformation($"Клиент {IDClients[IndexToEmptySlot]}[index in pollManager:{ClientsPointerIndexInPollSubscribeManager[IndexToEmptySlot]}] " +
                        $"переместился с позиции {IndexToEmptySlot} в {ticket.IndexArrayInPoll}");

                        // Перенесем крайнего клинта в освободившийся слот.
                        Runs[ticket.IndexArrayInPoll] = Runs[IndexToEmptySlot];
                        ToInformings[ticket.IndexArrayInPoll] = ToInformings[IndexToEmptySlot];
                        IDClients[ticket.IndexArrayInPoll] = IDClients[IndexToEmptySlot];
                        ClientsPointerIndexInPollSubscribeManager[ticket.IndexArrayInPoll] = ClientsPointerIndexInPollSubscribeManager[IndexToEmptySlot];

                        ToInformings[ticket.IndexArrayInPoll].Invoke(InformingType.ChangeOfIndex,
                                ClientsPointerIndexInPollSubscribeManager[ticket.IndexArrayInPoll], PollID, ticket.IndexArrayInPoll);

                        SystemInformation($"Позиция {IndexToEmptySlot} была освобождена.");
                    }
                    else
                    {
                        // Клиент уже в курсе что его текущее место в данном пулле было изменено.
                        // Он вышлет повторный запрос.
                    }
                }
                else
                {
                    // Клиент уже в курсе что его текущее место в данном пулле было изменено.
                    // Он вышлет повторный запрос.
                }
            }
            else
            {
                // Клиент уже уведомлен о том что его переместили
                // в другой пулл и вышлет повторное сообщение.
            }
        }

        /// <summary>
        /// Добавляем нового клинта и информируем его об этом передовая все необходимые данные.
        /// </summary>
        /// <param name="pIndexArray">Номер индекса в массиве где будет хранится текущий клиент в SubscribeClient(this).</param>
        /// <param name="pClientID">ID подписываемого клинта.</param>
        /// <param name="pPointerIndexToPollSubscribeManager">Индекс хранения билета в SubsribePollManager</param>
        /// <param name="pRun">Метод который должен обрабатывать пулл.</param>
        /// <param name="pInforming">Информирование клиента: 
        /// 1)Тип информирования 2) Индекс хранения билета в SubsribePollManager, 3) ID пула, 4)Индекс хранения клинта в SubscribeClient(this).</param>
        private void AddClient(int pIndexArray, ulong pClientID, int pPointerIndexToPollSubscribeManager, global::System.Action pRun,
            global::System.Action<InformingType, int, ulong, int> pInforming)
        {
            Runs[pIndexArray] = pRun;
            ToInformings[pIndexArray] = pInforming;
            IDClients[pIndexArray] = pClientID;
            ClientsPointerIndexInPollSubscribeManager[pIndexArray] = pPointerIndexToPollSubscribeManager;

            SystemInformation($"Обьект [ID:{pClientID}, index:{pPointerIndexToPollSubscribeManager} подписался на пулл [ID:{PollID}, index:{pIndexArray}]" +
                "  и далее проинформируем об этом подписчика.");

            SystemInformation("Текущий размер пулла " + PollID + " стал:" + IndexToEmptySlot + "/" + Size);

            pInforming.Invoke(InformingType.EndSubscribe, pPointerIndexToPollSubscribeManager, PollID, pIndexArray);

            SystemInformation("Обьект с id: " + pClientID + " проинформирован об подписании.");
        }
    }
}
