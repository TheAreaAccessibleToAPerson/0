namespace Butterfly.system.objects.poll.objects.poll.data.setting
{
    public struct Struct
    {
        /// <summary>
        /// Уникальный ID пула.
        /// </summary>~
        public ulong PollID;
        /// <summary>
        /// Максимальный размер пула.
        /// </summary>
        public int Size;
        /// <summary>
        /// Временой интервал повторения.
        /// </summary>
        public int TimeDelay;
        /// <summary>
        /// Имя пула, если оно не указано именем становится Explorer обьекта.
        /// </summary>
        public string Name;

        /// <summary>
        /// Для распределения ресурсов.
        /// </summary>
        public global::System.Func<Object, SYSTEM.objects.handler.polls.TurnToPollsHandlerResult> LinkAndDistribution;

        /// <summary>
        /// Для удаления.
        /// </summary>
        public global::System.Func<Object, bool> DestroyPoll;

        public Struct(global::System.Func<Object, SYSTEM.objects.handler.polls.TurnToPollsHandlerResult> pLinkAndDistribution, 
            global::System.Func<Object, bool> pRemovePollInList, ulong pUniqueID, string pName, int pSize, int pTimeDelay)
        {
            LinkAndDistribution = pLinkAndDistribution;
            DestroyPoll = pRemovePollInList;

            PollID = pUniqueID;
            Name = pName;
            Size = pSize;
            TimeDelay = pTimeDelay;
        }
    }
}
