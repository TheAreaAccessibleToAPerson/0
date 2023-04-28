namespace Butterfly.system.objects.poll.data.ticket
{
    /// <summary>
    /// Хранит билет на подписку и отписку из пуллов.
    /// 
    /// Когда мы регистрируемся в пуллы, а точнее подаем заявку на регистрацию!!!!!....
    /// Они не сразу начинают работать, при подписки это не страшно,  но при отписки
    /// нам нужно получить все данные из хранилищь которые обслуживают данные пуллы, до того как обьект закончит свою работу.
    /// 
    /// Для того что бы обьект знал место регистрации в поле InformingAction передается метод который релизует
    /// прием и записть информации о текущем месте нахождении. Пулы будут распределять нагрузку или соединяться и при каждом
    /// таком процессе они будут информировать обьект о перезде в другое место. Для того что бы после быстро отключатся от пулов
    /// и проинформировать подписчика о конце регистрации.
    /// Последний отключаемый Action поставит выставит пулл на уничтжение.
    /// </summary>
    public struct Struct
    {
        public struct Type
        {
            public const string SUBSCRIBE = "Subscribe";
            public const string UNSUBSCRIBE = "Unsubscribe";
        }

        /// <summary>
        /// Хранит тип билета:
        /// 1)Подписка. 
        /// 2)Отписка.
        /// </summary>
        public string TicketType;

        /// <summary>
        /// Номер ID пулла, указывается если тип билета Unsubscribe.
        /// </summary>
        public ulong PollID;

        /// <summary>
        /// Номер в массиве где работает наш Action.
        /// </summary>
        public int IndexArrayInPoll;





        /// <summary>
        /// Подписываемый метод.
        /// В случае подписки это поле хранит метод который нужно зарегестрировать в пуллы.
        /// В случае отписки это поле хранит метод который сообщит обькту узлу о том, что 
        /// </summary>
        public global::System.Action Action;

        /// <summary>
        /// Размер пула.
        /// </summary>
        public int Size;

        /// <summary>
        /// Тайм аут.
        /// </summary>
        public int TimeDelay;

        /// <summary>
        /// Имя пула куда нужно подписаться.
        /// </summary>
        public string Name;

        /// <summary>
        /// Используется для информирования о том в каком пуле происходит работа.
        /// Первый параметр это порядковый номер регистрируемого Action,
        /// втором это номер пула. Если приходит номер пула равный нулю, значит 
        /// мы отплючили Action от пула.
        /// </summary>
        public global::System.Action<objects.poll.InformingType, int, ulong, int> ToInforming;

        /// <summary>
        /// Уникальный ключ обьекта подписываемого в пуллы.
        /// </summary>
        public ulong IDObject;

        /// <summary>
        /// При создании происходит нумерация тикетов по порядку.
        /// </summary>
        public int IndexTicket;

        public Struct(string pTicketType, global::System.Action pAction, int pSize, int pTimeDelay, string pName,
            global::System.Action<objects.poll.InformingType, int, ulong, int> pInforming, ulong pUniqueKeyObject, int pIndexPoll)
        {
            TicketType = pTicketType;

            Action = pAction;
            Size = pSize;
            TimeDelay = pTimeDelay;
            Name = pName;

            IDObject = pUniqueKeyObject;
            IndexTicket = pIndexPoll;

            ToInforming = pInforming;



            PollID = 0;
            IndexArrayInPoll = 0;
        }

        public Struct(string pTicketType, int pIndexTicket, ulong pIDPoll, int pIndexArrayInPolls,  ulong pIDObject)
        {
            TicketType = pTicketType;
            PollID = pIDPoll;
            IDObject = pIDObject;
            IndexArrayInPoll = pIndexArrayInPolls;
            IndexTicket = pIndexTicket;


            Action = null;
            Size = 0;
            TimeDelay = 0;
            Name = null;
            
            
            ToInforming = null;
        }
    }
}
