namespace Butterfly.system.objects.main.manager.handlers
{
    /// <summary>
    /// Дестрой по умолчанию true.
    /// </summary>
    public class Private : Informing
    {
        /// <summary>
        /// Приватный ключ для всех PrivateHandler.
        /// </summary>
        public const string KEY = "P123r123i123v123a123t123e123K123e123y";

        private Object[] Values;

        /// <summary>
        /// Данное значение хранит количесво добавленых приватных обработчиков.
        /// При запуке остановки приватных обработчиков после каждого остановленого
        /// обработчика значение будет дикрементироватся и когда дастигнет 0, попробует остановить 
        /// обьект в нутри которого находится текущий менеджер.
        /// </summary>
        public int Count { private set; get; }


        private readonly information.Header HeaderInformation;
        private readonly information.State StateInformation;
        private readonly information.Node NodeInformation;

        private readonly objects.Global GlobalObjectsManager;

        private readonly activity.Life LifeActivity;

        /// <summary>
        /// Текущий менеджер приватных обработчиков запущен на уничтожение.
        /// </summary>
        private bool IsStartingToStopping = false;

        /// <summary>
        /// При первом обращении значение IsStartingToStopping выставится в true и вернет true
        /// что позволит запустить отсановку. При повторном обращении мы получим значение false
        /// которое будет означать что менеджер приватных обработчов уже поставлен на уничтожение.
        /// </summary>
        public bool IsStopping { 
            get 
            {
                if (IsStartingToStopping == false)
                {
                    IsStartingToStopping = true;

                    return true;
                }

                return false;
            } 
        }

        public Private(IInforming pInforming, information.Header pHeaderInformation, information.State pStateInformation, information.Node pNodeInformation,
            objects.Global pGlobalObjects) 
            : base("PrivateHandlersManager", pInforming)
        {
            Values  = new Object[0];

            LifeActivity = new activity.Life(pInforming, activity.Life.Data.PRIVATE_HANDLERS_MANAGER);

            StateInformation = pStateInformation;
            NodeInformation = pNodeInformation;
            GlobalObjectsManager = pGlobalObjects;
            HeaderInformation = pHeaderInformation;
        }

        /// <summary>
        /// Через данных метод приватный обработчик сообщает о том что он полностью остановил работу.
        /// </summary>
        public void Remove()
        {
            if (--Count < 0)
            {
                Exception("Вы удалили лишний приватный хендлер, скорее всего сообщение где то было продублировано.");
            }

            if (Count == 0)
            {
                ((main.activity.description.ILife)NodeInformation.CurrentMainObject).ContinueStop();
            }
        }

        public global::System.Tuple<string, bool>[] LifeCyrcle(string pStateLife)
        {
            return LifeActivity.Process(pStateLife, Values);
        }

        private bool InstantiatePrivateHandler<HandlerType>(HandlerType pHandler, out HandlerType oHandler) where HandlerType : Object, new()
        {
            lock(StateInformation.Locker)
            {
                oHandler = default;

                bool result = true;
                {
                    if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
                    {
                        if (typeof(HandlerType).FullName == HeaderInformation.SystemType.FullName)
                        {
                            Exception(Ex.Handler.x10003, typeof(HandlerType).FullName);

                            return false;
                        }

                        ((main.description.definition.INode)pHandler).BranchDefine(NodeInformation.NodeObject,
                            NodeInformation.CurrentMainObject, NodeInformation.NearNodeIndependentObject,
                            NodeInformation.SystemAccess, GlobalObjectsManager.Get(), KEY, NodeInformation.NodeID,
                            NodeInformation.AttackmentNumberObjectInNode + 1,
                            NodeInformation.AttachmentNodeNumberInSystem,
                            Hellper.ExpendArray(NodeInformation.IDNodeParents, NodeInformation.NodeObject.GetID()));

                        if (pHandler.HeaderInformation.IsBranchObject())
                        {
                            Object[] values = new Object[Values.Length + 1];
                            for (int i = 0; i < Values.Length; i++) values[i] = Values[i];
                            values[Values.Length] = pHandler;

                            Count++;

                            Values = values;

                            oHandler = pHandler;
                        }
                        else
                        {
                            Exception(Ex.MainObject.x10013, typeof(HandlerType).FullName);
                            return false;
                        }
                    }
                    else
                    {
                        Exception(Ex.MainObject.x10015);
                        return false;
                    }

                }

                return result;
            }
        }

        public PrivateHandlerType Add<PrivateHandlerType>() where PrivateHandlerType : Object, handler.description.IRestream, new()
        {
            if (InstantiatePrivateHandler(new PrivateHandlerType(), out PrivateHandlerType privateHandlerReduse))
            {
                return privateHandlerReduse;
            }
            else
                Exception(Ex.MainObject.x10017, typeof(PrivateHandlerType).FullName);

            return default;
        }

        public PrivateHandlerType Add<PrivateHandlerType>(global::System.Action<int> pContinueExecutingEvents, int pNumberOfTheInterruptedEvent) 
            where PrivateHandlerType : Object, handler.description.IRestream, handler.description.IContinueInterrupting, new()
        {
            if (InstantiatePrivateHandler(new PrivateHandlerType(), out PrivateHandlerType privateHandlerReduse))
            {
                Console("!!!!!!");
                privateHandlerReduse.Set(pContinueExecutingEvents, pNumberOfTheInterruptedEvent);

                return privateHandlerReduse;
            }
            else
                Exception(Ex.MainObject.x10017, typeof(PrivateHandlerType).FullName);

            return default;
        }

        public PrivateHandlerType Add<PrivateHandlerType>(PrivateHandlerType pPrivateHandler) where PrivateHandlerType : Object, new()
        {
            if (InstantiatePrivateHandler(pPrivateHandler, out PrivateHandlerType privateHandlerReduse))
            {
                return privateHandlerReduse;
            }
            else
                Exception(Ex.MainObject.x10017, typeof(PrivateHandlerType).FullName);

            return default;
        }
    }
}
