namespace Butterfly.system.objects.SYSTEM.objects.handler.polls
{
    /// <summary>
    /// Выполняем работу с пуллами. Регистрируем подписку и отписку.
    /// </summary>
    public sealed class PollsHandler : Handler<poll.data.ticket.Struct[]>
    {
        protected override void Construction()
        {
            input_to(Process); // Принимаем билеты.
        }
 

        /// <summary>
        /// Локер для работы с пулами.
        /// </summary>
        private readonly object Locker = new object();

        /// <summary>
        /// Уникальное значение для нового пула.
        /// </summary>
        private ulong UniqueIDPoll = 1;

        /// <summary>
        /// Храним ссылки на пуллы сдесь что бы не производить преобразования.
        /// </summary>
        private readonly System.Collections.Generic.List<poll.objects.poll.Object> PollObjects 
            = new System.Collections.Generic.List<poll.objects.poll.Object>();

        private void Process(poll.data.ticket.Struct[] pTickets)
        {
            if (pTickets.Length == 0) return;

            lock (Locker)
            {
                // Все билеты приходят в составе одной одинакового типа. Либо Subscribe, либо Unsubscribe.
                if (pTickets[0].TicketType == poll.data.ticket.Struct.Type.SUBSCRIBE)
                {
                    Subscribe(pTickets);
                }
                else if (pTickets[0].TicketType == poll.data.ticket.Struct.Type.UNSUBSCRIBE)
                {
                    Unsubscribe(pTickets);
                }
            }
        }

        private void Unsubscribe(poll.data.ticket.Struct[] pTickets)
        {
            int lastPollIndex = 0;
            for (int i = 0; i < pTickets.Length; i++)
            {
                for (int u = lastPollIndex; u < PollObjects.Count; u++)
                {
                    if (pTickets[i].PollID == PollObjects[u].PollID)
                    {
                        PollObjects[u].Unsubscribe(pTickets[i]);
  
                        if ((i + 1) < pTickets.Length)
                        {
                            if (pTickets[i + 1].PollID == pTickets[i].PollID)
                            {
                                lastPollIndex = u;
                            }
                            else
                                lastPollIndex = 0;
                        }
                        else
                            return;
                    }
                }
            }
        }

        private void Subscribe(poll.data.ticket.Struct[] pTickets)
        {
            int lastPollIndex = 0;

            for (int i = 0; i < pTickets.Length; i++)
            {
                bool isSubscribe = false;

                int u = 0;
                for (u = lastPollIndex; u < PollObjects.Count; u++)
                {
                    if (PollObjects[u].Name == pTickets[i].Name)
                    {
                        if (PollObjects[u].TrySubscribe(pTickets[i]))
                        {
                            isSubscribe = true;

                            if ((i + 1) < pTickets.Length)
                            {
                                if (pTickets[i + 1].Name == pTickets[i].Name)
                                {
                                    lastPollIndex = u;
                                }
                                else
                                    lastPollIndex = 0;
                            }
                            else
                            {
                                return;
                            }

                            break;
                        }
                        else
                        {
                            isSubscribe = false;
                        }
                    }
                }

                if (isSubscribe == false)
                {
                    string pollName = CreatingPoll(pTickets[i]).Name;

                    if ((i + 1) < pTickets.Length)
                    {
                        if (pTickets[i + 1].Name == pollName)
                        {
                            lastPollIndex = u;
                        }
                        else
                            lastPollIndex = 0;
                    }
                    else
                        return;
                }
            }
        }


        private poll.objects.poll.Object CreatingPoll(poll.data.ticket.Struct pTicket)
        {
            // Создаем пулл.
            poll.objects.poll.Object pollObject = create_object<poll.objects.poll.Object>
                (UniqueIDPoll.ToString(), new poll.objects.poll.data.setting.Struct(LinkOrDistribution, Remove,
                    UniqueIDPoll, pTicket.Name, pTicket.Size,
                        pTicket.TimeDelay));

            UniqueIDPoll++;

            PollObjects.Add(pollObject);

            // Выставляем на регестрацию.
            pollObject.TrySubscribe(pTicket);

            return pollObject;
        }

