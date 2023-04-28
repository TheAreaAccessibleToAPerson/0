namespace Butterfly.system.objects.main.manager.objects
{
    public class Global : Informing
    {
        private readonly IInforming Informing;

        private readonly information.Header HeaderInformation;
        private readonly information.State StateInformation;
        private readonly information.Node NodeInformation;

        // Хранит ссылку на все глобальные обьекты.
        private global::System.Collections.Generic.Dictionary<string, object> Values;

        // Хранит ключи на все глобальные обьекты созданые в нутри данного обьекта.
        private string[] CreatingInCurrentObjectGlobalObjectsKey = new string[0];

        /// <summary>
        /// Количесво созданых в текущем обьекте глобальных обьектов.
        /// </summary>
        public int CreatingInCurrentObjectGlobalObjectsCount { get { return CreatingInCurrentObjectGlobalObjectsKey.Length; } }

        private readonly information.description.access.get.INode NodeAccess;
        private readonly handlers.description.access.add.IPrivate PrivateHandlerManager;
        private readonly description.access.get.IShared SharedObjectsManager;
        private readonly poll.description.access.add.IPoll PollAccess;
        private readonly main.description.access.add.IDependency Dependency;

        // Echo. При отправке Echo запроса. Обьект проконтралирует что бы ему пришол ответ.
        private int EchoRequestCount = 0; // Количесво отправленых Echo запросов.


        public Global(information.State pStateInformation, information.Header pHeaderInfomation, information.Node pNodeInformation, 
            IInforming pInforming, System.Collections.Generic.Dictionary<string, object> pGlobalObjects,
            information.description.access.get.INode pNodeAccess,
            handlers.description.access.add.IPrivate pPrivateHandlerManager,
            description.access.get.IShared pSharedObjectsManager,
            poll.description.access.add.IPoll pPoll,
            main.description.access.add.IDependency pDependency)
            : base("GlobalObjectsManager", pInforming)
        {
            Informing = pInforming;

            HeaderInformation = pHeaderInfomation;
            StateInformation = pStateInformation;   
            NodeInformation = pNodeInformation;

            NodeAccess = pNodeAccess;
            PrivateHandlerManager = pPrivateHandlerManager;
            SharedObjectsManager = pSharedObjectsManager;
            PollAccess = pPoll;
            Dependency = pDependency;

            Values = pGlobalObjects;
        }

        /// <summary>
        /// Получить ссылку на Dictionary глобальных обьектов.
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.Dictionary<string, object> Get()
        {
            return Values;
        }

        public bool TryGetValue(string pKey, out object oValueObject)
        {
            oValueObject = null;

            if (Values.TryGetValue(pKey, out object oObject))
            {
                oValueObject = oObject;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Получает ссылка на все глобальные обькты родительских обьектов.
        /// </summary>
        /// <param name="pValues"></param>
        public void Set(global::System.Collections.Generic.Dictionary<string, object> pValues)
        {
            Values = pValues;
        }

        /// <summary>
        /// Удалить созданые в текущем обьекте глобальные обьекты.
        /// </summary>
        public void RemoveCreatingGlobalObjectsInCurrentObject()
        {
            foreach(string pKey in CreatingInCurrentObjectGlobalObjectsKey)
            {
                if (Values.Remove(pKey, out object oGlobalObject))
                {
                    SystemInformation($"Глобальный обьект {pKey} был удален.");
                }
                else
                {
                    if (oGlobalObject is main.objects.description.access.get.IInformationCreatingObject globalObjectCreatingInformationReduse)
                    {
                        Exception(Ex.GlobalObjectsManager.x10017, oGlobalObject.GetType().ToString(), pKey, 
                            globalObjectCreatingInformationReduse.GetExplorerObject());
                    }
                    else
                        Exception(Ex.GlobalObjectsManager.x10018, oGlobalObject.GetType().ToString(), pKey, 
                            typeof(main.objects.description.access.get.IInformationCreatingObject).ToString());
                }
            }
        }

        public IInput<SendValueType> CreatingSendMessage<SendValueType>(string pListenerMessageName)
        {
            if (StateInformation.IsCreating)
            {
                if (Values.TryGetValue(pListenerMessageName, out object oObject))
                {
                    if (oObject is main.objects.description.access.get.IInformationCreatingObject objectInformationReduse)
                    {
                        bool isParent = false;
                        foreach (ulong id in NodeInformation.IDNodeParents)
                            if (id == objectInformationReduse.GetIDNodeObject())
                            {
                                isParent = true;

                                break;
                            }

                        if (isParent)
                        {
                            if (oObject is main.objects.listen.Object<SendValueType> listenMessageObjectReduse)
                            {
                                if (listenMessageObjectReduse is IInput<SendValueType>)
                                {
                                    return listenMessageObjectReduse;
                                }
                                else
                                    Exception(Ex.GlobalObjectsManager.x10024, pListenerMessageName, typeof(SendValueType).FullName,
                                        listenMessageObjectReduse.ListenMessageType);
                            }
                            else
                                Exception(Ex.GlobalObjectsManager.x10023, pListenerMessageName);
                        }
                        else
                            Exception(Ex.GlobalObjectsManager.x10022, pListenerMessageName, objectInformationReduse.GetExplorerObject());
                    }
                    else
                        Exception(Ex.GlobalObjectsManager.x10026, oObject.GetType().FullName);
                }
                else
                    Exception(Ex.GlobalObjectsManager.x10025, pListenerMessageName, typeof(SendValueType).FullName);
            }
            else
                Exception(Ex.GlobalObjectsManager.x10020, typeof(SendValueType).FullName, pListenerMessageName);

            return default;
        }

        /// <summary>
        /// Реализует создание/доступа к глобальным обьектам прослушивающих входящие данные.
        /// </summary>
        /// <typeparam name="GlobalObjectType"></typeparam>
        /// <param name="pKey"></param>
        public main.objects.listen.Object<ValueType> CreatingListenMessage<ValueType>(string pKey)
        {
            if (StateInformation.IsCreating)
            {
                if (Values.TryGetValue(pKey, out object oListenObject))
                {
                    if (oListenObject is main.objects.listen.Object<ValueType> listenObjectReduse)
                    {
                        return listenObjectReduse;
                    }
                    else
                    {
                        if (oListenObject is main.objects.description.access.get.IInformationCreatingObject directoryNameReduse)
                        {
                            Exception(Ex.GlobalObjectsManager.x10003, pKey, typeof(main.objects.listen.Object<ValueType>).FullName, 
                                directoryNameReduse.GetExplorerObject());
                        }
                    }
                }
                else
                {
                    main.objects.listen.Object<ValueType> listenInputValueObject = new main.objects.listen.Object<ValueType>
                        (HeaderInformation.Explorer, NodeInformation.NodeObject.GetID(), NodeInformation.ID, 
                            Informing, StateInformation, NodeAccess, PrivateHandlerManager, SharedObjectsManager, PollAccess, Dependency);

                    SystemInformation($"Creating GlobalListenMessage:{pKey}, MessageType:{typeof(ValueType).FullName}", System.ConsoleColor.Gray);

                    Values.Add(pKey, listenInputValueObject);

                    return listenInputValueObject;
                }
            }
            else
                Exception(Ex.GlobalObjectsManager.x10002, typeof(ValueType).FullName, information.State.Data.CREATING);

            return default;
        }

        public main.objects.description.IRestream<ReturnValueType> CreatingSendEcho<ReceiveValueType, ReturnValueType>(string pEchoName, 
            ref IInput<ReceiveValueType> rInput)
        {
            if (StateInformation.IsOccurrence || StateInformation.IsCreating)
            {
                // Получаем глобальный прослушиватель.
                if (TryGetValue(pEchoName, out object oReceiveGlobalObject))
                {
                    // Получаем его входной метод.
                    if (oReceiveGlobalObject is IInput<ReceiveValueType, IEchoReturn<ReturnValueType>> 
                            receiveEchoGlobalObjectReduse)
                    {
                         // Создаем echo для обратной связи.
                         main.objects.sending.echo.Object<ReceiveValueType, ReturnValueType> echoObject 
                            = new main.objects.sending.echo.Object<ReceiveValueType, ReturnValueType>
                            (Informing, NodeInformation.CurrentMainObject, NodeInformation.CurrentMainObject, StateInformation, NodeAccess,
                            PollAccess, NodeInformation.CurrentMainObject, receiveEchoGlobalObjectReduse.ToInput);

                        // Получаем входной метод echo,
                        // который будет перенаправлять данные в прослушивающий глобальный echo.
                        if (echoObject is IInput<ReceiveValueType> echoObjectInputReduse)
                        {
                            rInput = echoObjectInputReduse;
                        }

                        return echoObject;
                    }
                }
                else
                    Exception(Ex.ActionValue.x10006, pEchoName);
            }
            else
                Exception(Ex.GlobalObjectsManager.x10019, pEchoName);

            return default;
        }

        public main.objects.receive.echo.Object<ReceiveValueType, ReturnValueType> CreatingEcho<ReceiveValueType, ReturnValueType>(string pEchoName)
        {
            if (StateInformation.__IsCreating)
            {
                if (pEchoName != "")
                {
                    if (Values.ContainsKey(HeaderInformation.FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName))
                    {
                        Exception(Ex.GlobalObjectsManager.x10005, typeof(ReceiveValueType).FullName + ", " + typeof(ReturnValueType).FullName);
                    }
                    else
                    {
                        if (Values.TryGetValue(pEchoName, out object oGlobalEchoObject))
                        {
                            if (oGlobalEchoObject is main.objects.description.access.get.IInformationCreatingObject globalObjectDirectoryNameReduse)
                            {
                                Exception(Ex.GlobalObjectsManager.x10008, pEchoName, globalObjectDirectoryNameReduse.GetExplorerObject());
                            }
                            else
                            {
                                Exception(Ex.GlobalObjectsManager.x10009, typeof(main.objects.description.access.get.IInformationCreatingObject).FullName, 
                                    oGlobalEchoObject.GetType().FullName);
                            }
                        }
                        else
                        {
                            main.objects.receive.echo.Object<ReceiveValueType, ReturnValueType> receiveEchoObject 
                                = new main.objects.receive.echo.Object<ReceiveValueType, ReturnValueType>
                                (Informing, StateInformation, PollAccess, HeaderInformation.Explorer, NodeInformation.ID, NodeInformation.NodeObject.GetID());

                            Informing.SystemInformation($"Creating[GlobalEcho - {pEchoName}<ReceiveValue:" +
                                $"{typeof(ReceiveValueType).FullName}, ReturnValue:{typeof(ReturnValueType).FullName}>]");

                            Values.Add(pEchoName, receiveEchoObject);

                            CreatingInCurrentObjectGlobalObjectsKey = Hellper.ExpendArray(CreatingInCurrentObjectGlobalObjectsKey, pEchoName);

                            return receiveEchoObject;
                        }
                    }
                }
                else
                {
                    string echoName = HeaderInformation.FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName;

                    if (Values.ContainsKey(echoName))
                    {
                        Exception(Ex.GlobalObjectsManager.x10005, typeof(ReceiveValueType).FullName);
                    }
                    else
                    {
                        main.objects.receive.echo.Object<ReceiveValueType, ReturnValueType> receiveEchoObject 
                            = new main.objects.receive.echo.Object<ReceiveValueType, ReturnValueType>
                            (Informing, StateInformation, PollAccess, HeaderInformation.Explorer, NodeInformation.ID, NodeInformation.NodeObject.GetID());

                        Informing.SystemInformation($"Creating[GlobalEcho<ReceiveValue:{typeof(ReceiveValueType).FullName}," +
                            $" ReturnValue:{typeof(ReturnValueType).FullName}>]");

                        Values.Add(echoName, receiveEchoObject);

                        CreatingInCurrentObjectGlobalObjectsKey = Hellper.ExpendArray(CreatingInCurrentObjectGlobalObjectsKey, echoName);

                        return receiveEchoObject;
                    }
                }
            }
            else
                Exception(Ex.GlobalObjectsManager.x10001, typeof(ReceiveValueType).FullName);

            return default;
        }

        public IInput<SendingValueType> CreatingSending<SendingValueType>(string pName, int pRangeSize)
        {
            if (StateInformation.IsCreating || StateInformation.IsOccurrence)
            {
                if (pName == "")
                {
                    if (HeaderInformation.IsNodeObject())
                        pName = HeaderInformation.FullName + typeof(SendingValueType).FullName;
                    else if (HeaderInformation.IsBranchObject())
                        pName = NodeInformation.NodeObject.HeaderInformation.FullName + typeof(SendingValueType).FullName;
                }
                    
                if (Values.TryGetValue(pName, out object oObject))
                {
                    if (oObject is main.objects.description.access.get.IInformationCreatingObject objectReduse)
                    {
                        Exception(Ex.GlobalObjectsManager.x10011, typeof(SendingValueType).FullName, 
                            pName, objectReduse.GetExplorerObject());
                    }
                    else
                        Exception(Ex.GlobalObjectsManager.x10012, oObject.GetType().FullName, 
                            typeof(main.objects.description.access.get.IInformationCreatingObject).FullName);
                }
                else
                {
                    main.objects.sending.Object<SendingValueType> sendingObject 
                        = new main.objects.sending.Object<SendingValueType>(pName, Informing, HeaderInformation.Explorer, 
                        NodeInformation.NodeObject.GetID(), NodeInformation.ID, pRangeSize);

                    Values.Add(pName, sendingObject);

                    CreatingInCurrentObjectGlobalObjectsKey = Hellper.ExpendArray(CreatingInCurrentObjectGlobalObjectsKey, pName);

                    Informing.SystemInformation($"Creating [GlobalSending:{typeof(SendingValueType)}.]");

                    return sendingObject;
                }
            }
            else
                Exception(Ex.GlobalObjectsManager.x10010, typeof(SendingValueType).FullName);

            return default;
        }

        public main.objects.description.IRestream<ListenValueType> CreatingListenSendingMessage<ListenValueType>
            (string pName, int pTimeDelay, string pPollName)
        {
            if (StateInformation.IsCreating || StateInformation.IsOccurrence)
            {
                if (Values.TryGetValue(pName, out object oObject))
                {
                    if (oObject is main.objects.description.access.get.IInformationCreatingObject objectInformationReduse)
                    {
                        //Узнаем является ли создателем рассылки родительский обьект.
                        bool isParent = false;
                        foreach(ulong id in NodeInformation.IDNodeParents)
                            if (id == objectInformationReduse.GetIDNodeObject())
                            {
                                isParent = true;

                                break;
                            }

                        if (isParent)
                        {
                            if (oObject is main.objects.sending.Object<ListenValueType> listenMessageObjectReduse)
                            {
                                main.objects.sending.listener.Object<ListenValueType> objectLisenerSending =
                                    new main.objects.sending.listener.Object<ListenValueType>
                                    (Informing, StateInformation, NodeAccess, PrivateHandlerManager, SharedObjectsManager, PollAccess, Dependency);

                                //Регистрируем на подписку через Node обьект.
                                ((subscribe.objects.description.add.IGlobal)NodeInformation.NodeObject).
                                    RegisterListenerObjectInSendingMessageObject(listenMessageObjectReduse, objectLisenerSending);

                                return objectLisenerSending;
                            }
                            else
                                Exception(Ex.GlobalObjectsManager.x10016, typeof(ListenValueType).FullName, pName);
                        }
                        else
                            Exception(Ex.GlobalObjectsManager.x10015, pName, objectInformationReduse.GetExplorerObject());
                    }
                    else
                        Exception(Ex.GlobalObjectsManager.x10012, oObject.GetType().FullName,
                            typeof(main.objects.description.access.get.IInformationCreatingObject).FullName);
                }
                else
                    Exception(Ex.GlobalObjectsManager.x10014, pName);
            }
            else
                Exception(Ex.GlobalObjectsManager.x10013, pName, typeof(ListenValueType).FullName);

            return default;
        }
    }

    
}


