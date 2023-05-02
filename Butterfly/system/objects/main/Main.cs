namespace Butterfly.system.objects.main
{
    public abstract class Object : activity.description.ILife, IInforming,
        description.activity.INode, 
        description.definition.INode, 
        information.description.access.get.INode, 
        description.activity.IBranch,
        manager.handlers.description.access.add.IPrivate, 
        description.access.add.IDependency, 
        manager.objects.description.access.get.IShared, 
        manager.objects.description.access.set.IShared, 
        manager.objects.description.access.remove.IMain,
        manager.objects.description.access.inform.IMain, 
        poll.description.access.add.IPoll, 
        manager.subscribe.objects.description.add.IGlobal,
        description.activity.IIndependent
    {
        #region Define

        private readonly information.State.Manager StateInformationManager;
        public readonly information.State StateInformation;
        public readonly information.Header HeaderInformation;
        public readonly information.Tegs TegsInformation;

        public Object()
        {
            HeaderInformation = new information.Header(this);
            StateInformation = new information.State();
            StateInformationManager = new information.State.Manager(StateInformation, HeaderInformation, this);
            TegsInformation = new information.Tegs(this, StateInformation);
        }

        protected void add_tegs(params string[] pTegs) => ((information.description.access.add.ITegs)TegsInformation).AddTegs(pTegs);

        #endregion

        #region Dependency

        private global::System.Action[] DependecyArray;

        void description.access.add.IDependency.Add(global::System.Action pAction)
        {
            if (DependecyArray == null) DependecyArray = new global::System.Action[0];
            DependecyArray = Hellper.ExpendArray(DependecyArray, pAction);
        }

        #endregion

        #region MainObjects

        private manager.objects.Main MainObjectsManager;

        void description.activity.IBranch.RemoveBranchMainObject() => MainObjectsManager.RemoveBrachObject();

        void manager.objects.description.access.inform.IMain.InformCreateChildrenObjectToParentObject() => MainObjectsManager.InformCreateObject();
            
        protected MainObjectType add_object<MainObjectType>(string pKey = "", object pLocalValueObject = null)
            where MainObjectType : Object, new()
        {
            return MainObjectsManager.TryAddBranch(new MainObjectType(), pKey, pLocalValueObject);
        }

        protected MainObjectType obj<MainObjectType>(string pKey = "", object pLocalValueObject = null)
            where MainObjectType : Object, new()
        {
            return MainObjectsManager.TryAddBranch(new MainObjectType(), pKey, pLocalValueObject);
        }

        /// <summary>
        /// Добавить/создать дочерний обьект, который представляет дочернию ветку.
        /// Вызов осуществляется в методе Start() или во время жизни текущего обьекта.
        /// Если явно не указать имя обьекта, то оно выставится автаматически.
        /// </summary>
        protected MainObjectType create_object<MainObjectType>(string pKey, object pLocalValueObject = null)
            where MainObjectType : Object, new()
        {
            return MainObjectsManager.CreateNode<MainObjectType>(pKey, pLocalValueObject);
        }

        /// <summary>
        /// Отложеное создание обьекта. Обьект будет создан когда текущий обьект перейдет в жизненый процесс Start.
        /// Это произойдет после вызова функции Start(). Тем самым будет произведена имитация создания во время работы.
        /// Обьект не может быть Independent, теперь он неявно является частью своего родителя.
        /// </summary>
        /// <returns></returns>
        protected void deferred_create_object<MainObjectType>(string pKey, object pLocalValueObject = null)
            where MainObjectType : Object, new()
        {
            MainObjectsManager.DeferredAddNode<MainObjectType>(pKey, pLocalValueObject);
        }

        protected bool try_delete_object<MainObjectType>(string pKey)
            where MainObjectType : Object, new()
        {
            return MainObjectsManager.TryDeleteNode<MainObjectType>(pKey);
        }

        void manager.objects.description.access.remove.IMain.Remove(string pKey, global::System.Type pType)
        {
            MainObjectsManager.Remove(pKey, pType);
        }

        #endregion

        #region PublicHandler

        private manager.handlers.Public PublicHandlersManager;

        void description.activity.IBranch.RemovePublicHandler() => PublicHandlersManager.Remove();


        /// <summary>
        /// Добовляем/получаем публичный обрабочик.
        /// Доступ к входному методу ToInput можно получить явно указав входной тип данных.
        /// </summary>
        /// <typeparam name="PublicHandlerType"></typeparam>
        /// <returns></returns>
        public PublicHandlerType handler<PublicHandlerType>()
            where PublicHandlerType : Object, handler.description.IRestream, IInput, new()
        {
            return PublicHandlersManager.TryAdd<PublicHandlerType>();
        }

        /// <summary>
        /// Добовляем/получаем публичный обрабочик.
        /// Доступ к входному методу ToInput можно получить явно указав входной тип данных.
        /// </summary>
        /// <typeparam name="PublicHandlerType"></typeparam>
        /// <returns></returns>
        public PublicHandlerType handler<PublicHandlerType>(string pPublicHandlerName = "")
            where PublicHandlerType : Object, handler.description.IRestream, IInput, new()
        {
            return PublicHandlersManager.TryAdd<PublicHandlerType>(pPublicHandlerName);
        }

        /// <summary>
        /// Добовляем/получаем публичный обрабочик.
        /// Доступ к входному методу ToInput можно получить явно указав входной тип данных.
        /// </summary>
        /// <typeparam name="PublicHandlerType"></typeparam>
        /// <returns></returns> 
        protected handler.description.IRestream add_handler<PublicHandlerType>(string pPublicHandlerName = "")
            where PublicHandlerType : Object, handler.description.IRestream, IInput, new()
        {
            return PublicHandlersManager.TryAdd<PublicHandlerType>(pPublicHandlerName);
        }

        protected handler.description.IRestream add_handler<PublicHandlerType, InputValueType>(ref IInput<InputValueType> rInput, string pPublicHandlerName = "")
            where PublicHandlerType : Object, handler.description.IRestream, IInput, new()
        {
            return PublicHandlersManager.TryAdd<PublicHandlerType, InputValueType>(ref rInput, pPublicHandlerName);
        }

        /// <summary>
        /// Добовляем публичный обработчик, получает его входной метод.
        /// </summary>
        /// <typeparam name="PublicHandlerValueType"></typeparam>
        /// <returns></returns>
        protected IInput<InputValueType> add_handler<PublicHandlerValueType, InputValueType>(string pPublicHandlerName = "")
            where PublicHandlerValueType : Object, IInput<InputValueType>, new()
        {
            return PublicHandlersManager.TryAdd<PublicHandlerValueType>(pPublicHandlerName);
        }


        #endregion

        #region PrivateHandler

        private manager.handlers.Private PrivateHandlersManager;

        void description.activity.IBranch.RemovePrivateHandler() => PrivateHandlersManager.Remove();

        PrivateHandlerType manager.handlers.description.access.add.IPrivate.Add<PrivateHandlerType>
            (global::System.Action<int> pContinueExecutingEvents, int pNumberOfTheInterruptedEvent)
        {
            return PrivateHandlersManager.Add<PrivateHandlerType>(pContinueExecutingEvents, pNumberOfTheInterruptedEvent);
        }

        PrivateHandlerType manager.handlers.description.access.add.IPrivate.Add<PrivateHandlerType>()
        {
            return PrivateHandlersManager.Add<PrivateHandlerType>();
        }

        PrivateHandlerType manager.handlers.description.access.add.IPrivate.Add<PrivateHandlerType>(PrivateHandlerType pPrivateHandler)
        {
            return PrivateHandlersManager.Add(pPrivateHandler);
        }

        #endregion

        #region Independent

        void description.activity.IIndependent.NodeObjectStop()
        {
            // Начнем остановку всех его Node обьектов.
            MainObjectsManager.StoppingAllNodeObject();
        }

        void description.activity.IIndependent.BackgroundObjectStop()
        {
            MainObjectsManager.StoppingBackgroundNodeObject();
        }

        #endregion

        #region Node    

        private information.Node NodeInformation;

        void description.definition.INode.NodeDefine(Object pParentObject, Object pIndependentObject, SYSTEM.objects.description.access.ISystem pSystemAccess,
            global::System.Collections.Generic.Dictionary<string, object> pGlobalObjects, string pKeyObject, int pAttachmentNodeNumberInSystem, ulong[] pNodeIDParents)
        {
            HeaderInformation.NodeDefine(pParentObject.HeaderInformation.Explorer, GetType(), pParentObject.NodeInformation, pKeyObject);
           
            if (HeaderInformation.IsIndependent()) pIndependentObject = this;

            NodeInformation = new information.Node(this, pParentObject, pIndependentObject, pSystemAccess, pKeyObject, 
                pAttachmentNodeNumberInSystem, pNodeIDParents);
            
            Define(pGlobalObjects);
        }

        void description.definition.INode.BranchDefine(Object pNodeObject, Object pParentObject, Object pIndependentObject, 
            SYSTEM.objects.description.access.ISystem pSystemAccess, global::System.Collections.Generic.Dictionary<string, object> pGlobalObjects, 
            string pKeyObject, ulong pUniqueNodeKey, int pAttackmentNumberObjectInNode, int pAttachmentNodeNumberInSystem, ulong[] pNodeIDParents)
        {
            HeaderInformation.BranchDefine(pParentObject.HeaderInformation.Explorer, GetType(), pParentObject.NodeInformation, pKeyObject);

            NodeInformation = new information.Node(pNodeObject, this, pParentObject, pIndependentObject, pSystemAccess, pKeyObject, 
                pUniqueNodeKey, pAttackmentNumberObjectInNode, pAttachmentNodeNumberInSystem, pNodeIDParents);

            Define(pGlobalObjects);
        }

        private void Define(System.Collections.Generic.Dictionary<string, object> pGlobalObjects)
        {
                // *********************************************************************************************************************************//
            {   // *********************************************************************************************************************************//
                SubscriptionControlManager = new manager.control.Subscription(this, StateInformation, NodeInformation, ContinueCreateNode, ContinueStop);

                AccessPollsSystemManager = new manager.system.polls.Access(this, HeaderInformation, NodeInformation, StateInformation, SubscriptionControlManager);
                GlobalObjectSubscribe = new manager.subscribe.objects.Global(this, HeaderInformation, NodeInformation, StateInformation, SubscriptionControlManager);
            }   // *********************************************************************************************************************************//
            {   // *********************************************************************************************************************************//
                LocalObjectsManager = new manager.objects.Local(StateInformation, this);
                GlobalObjectsManager = new manager.objects.Global(StateInformation, HeaderInformation, NodeInformation, this, pGlobalObjects, this, this, this, this, this);
                MainObjectsManager = new manager.objects.Main(this, HeaderInformation, StateInformation, StateInformationManager, NodeInformation, GlobalObjectsManager, ContinueStop);
            }   // *********************************************************************************************************************************//
            {   // *********************************************************************************************************************************//
                PrivateHandlersManager = new manager.handlers.Private(this, HeaderInformation, StateInformation, NodeInformation, GlobalObjectsManager);
                PublicHandlersManager = new manager.handlers.Public(this, HeaderInformation, StateInformation, NodeInformation, GlobalObjectsManager);
            }   // *********************************************************************************************************************************//
                // *********************************************************************************************************************************//
            {
                LifeCyrcleActivity = new activity.LifeCyrcle(this, HeaderInformation, StateInformation, StateInformationManager, NodeInformation, 
                    TegsInformation, MainObjectsManager, LocalObjectsManager, GlobalObjectsManager, PublicHandlersManager, PrivateHandlersManager, SubscriptionControlManager);
            }
        }

        void description.activity.INode.Create()
        {
            ((activity.description.ILife)this).Construction(); // Устанавливаем связи.
        }

        // Продолжить создание узла когда попишимся на все что надо.
        private void ContinueCreateNode()
        {
            ((activity.description.ILife)this).Start(); // Запускаем.

            // Информирет родительский обьект о том что текущий обьект(дочерний) был создан. С этого момента родительский обьект может быть уничтожен.
            // Обьект может быть уничтожен только после того как все добавленые Node обьекты были созданы.
            // Подсчет добавленых и созданых обьектов ведется в отдельном инкрементируемом и декриментируемом значении.
            // Отчасти именно по этому нельзя вызывать destroy() в Contruction(). Этот метод должен 100 процентов запускаться и завершаться
            // в Node обьекте и во всех его Branch обьектах. По окончанию выполнения метода Start() вызовется текущий метод
            // InformCreateChildrenObjectToParentObject() и проинформирует что обьект был полностью создан. Так как может быть вызван destroy() в Configurate(),
            // по причине не доставерности данных или какой либо другой, мы можем проинфомировать родительский или какой либо другой глобальный обьект о причиной сбоя
            // перед уничтожением. В случае уничтожения обьекта в Configurate() метод Start() вызван не будет,
            // но будет вызван метод Stop() в котором вы также сможете передать какие либо данные в родительский обьекты или глобальные.
            // Так же метод destroy() может быть вызван в методе Start(), но это нецелесообразно с точки срения производительности.
            // Если необходимо установить соединение с сервером или BD
            // лучше сделать это в Configurate(), тем самым прервется цепочка вызовов в его Branch обьектах, которые уже будут не нужны.
            // Так же не будет выполнен запуск потоков в методе ContinueStart(), это тоже дастаточно ресурсо емко.

            ((manager.objects.description.access.inform.IMain)NodeInformation.ParentMainObject).InformCreateChildrenObjectToParentObject();

            if (NodeInformation.NodeObject.StateInformation.IsDestroy)
            {
                //...
            }
            else // Влючаем потоки(если необходимо) и меняем жизненый цикл на Start.
                ((activity.description.ILife)this).ContinueStart();
        }

        void description.activity.INode.Destroy()
        {
            lock(StateInformation.Locker)
            {
                if (HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                {
                    StateInformationManager.Destroy();

                    NodeInformation.SystemAccess.AddActionInvoke(MainObjectsManager.StoppingAllNodeObject);
                }
                // Метка Destroy могла быть уже вытсавлена в одном из Branch обьектов для того что бы преостановить
                // создания Node обьектов в ней. Но уничтожаться Branch обьект всегда будет в составе всего Independent
                // обьекта.
                else if (HeaderInformation.IsBranchObject())
                {
                    StateInformationManager.Destroy();

                    NodeInformation.SystemAccess.AddActionInvoke(MainObjectsManager.StoppingAllNodeObject);
                }
            }
        }

        void description.activity.INode.SetStateStopping()
        {
            StateInformationManager.Set(information.State.Data.STOPPING);
        }

        public ulong GetID() => NodeInformation.ID;
        public ulong GetNodeID() => NodeInformation.NodeID;
        public int GetAttachmentNodeNumberInSystem() => NodeInformation.AttachmentNodeNumberInSystem;
        public int GetAttackmentNumberObjectInNode() => NodeInformation.AttackmentNumberObjectInNode;
        public string GetKey() => NodeInformation.KeyObject;

        #endregion

        #region LocalObjects

        private manager.objects.Local LocalObjectsManager;

        /// <summary>
        /// Создает/получает локальный обьект который реализует интерфейс ILocalObject, 
        /// во время выполнения методов construction(), start();
        /// Указав базовым класом InformationSpace вы получите доступ к информационому пространсву объекта 
        /// в котором вы определили данный локальный обьект.
        /// Используя возможности описаные во вложеных интерфейсах ILocalObject мы пожете гибко настроить жизнь локального обьекта вокруг жизненых процесов его пространсва.
        /// Если не задать явно имя локального обькта, то именем выступит его тип.
        /// Далее при вызове метода l_obj'<typeparamref name="LocalObjectType"/>'() вы получите этот обьект.
        /// Если задать 2 обьекта одного и тогоже типа, но одному указать имя явно, а второму не явно(именем будет его тип)
        /// то вы получите ошибку времени сборки. Разным обьектам с одинаковыми типами нужно явно указывать их имена. 
        /// </summary>
        /// <typeparam name="LocalObjectType">Тип локального обьекта.</typeparam>
        /// <param name="pObjectName">Имя локального обьекта.</param>
        /// <returns></returns>
        protected LocalObjectType l_obj<LocalObjectType>(string pObjectName = "") where LocalObjectType : ILocalObject, new()
        {
            return LocalObjectsManager.TryAdd<LocalObjectType>(pObjectName);
        }

        #endregion

        #region GlobalObjects

        private manager.subscribe.objects.Global GlobalObjectSubscribe;

        void manager.subscribe.objects.description.add.IGlobal.RegisterListenerObjectInSendingMessageObject<SendingValueType> 
            (objects.sending.Object<SendingValueType> pGlobalSendingMessageObject,
                objects.sending.listener.Object<SendingValueType> pListenerSendingMessageObject)
        {
            GlobalObjectSubscribe.RegisterListenerObjectInSendingMessageObject
                (pGlobalSendingMessageObject, pListenerSendingMessageObject);
        }

        private manager.objects.Global GlobalObjectsManager;


        /// <summary>
        /// Создает обьект для массовой рассылки сообщения всем кто подписан на нее в данный момент.
        /// Если не указать имя явно, то именем выступит Type.FullName + тип рассылаемого значения.
        /// Все подписавшиеся хранятся в многомерных массивах, вы пожете явно указать размер массива.
        /// Так же его можно выставить на работу в пулл. 
        /// </summary>
        /// <typeparam name="SendingValueType"></typeparam>
        /// <returns></returns>
        protected IInput<SendingValueType> sending_message<SendingValueType>(string pSendingMessageName)
        {
            return GlobalObjectsManager.CreatingSending<SendingValueType>
                (pSendingMessageName, GlobalData.DEFAULT_VALUE_SENDING_MESSAGE_ARRAY_SIZE);
        }

        /// <summary>
        /// Создает обьект для массовой рассылки сообщения всем кто подписан на нее в данный момент.
        /// Если не указать имя явно, то именем выступит Type.FullName + тип рассылаемого значения.
        /// Все подписавшиеся хранятся в многомерных массивах, вы пожете явно указать размер массива.
        /// Так же его можно выставить на работу в пулл. 
        /// </summary>
        /// <typeparam name="SendingValueType"></typeparam>
        /// <returns></returns>
        protected IInput<SendingValueType> sending_message<SendingValueType>()
        {
            return GlobalObjectsManager.CreatingSending<SendingValueType>("", GlobalData.DEFAULT_VALUE_SENDING_MESSAGE_ARRAY_SIZE);
        }

        protected main.objects.description.IRestream<ListenValueType> listening_messages<ListenObjectType, ListenValueType>()
        {
            return GlobalObjectsManager.CreatingListenSendingMessage<ListenValueType>
                (typeof(ListenObjectType).FullName + typeof(ListenValueType).FullName, 0, "");
        }
        protected main.objects.description.IRestream<ListenValueType> listening_messages<ListenValueType>(string pName)
        {
            return GlobalObjectsManager.CreatingListenSendingMessage<ListenValueType>(pName, 0, "");
        }

        /// <summary>
        /// Глобаное эхо. Доступно для всех дочерних обьектов через сигнаруные методы async_...._to_echo().
        /// Создайте локальный класс подлкючите к нему интерфейс ILocalObject.ReceiveEcho[<typeparamref name="ReturnValueType"/>], после явно укажите метод
        /// который реализует данный интерфейс с помощью .output_to(l_obj[LocalObject]().ReceiveEcho).
        /// При создании одинаковых глобальных echo задайте им имена, иначе может произойти ошибка времени сборки.
        /// Если имя не задать явно, то его менем станет GetType().FullName текущего обьекта + <typeparamref name="ReceiveValueType"/>.GetType().FullName 
        /// + <typeparamref name="ReturnValueType"/>.GetType.FullName .
        /// </summary>
        /// <returns></returns>
        protected objects.description.IRestream<ReceiveValueType, IEchoReturn<ReturnValueType>> listen_echo
            <ReceiveValueType, ReturnValueType>(string pEchoName = "")
        {
            return GlobalObjectsManager.CreatingEcho<ReceiveValueType, ReturnValueType>(pEchoName);
        }

        /// <summary>
        /// Глобаное эхо. Доступно для всех дочерних обьектов через сигнаруные методы async_...._to_echo() или .
        /// Создайте локальный класс подлкючите к нему интерфейс ILocalObject.ReceiveEcho[<typeparamref name="ValueType"/>], после явно  укажите метод
        /// который реализует данный интерфейс с помощью .output_to(l_obj[LocalObject]().ReceiveEcho).
        /// При создании одинаковых глобальных echo задайте им имена, иначе может произойти ошибка времени сборки.
        /// Если имя не задать явно, то его менем станет GetType().FullName текущего обьекта + <typeparamref name="ValueType"/>.GetType().FullName .
        /// </summary>
        /// <returns></returns>
        protected objects.description.IRestream<ValueType, IEchoReturn<ValueType>> listen_echo<ValueType>
            (string pEchoName = "")
        {
            return GlobalObjectsManager.CreatingEcho<ValueType, ValueType>(pEchoName);
        }

        protected objects.description.IRestream<ReturnValueType> send_echo<ReceiveValueType, ReturnValueType>
            (ref IInput<ReceiveValueType> rInput, string pEchoName)
        {
            return GlobalObjectsManager.CreatingSendEcho<ReceiveValueType, ReturnValueType>(pEchoName, ref rInput);
        }
        protected objects.description.IRestream<ValueType> send_echo<ValueType>
            (ref IInput<ValueType> rInput, string pEchoName)
        {
            return GlobalObjectsManager.CreatingSendEcho<ValueType, ValueType>(pEchoName, ref rInput);
        }
        protected objects.description.IRestream<ReturnValueType> send_echo<LocationEchoType, ReceiveValueType, ReturnValueType>
            (ref IInput<ReceiveValueType> rInput)
            where LocationEchoType : main.Object
        {
            return GlobalObjectsManager.CreatingSendEcho<ReceiveValueType, ReturnValueType>
                (typeof(LocationEchoType).FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName, ref rInput);
        }
        protected objects.description.IRestream<ValueType> send_echo<LocationEchoType, ValueType>
            (ref IInput<ValueType> rInput)
            where LocationEchoType : main.Object
        {
            return GlobalObjectsManager.CreatingSendEcho<ValueType, ValueType>
                (typeof(LocationEchoType).FullName + typeof(ValueType).FullName + typeof(ValueType).FullName, ref rInput);
        }

        protected objects.description.IRestream<ListenValueType> listen_message<ListenValueType>()
        {
            return GlobalObjectsManager.CreatingListenMessage<ListenValueType>(GetType().FullName + typeof(ListenValueType).FullName);
        }

        protected objects.description.IRestream<ListenValueType> listen_message<ListenValueType>(string pListenName)
        {
            return GlobalObjectsManager.CreatingListenMessage<ListenValueType>(pListenName);
        }

        protected IInput<SendValueType> send_message<ListenObjectType, SendValueType>()
        {
            return GlobalObjectsManager.CreatingSendMessage<SendValueType>(typeof(ListenObjectType).FullName + typeof(SendValueType).FullName);
        }

        protected IInput<SendValueType> send_message<SendValueType>(string pListenName)
        {
            return GlobalObjectsManager.CreatingSendMessage<SendValueType>(pListenName);
        }

        protected void send_message<ListenObjectType, SendValueType>(ref IInput<SendValueType> rInput)
        {
            rInput = GlobalObjectsManager.CreatingSendMessage<SendValueType>(typeof(ListenObjectType).FullName + typeof(SendValueType).FullName);
        }

        protected void send_message<SendValueType>(ref IInput<SendValueType> rInput, string pListenName)
        {
            rInput = GlobalObjectsManager.CreatingSendMessage<SendValueType>(pListenName);
        }

        bool manager.objects.description.access.get.IShared.GlobalTryGet(string pKey, out object oValueObject)
        {
            if (GlobalObjectsManager.TryGetValue(pKey, out object oObject))
            {
                oValueObject = oObject;
                return true;
            }

            oValueObject = null;
            return false;
        }

        void manager.objects.description.access.set.IShared.GlobalSet(global::System.Collections.Generic.Dictionary<string, object> pValues)
        {
            GlobalObjectsManager.Set(pValues);
        }

        #endregion

        #region Life

        private activity.LifeCyrcle LifeCyrcleActivity;

        public virtual void destroy() 
        {
            LifeCyrcleActivity.Destroy();
        }

        protected abstract void Construction();

        private void ConfigurateCall() 
        {
            if (Hellper.GetSystemMethod(activity.LifeCyrcle.SystemMethod.CONFIGURATE, GetType(), out System.Reflection.MethodInfo oSystemMethod))
            {
                oSystemMethod.Invoke(this, null);
            }
            else
                LifeCyrcleActivity.SetNotOverrideCallMethod(activity.LifeCyrcle.SystemMethod.CONFIGURATE);
        }
        private void StartCall() 
        {
            if (Hellper.GetSystemMethod(activity.LifeCyrcle.SystemMethod.START, GetType(), out System.Reflection.MethodInfo oSystemMethod))
            {
                oSystemMethod.Invoke(this, null);
            }
            else
                LifeCyrcleActivity.SetNotOverrideCallMethod(activity.LifeCyrcle.SystemMethod.START);
        }
        private void StopCall() 
        {
            if (Hellper.GetSystemMethod(activity.LifeCyrcle.SystemMethod.STOP, GetType(), out System.Reflection.MethodInfo oSystemMethod))
            {
                oSystemMethod.Invoke(this, null);
            }
            else
                LifeCyrcleActivity.SetNotOverrideCallMethod(activity.LifeCyrcle.SystemMethod.STOP);
        }

        void activity.description.ILife.Construction()
        {
            LifeCyrcleActivity.Construction(Construction);
        }

        void activity.description.ILife.Dependency()
        {
            lock (StateInformation.Locker)
            {
                if (NodeInformation.NodeObject.StateInformation.StartProcess)
                    for (int i = 0; i < DependecyArray?.Length; i++)
                    {
                        
                        DependecyArray?[i].Invoke();
                    }    
            }
        }

        void activity.description.ILife.Start()
        {
            LifeCyrcleActivity.Start(StartCall, ConfigurateCall);
        }

        void activity.description.ILife.Stop()
        {
            if (this is thread.description.activity.IThread threadStopReduse)
                LifeCyrcleActivity.Stop(StopCall, threadStopReduse);
            else
                LifeCyrcleActivity.Stop(StopCall);
        }

        private void ContinueStop()
        {
            LifeCyrcleActivity.ContinueStop(StopCall);
        }

        void activity.description.ILife.ContinueStop()
        {
            ContinueStop();
        }

        void activity.description.ILife.ContinueStart()
        {
            if (this is thread.description.activity.IThread threadStartReduse)
                LifeCyrcleActivity.StartThread(threadStartReduse);
            else
                LifeCyrcleActivity.StartThread();
        }

        #endregion

        #region Console, Exception, RuntimeError, SystemInfomation, GetUniqueID

        public void Exception(string pMessage, params string[] pParams)
        {
            System.Console.ForegroundColor = System.ConsoleColor.Red;
            System.Console.WriteLine(HeaderInformation.ExplorerFullInformation + "/" + pMessage, pParams);

            StateInformationManager.Set(information.State.Data.EXCEPTION);

            System.Threading.Thread.Sleep(10000000);
        }

        public void Exception(System.Exception pException)
        {
            Exception(pException.ToString());
        }

        public void Console(string pMessage)
        {
            global::System.Console.ForegroundColor = System.ConsoleColor.White;
            global::System.Console.WriteLine(HeaderInformation.ExplorerFullInformation + "[ " + pMessage + " ]");
        }

        public void ConsoleLine(string pMessage)
        {
            global::System.Console.ForegroundColor = System.ConsoleColor.White;
            global::System.Console.Write(HeaderInformation.ExplorerFullInformation + "[ " + pMessage + " ]");
        }

        public string ReadLine(string pMessage = "")
        {
            global::System.Console.ForegroundColor = System.ConsoleColor.White;
            global::System.Console.Write(HeaderInformation.ExplorerFullInformation + "/" + pMessage);

            return global::System.Console.ReadLine(); 
        }

        public void RuntimeError(string pMessage)
        {
            destroy();
        }

        public void SystemInformation(string pMessage, global::System.ConsoleColor pColor = global::System.ConsoleColor.Green)
        {
            System.Console.ForegroundColor = pColor;
            System.Console.WriteLine(HeaderInformation.ExplorerFullInformation + "[" + GetID() + "]" + "/" + pMessage);
        }

        #endregion

        #region System : Poll

        private manager.system.polls.Access AccessPollsSystemManager;
       
        void poll.description.access.add.IPoll.Add(global::System.Action pAction, int pSize, int pTimeDelay, string pName)
        {
            AccessPollsSystemManager.Add(pAction, pSize, pTimeDelay, pName);
        }

        #endregion

        #region SubscriptionCancellationControl

        private manager.control.Subscription SubscriptionControlManager;

        #endregion

        #region Hellpers

        private System.DateTime LocalDateTime = System.DateTime.Now;
        private static System.DateTime GlobalStartTime = System.DateTime.Now;

        protected void start_timer()
        {
            LocalDateTime = System.DateTime.Now;
        }

        protected int step_timer()
        {
            return (System.DateTime.Now.Subtract(LocalDateTime).Seconds * 1000) 
                + System.DateTime.Now.Subtract(LocalDateTime).Milliseconds;
        }

        protected static void global_start_timer()
        {
            GlobalStartTime = System.DateTime.Now;
        }

        protected static int global_end_timer()
        {
            return System.DateTime.Now.Subtract(GlobalStartTime).Milliseconds;
        }

        protected void sleep(int pTimeSpeep)
        {
            System.Threading.Thread.Sleep(pTimeSpeep);
        }

        #endregion        
    }
}