        /// <summary>
        /// Если пулл пустой, то мы сначало удалим его из листа где хранится его ссылка,
        /// а после зарегестрируем обьект на уничтожение.
        /// </summary>
        /// <param name="pObjectPoll"></param>
        private bool Remove(poll.objects.poll.Object pObjectPoll)
        {
            if (System.Threading.Monitor.TryEnter(Locker))
            {
                PollObjects.Remove(pObjectPoll);

                pObjectPoll.destroy();

                //Console($"Текущее количесво пулов {PollObjects.Count} !!!!!!!!!!!!!!!!!!!!!!!!!!");

                System.Threading.Monitor.Exit(Locker);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Совмещение пуллов или равномерное распределение.
        /// Если locker занят, то вернем false и позже повторим попытку,
        /// что бы пуллы не простаивали.
        /// </summary>
        /// <returns></returns>
        private TurnToPollsHandlerResult LinkOrDistribution(poll.objects.poll.Object pObjectPoll)
        {
            // Запишим все собраные данные...
            int[] workloadComporablePolls = new int[0];
            // Запишим индексы пулов.
            int[] indexComporablePolls = new int[0];
            // ... попутно получив общую сумму.
            int totalSumWorkloadComporablePolls = 0;

            // Проверка будет происходить из разных потоков, поэтому обезатьно через проверку на null.
            for (int i = 0; i < PollObjects.Count; i++)
            {
                if (pObjectPoll.Name == PollObjects[i]?.Name)
                {
                    workloadComporablePolls = Hellper.ExpendArray(workloadComporablePolls, (int)PollObjects[i]?.ClientsManager.Count);
                    indexComporablePolls = Hellper.ExpendArray(indexComporablePolls, i);
                    totalSumWorkloadComporablePolls += (int)PollObjects[i]?.ClientsManager.Count;
                }
            }

            // Если небыло найдено не одного схожего пулла...
            if (workloadComporablePolls.Length == 0) return TurnToPollsHandlerResult.NotLinkOrDistribution;

            global::System.Array.Sort(workloadComporablePolls, (x, y) => y.CompareTo(x));

            // Сравним общую сумму с максимально возможным количесвом клинтов в текущем пулле.
            if (totalSumWorkloadComporablePolls <= pObjectPoll.ClientsManager.Size)
            {
                // Количесво клинтов во всех схожих пулах можно попытатся поместить в один.
            }
            else
            {
                // Собраное общее количесво клинтов во всех схожих пулах превышает максимально допустимое
                // значение для одного пула.

                // Узнаем сколько всего необходимо пулов для содержания всех клинтов.
                int countNeedPoll = totalSumWorkloadComporablePolls / pObjectPoll.ClientsManager.Size;

                // Если для обслуживания клинтов нужно меньшее количесво пулов, то...
                if (countNeedPoll < workloadComporablePolls.Length)
                {
                    // Получаем неоходимое количесво пулов из имеющихся.
                }
                else
                {
                    // Иначе просто распределим ресурсы.
                }
            }

            // Проверяем можем ли мы совместить пулы.
            foreach (int workloadPoll in workloadComporablePolls)
            {
                
            }

            if (System.Threading.Monitor.TryEnter(Locker))
            {
                //...

                System.Threading.Monitor.Exit(Locker);

                return TurnToPollsHandlerResult.LinkOrDistribution;
            }
            else
                return TurnToPollsHandlerResult.NeedLinkOrDistribution;
        }
    }

    /// <summary>
    /// Используется в методе LinkAndDistibution для возращения результата его работы.
    /// </summary>
    public enum TurnToPollsHandlerResult
    {
        /// <summary>
        /// Необходимо произвести работу по распределению ресурсов текущего пулла.
        /// Это небыло сделано по причине неудачного захвата Locker в PollsHandler.
        /// Необходимо повторное обращение для попытки захвата Locker.
        /// </summary>
        NeedLinkOrDistribution = 1,

        /// <summary>
        /// Нет необходимости в распределении ресурсов. Нужно сбросить счетчик.
        /// </summary>
        NotLinkOrDistribution = 2,

        /// <summary>
        /// Было произведено распределение ресурсов.
        /// </summary>
        LinkOrDistribution = 3
    }
}
