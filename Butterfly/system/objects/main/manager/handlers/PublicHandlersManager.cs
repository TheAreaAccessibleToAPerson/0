namespace Butterfly.system.objects.main.manager.handlers
{
    public class Public : Informing
    {
        // Если обьекты создаются только в методе Start(), то мы записываем
        // их в массив.
        private string[] KeyValues;
        private Object[] Values;

        /// <summary>
        /// Данное значение хранит количесво добавленых публичных обработчиков.
        /// При запуке остановки публичных обработчиков после каждого остановленого
        /// обработчика значение будет дикрементироватся и когда дастигнет 0, попробует остановить 
        /// обьект в нутри которого находится текущий менеджер.
        /// </summary>
        public int Count { private set; get; }

        /// <summary>
        /// Текущий менеджер приватных обработчиков запущен на уничтожение.
        /// </summary>
        private bool IsStartingToStopping = false;

        /// <summary>
        /// При первом обращении значение IsStartingToStopping выставится в true и вернет true
        /// что позволит запустить отсановку. При повторном обращении мы получим значение false
        /// которое будет означать что менеджер публичных обработчов уже поставлен на уничтожение.
        /// </summary>
        public bool IsStopping
        {
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

        private readonly information.Header HeaderInformation;
        private readonly information.State StateInformation;
        private readonly information.Node NodeInformation;

        private readonly manager.objects.Global GlobalObjectsManager;

        private readonly activity.Life LifeActivity;

        public Public(IInforming pInforming, information.Header pHeaderInformation, information.State pStateInformation, 
            information.Node pNodeInformation, objects.Global pGlobalObjects) 
            : base("PublicHandlersManager", pInforming)
        {
            KeyValues = new string[0];
            Values = new Object[0];

            LifeActivity = new activity.Life(pInforming, activity.Life.Data.PUBLIC_HANDLERS_MANAGER);

            StateInformation = pStateInformation;
            NodeInformation = pNodeInformation;
            GlobalObjectsManager = pGlobalObjects;
            HeaderInformation = pHeaderInformation;
        }

        public void Remove()
        {
            if (--Count < 0)
            {
                Exception("Вы удалили лишний публичный хендлер, скорее всего сообщение где то было продублировано.");
            }

            if (Count == 0)
            {
                ((main.activity.description.ILife)NodeInformation.CurrentMainObject).ContinueStop();
            }
        }

        public System.Tuple<string, bool>[] LifeCyrcle(string pStateLife) 
        {
            return LifeActivity.Process(pStateLife, Values);
        }

        private bool InstantiatePublicHandler<HandlerType>(HandlerType pHandler, out HandlerType oReturnHandler, string pKey = "")
            where HandlerType : Object, new()
        {
            //lock(StateInformation.Locker)
            {
                oReturnHandler = default;

                bool result = false;
                {
                    if (StateInformation.__IsCreating)
                    {
                        if (typeof(HandlerType).FullName == HeaderInformation.SystemType.FullName)
                        {
                            Exception(Ex.Handler.x10003, typeof(HandlerType).FullName);

                            return false;
                        }

                        if (pKey == "") pKey = typeof(HandlerType).FullName;

                        for (int i = 0; i < KeyValues.Length; i++)
                        {
                            if (pKey == KeyValues[i])
                            {
                                oReturnHandler = (HandlerType)Values[i];

                                return result = true;
                            }
                        }

                        ((main.description.definition.INode)pHandler).BranchDefine(NodeInformation.NodeObject,
                            NodeInformation.CurrentMainObject, NodeInformation.NearNodeIndependentObject,
                            NodeInformation.SystemAccess, GlobalObjectsManager.Get(), pKey, NodeInformation.NodeID,
                            NodeInformation.AttackmentNumberObjectInNode + 1,
                            NodeInformation.AttachmentNodeNumberInSystem + 1,
                            Hellper.ExpendArray(NodeInformation.IDNodeParents, NodeInformation.NodeObject.GetID()));

                        if (pHandler.GetType().FullName != GetType().FullName)
                        {
                            if (pHandler.HeaderInformation.IsBranchObject())
                            {
                                string[] keyValues = new string[KeyValues.Length + 1];
                                Object[] objectValues = new Object[Values.Length + 1];

                                for (int i = 0; i < KeyValues.Length; i++) keyValues[i] = KeyValues[i];
                                for (int i = 0; i < Values.Length; i++) objectValues[i] = Values[i];

                                keyValues[KeyValues.Length] = pKey.ToString();
                                objectValues[Values.Length] = pHandler;

                                Count++;

                                KeyValues = keyValues;
                                Values = objectValues;

                                oReturnHandler = pHandler;

                                result = true;
                            }
                            else
                            {
                                Exception(Ex.MainObject.x10013, typeof(HandlerType).FullName);
                            }
                        }
                        else
                            Exception(Ex.MainObject.x10018, pHandler.GetType().FullName);
                    }
                    else
                        Exception(Ex.MainObject.x10015);
                }

                return result;
            }
        }

        public HandlerType TryAdd<HandlerType>(string pPublicHandlerName = "") where HandlerType : Object, new()
        {
            if (InstantiatePublicHandler(new HandlerType(), out HandlerType oHandlerReduse, pPublicHandlerName))
            {
                return oHandlerReduse;
            }
            else
                Exception(Ex.MainObject.x10016);

            return default;
        }

        public HandlerType TryAdd<HandlerType, InputValueType>(ref IInput<InputValueType> rInput, string pPublicHandlerName = "") where HandlerType : Object, new()
        {
            if (InstantiatePublicHandler(new HandlerType(), out HandlerType oHandlerReduse, pPublicHandlerName))
            {
                if (oHandlerReduse is IInput<InputValueType> handlerInputReduse)
                {
                    rInput = handlerInputReduse;
                }
                else
                    Exception(Ex.MainObject.x10028, typeof(IInput<InputValueType>).FullName, oHandlerReduse.GetType().FullName);

                return oHandlerReduse;
            }
            else
                Exception(Ex.MainObject.x10016);

            return default;
        }
    }
}
