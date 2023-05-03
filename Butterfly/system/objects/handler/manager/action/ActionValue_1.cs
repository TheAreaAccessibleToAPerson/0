namespace Butterfly.system.objects.handler.manager.action
{
    public class Object<ParamType> : main.Informing
    {
        /// <summary>
        /// Сдесь будет хранится способ обработки входящих данных.
        /// Обычная передача дальше по цепочке или запись в хранилище.
        /// </summary>
        public global::System.Action<ParamType> Action { private set; get; }

        private readonly events.Object<ParamType> EventsManager;

        private readonly main.manager.handlers.description.access.add.IPrivate PrivateHandlerManagerAccess;
        private readonly main.manager.objects.description.access.get.IShared SharedObjectsManagerAccess;
        private readonly main.description.access.add.IDependency DependencyAccess;
        private readonly main.IInforming Informing;

        private readonly poll.description.access.add.IPoll PollAccess;

        private readonly main.information.description.access.get.INode NodeAccess;
        private readonly main.information.State StateInformation;

        private bool IsContinueInterrupting;

        public Object(events.events.Type pCreatorType, main.information.State pStateInformation, 
            main.information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManagerAccess,
            main.manager.objects.description.access.get.IShared pSharedObjectsManagerAccess, 
            main.description.access.add.IDependency pDependencyAccess,
            main.IInforming pInforming, 
            poll.description.access.add.IPoll pPollAccess) 
            : base("ActionValue_1", pInforming)
        {
            EventsManager = new events.Object<ParamType>(pInforming, pCreatorType);

            IsContinueInterrupting = false;

            NodeAccess = pNodeAccess;
            StateInformation = pStateInformation;
            PrivateHandlerManagerAccess = pPrivateHandlerManagerAccess;
            SharedObjectsManagerAccess = pSharedObjectsManagerAccess;
            DependencyAccess = pDependencyAccess;
            PollAccess = pPollAccess;
            Informing = pInforming;

            // Устанавливаем по умолчанию стандартный способ работы с данными(передача далее по цепочке).
            Action = DefaultInput;
        }

        public Object(events.events.Type pCreatorType,
            global::System.Action<int> pContinueExecutingEvents, int NumberOfTheInterruptedEvent, 
            main.information.State pStateInformation,
            main.information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManagerAccess,
            main.manager.objects.description.access.get.IShared pSharedObjectsManagerAccess,
            main.description.access.add.IDependency pDependencyAccess,
            main.IInforming pInforming,
            poll.description.access.add.IPoll pPollAccess)
            : base("ActionValue_1", pInforming)
        {
            EventsManager = new events.Object<ParamType>(pInforming, pCreatorType, pContinueExecutingEvents, NumberOfTheInterruptedEvent);

            IsContinueInterrupting = true;

            NodeAccess = pNodeAccess;
            StateInformation = pStateInformation;
            PrivateHandlerManagerAccess = pPrivateHandlerManagerAccess;
            SharedObjectsManagerAccess = pSharedObjectsManagerAccess;
            DependencyAccess = pDependencyAccess;
            PollAccess = pPollAccess;
            Informing = pInforming;

            // Устанавливаем по умолчанию стандартный способ работы с данными(передача далее по цепочке).
            Action = DefaultInput;
        }

        // Теперь, обькт принимает данные и записывает их в хранилище.
        // Дальнейшая работа будет происходить в составе пула.
        private collections.safe.Values<ParamType> ValuesSafeCollection;
        private void PollProcess() // Даный метод будет подписан в пул.
        {
            if (ValuesSafeCollection.ExtractAll(out ParamType[] oValueArray))
            {
                foreach (ParamType value in oValueArray)
                {
                    DefaultInput(value);
                }
            }
        }

        /// <summary>
        /// Регистрируем метод PollProcess в пуллы.
        /// Меняем обычный способ приема данных с дальнейшей ретрансляцией
        /// на запись данных в хранилище. Далее в пуллах вызовется данный метод
        /// извлекет записаные данные и передаст дальше по цепочке до следующего
        /// места где так же реализован такой же способ передачи данных.
        /// </summary>
        public void RegisterInPoll(int pSizePoll, int pTimeDelay, string pName)
        {
            if (pSizePoll != 0)
            {
                // Инициализируем хранилище данных.
                ValuesSafeCollection = new collections.safe.Values<ParamType>();
                // Далее переопределяем стандартный способ работы с входными данными
                // (дальнейшая передача по цепочке) на запись в хранилище.
                Action = ValuesSafeCollection.Add;
                // Создаем регистрационый билет. Который передасться в систему
                // после того как все связи установятся.
                PollAccess.Add(PollProcess, pSizePoll, pTimeDelay, pName);
            }
        }

        /// <summary>
        /// Из за пулов может зайти дразу 2 операции.
        /// Будем в echo записывать на какой операции мы остановились.
        /// И после возрата продолжать с того же места.
        /// </summary>
        /// <param name="pValue"></param>
        private void DefaultInput(ParamType pValue)
        {
            EventsManager.Run(pValue);

            if (IsContinueInterrupting)
            {
                EventsManager.ContinueExecutingEvents(EventsManager.NumberOfTheInterruptedEvent);
            }
        }

        public handler.description.IRestream<ParamType> AddAction
            (global::System.Action<ParamType> pAction, int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                IsContinueInterrupting = false;

                objects.action.Object<ParamType>  actionObject = new objects.action.Object<ParamType>(pAction,
                        EventsManager.ContinueExecutingEvents, EventsManager.NumberOfTheInterruptedEvent,
                            Informing, StateInformation, NodeAccess, PrivateHandlerManagerAccess,
                                SharedObjectsManagerAccess, PollAccess, DependencyAccess, pPollSize, pTimeDelay, pPollName);

                EventsManager.Add(events.Object<ParamType>.Type.Break, actionObject.ToInput);

                return actionObject;
            }
            else
                Exception(Ex.ActionValue.x10001, pAction.GetType().FullName);

            return default;
        }

        public handler.description.IRestream<ParamType> AddActionIsAsync
            (global::System.Action<ParamType> pAction, int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                IsContinueInterrupting = false;

                objects.action.Object<ParamType> actionObject = new objects.action.Object<ParamType>(pAction,
                            Informing, StateInformation, NodeAccess, PrivateHandlerManagerAccess,
                                SharedObjectsManagerAccess, PollAccess, DependencyAccess, pPollSize, pTimeDelay, pPollName);

                EventsManager.Add(events.Object<ParamType>.Type.Continue, actionObject.ToInput);

                return actionObject;
            }
            else
                Exception(Ex.ActionValue.x10001, pAction.GetType().FullName);

            return default;
        }

        /// <summary>
        /// Создает обьект для Action'<typeparamref name="InputValueType"/>' и перенаправляет в его метод ToInput
        /// (c проверкой на сооветсвие типа) все входящие данные.
        /// Используется только когда MainObject находится в состоянии __IsCreating.
        /// </summary>
        public handler.description.IRestream AddActionIsType<InputValueType>
            (global::System.Action<InputValueType> pAction, int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                objects.action.Object<InputValueType> actionObject;

                actionObject = new objects.action.Object<InputValueType>(pAction,
                        EventsManager.ContinueExecutingEvents, EventsManager.NumberOfTheInterruptedEvent,
                            Informing, StateInformation, NodeAccess, PrivateHandlerManagerAccess,
                                SharedObjectsManagerAccess, PollAccess, DependencyAccess, pPollSize, pTimeDelay, pPollName);

                IsContinueInterrupting = false;

                if (actionObject is IInput<ParamType> actionObjectReduse)
                {
                    EventsManager.Add(events.Object<ParamType>.Type.Break, actionObjectReduse.ToInput);
                }
                else
                    Exception(Ex.ActionValue.x10008, typeof(ParamType).FullName, actionObject.Name);

                return actionObject;
            }
            else
                Exception(Ex.ActionValue.x10007, pAction.GetType().FullName);

            return default;
        }

        public description.IRestream<OutputValueType> AddFunc<OutputValueType>
            (global::System.Func<ParamType, OutputValueType> pFunc, int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                IsContinueInterrupting = false;

                objects.func.Object<ParamType, OutputValueType> funcObject = new objects.func.Object<ParamType, OutputValueType>
                    (pFunc, EventsManager.ContinueExecutingEvents, EventsManager.NumberOfTheInterruptedEvent,
                    Informing, StateInformation, NodeAccess, PrivateHandlerManagerAccess, 
                    SharedObjectsManagerAccess, PollAccess, DependencyAccess, pPollSize, pTimeDelay, pPollName);

                EventsManager.Add(events.Object<ParamType>.Type.Break, funcObject.ToInput);

                return funcObject;
            }
            else
                Exception(Ex.ActionValue.x10001, pFunc.GetType().FullName);

            return default;
        }

        public description.IAsyncRestream<OutputValueType> AddFuncIsAsync<OutputValueType>
            (global::System.Func<ParamType, OutputValueType> pFunc, int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                objects.func.Object<ParamType, OutputValueType> funcObject = new objects.func.Object<ParamType, OutputValueType>
                    (pFunc, Informing, StateInformation, NodeAccess, PrivateHandlerManagerAccess,
                    SharedObjectsManagerAccess, PollAccess, DependencyAccess, pPollSize, pTimeDelay, pPollName);

                EventsManager.Add(events.Object<ParamType>.Type.Continue, funcObject.ToInput);

                return funcObject;
            }
            else
                Exception(Ex.ActionValue.x10001, pFunc.GetType().FullName);

            return default;
        }

        public description.IRestream AddPrivateHandler<PrivateHandlerType>(int pPollSize, int pTimeDelay, string pPollName) 
            where PrivateHandlerType : main.Object, IInput, IInput<ParamType>, description.IRestream, description.IRegisterInPoll,
                handler.description.IContinueInterrupting, new()
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                PrivateHandlerType handler;
                
                if (IsContinueInterrupting)
                {
                    handler = PrivateHandlerManagerAccess.Add<PrivateHandlerType>
                        (EventsManager.ContinueExecutingEvents, EventsManager.NumberOfTheInterruptedEvent);
                }
                else
                {
                    handler = PrivateHandlerManagerAccess.Add<PrivateHandlerType>();
                }

                IsContinueInterrupting = false;

                if (handler != null)
                {
                    handler.RegisterInPoll(pPollSize, pTimeDelay, pPollName);

                    EventsManager.Add(events.Object<ParamType>.Type.Break, handler.ToInput);
                }
                else
                    Exception(Ex.ActionValue.x10009, typeof(PrivateHandlerType).FullName);
                
                return handler;
            }
            else
                Exception(Ex.ActionValue.x10001, typeof(PrivateHandlerType).FullName);

            return default;
        }

        public description.IRestream AddPrivateHandlerIsAsync<PrivateHandlerType>(int pPollSize, int pTimeDelay, string pPollName)
            where PrivateHandlerType : main.Object, IInput, IInput<ParamType>, description.IRestream, description.IRegisterInPoll,
                handler.description.IContinueInterrupting, new()
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                PrivateHandlerType handler = PrivateHandlerManagerAccess.Add<PrivateHandlerType>();

                if (handler != null)
                {
                    handler.RegisterInPoll(pPollSize, pTimeDelay, pPollName);

                    EventsManager.Add(events.Object<ParamType>.Type.Continue, handler.ToInput);
                }
                else
                    Exception(Ex.ActionValue.x10009, typeof(PrivateHandlerType).FullName);

                return handler;
            }
            else
                Exception(Ex.ActionValue.x10001, typeof(PrivateHandlerType).FullName);

            return default;
        }

        /// <summary>
        /// Создает приватный обработчик, устанавливает прямой доступ в <typeparamref name="PrivateHandlerType"/> 
        /// </summary>
        /// <typeparam name="PrivateHandlerType"></typeparam>
        /// <returns></returns>
        public description.IRestream AddPrivateHandlerIsType<PrivateHandlerType>
            (int pPollSize, int pTimeDelay, string pPollName)
            where PrivateHandlerType : main.Object, description.IRestream, IInput, description.IRegisterInPoll,
            handler.description.IContinueInterrupting, new()
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                PrivateHandlerType privateHandler = PrivateHandlerManagerAccess.Add<PrivateHandlerType>
                        (EventsManager.ContinueExecutingEvents, EventsManager.NumberOfTheInterruptedEvent);

                IsContinueInterrupting = false;

                if (privateHandler is IInput<ParamType> privateHandlerReduse)
                {
                    EventsManager.Add(events.Object<ParamType>.Type.Break, privateHandlerReduse.ToInput);
                }
                else
                    Exception(Ex.ActionValue.x10008, typeof(ParamType).FullName, typeof(PrivateHandlerType).FullName);

                return privateHandler;
            }
            else
                Exception(Ex.ActionValue.x10007, typeof(PrivateHandlerType).FullName);

            return default;
        }

        /// <summary>
        /// Создает обьект для Func'<typeparamref name="InputValueType"/>','<typeparamref name="OutputValueType"/> и перенаправляет
        /// данные(c проверкой на сооветсвие типа) в его метод ToInput'<typeparamref name="InputValueType"/>'.
        /// Используется только когда MainObject находится в состоянии __IsCreating.
        /// </summary>
        public description.IRestream<OutputValueType> AddFuncIsType<InputValueType, OutputValueType>
            (global::System.Func<InputValueType, OutputValueType> pFunc, 
                int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                IsContinueInterrupting = false;

                objects.func.Object<InputValueType, OutputValueType> funcObject 
                    = new objects.func.Object<InputValueType, OutputValueType>
                    (pFunc, EventsManager.ContinueExecutingEvents, EventsManager.NumberOfTheInterruptedEvent, 
                        Informing,  StateInformation, NodeAccess, PrivateHandlerManagerAccess, SharedObjectsManagerAccess, 
                            PollAccess, DependencyAccess, pPollSize, pTimeDelay, pPollName);

                if (funcObject is IInput<ParamType> funcObjectReduse)
                {
                    EventsManager.Add(events.Object<ParamType>.Type.Break, funcObjectReduse.ToInput);
                }
                else
                    Exception(Ex.ActionValue.x10008, typeof(ParamType).FullName, funcObject.Name);

                return funcObject;
            }
            else
                Exception(Ex.ActionValue.x10007, pFunc.GetType().FullName);

            return default;
        }

        /// <summary>
        /// Создает обьект для Func'<typeparamref name="ParamType"/>','<typeparamref name="OutputValueType"/> и перенаправляет
        /// данные в его метод ToInput'<typeparamref name="ParamType"/>'.
        /// Используется только когда MainObject находится в состоянии __IsCreating.
        /// </summary>
        public description.IRestream<OutputValueType> AddFunc<OutputValueType>(global::System.Func<ParamType, OutputValueType> pFunc,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManagerAccess, 
            int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                IsContinueInterrupting = false;

                objects.func.Object<ParamType, OutputValueType> funcObject = new objects.func.Object<ParamType, OutputValueType>
                    (pFunc, EventsManager.ContinueExecutingEvents, EventsManager.NumberOfTheInterruptedEvent,
                    Informing, StateInformation, NodeAccess, pPrivateHandlerManagerAccess, SharedObjectsManagerAccess, 
                    PollAccess, DependencyAccess, pPollSize, pTimeDelay, pPollName);

                EventsManager.Add(events.Object<ParamType>.Type.Break, funcObject.ToInput);

                return funcObject;
            }
            else
                Exception(Ex.ActionValue.x10007, pFunc.GetType().FullName);

            return default;
        }
        
        #region Соединение с echo. Ответ будет содержать другие типы данных отличные от отпровляемых.

        /// <summary>
        /// Добовляет соединение с echo. С дальнейшей асинхроной работой. 
        /// </summary>
        /// <param name="pEchoName"></param>
        /// <param name="pManagerObjectsGlobal"></param>
        /// <returns></returns>
        public description.IAsyncRestream<ReturnValueType> AddConnectingToAsyncEcho<ReceiveValueType, ReturnValueType>(string pEchoName,
            int pPollSize, int pTimeDelay, string pPollName)
        {
            IsContinueInterrupting = true;

            return CreatingConnectingToEcho<ReceiveValueType, ReturnValueType>
                (pEchoName, events.Object<ParamType>.Type.Continue,
                    pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Добовляет соединение с echo. С дальнейшей реализасией синхроной работы в нутри обьекта.
        /// </summary>
        /// <param name="pEchoName"></param>
        /// <param name="pManagerObjectsGlobal"></param>
        /// <returns></returns>
        public description.IRestream<ReturnValueType> AddConnectingToEcho<ReceiveValueType, ReturnValueType>(string pEchoName,
            int pPollSize, int pTimeDelay, string pPollName)
        {
            IsContinueInterrupting = false;

            return CreatingConnectingToEcho<ReceiveValueType, ReturnValueType>
                (pEchoName, events.Object<ParamType>.Type.Break,
                    pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Реализует связь с echo с одинаковым типов передоваемых и принимаемых данных.
        /// </summary>
        private objects.echo.Object<ReceiveValueType, ReturnValueType> CreatingConnectingToEcho<ReceiveValueType, ReturnValueType>
            (string pEchoName, string pWorkEchoType, int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.IsCreating || StateInformation.IsOccurrence)
            {
                // Получаем глобальный прослушиватель.
                if (SharedObjectsManagerAccess.GlobalTryGet(pEchoName, out object oReceiveGlobalObject))
                {
                    // Получаем его входной метод.
                    if (oReceiveGlobalObject is IInput<ReceiveValueType, IEchoReturn<ReturnValueType>> 
                            receiveEchoGlobalObjectReduse)
                    {
                        objects.echo.Object<ReceiveValueType, ReturnValueType>.Type echoType = default;
                        {
                            if (pWorkEchoType == events.Object<ParamType>.Type.Break)
                                echoType = objects.echo.Object<ReceiveValueType, ReturnValueType>.Type.Break;
                            else if (pWorkEchoType == events.Object<ParamType>.Type.Continue)
                            {
                                echoType = objects.echo.Object<ReceiveValueType, ReturnValueType>.Type.Continue;
                            }
                            else
                                Exception(Ex.ActionValue.x10005, pWorkEchoType,
                                    events.Object<ReturnValueType>.Type.Break, events.Object<ReturnValueType>.Type.Continue);
                        }

                        // Создаем echo для обратной связи.
                        objects.echo.Object<ReceiveValueType, ReturnValueType> echoObject = new objects.echo.Object<ReceiveValueType, ReturnValueType>
                            (echoType, EventsManager.ContinueExecutingEvents, EventsManager.NumberOfTheInterruptedEvent, Informing, 
                                PrivateHandlerManagerAccess, SharedObjectsManagerAccess, StateInformation, NodeAccess,
                                    PollAccess, DependencyAccess, receiveEchoGlobalObjectReduse.ToInput, pPollSize, pTimeDelay, pPollName);

                        // Получаем входной метод echo,
                        // который будет перенаправлять данные в прослушивающий глобальный echo.
                        if (echoObject is IInput<ParamType> echoObjectInputReduse)
                        {
                            EventsManager.Add(pWorkEchoType, echoObjectInputReduse.ToInput);

                            return echoObject;
                        }
                        else
                            Exception(Ex.ActionValue.x10004, typeof(objects.echo.Object<ReceiveValueType, ReturnValueType>).FullName, 
                                typeof(IInput<ParamType>).FullName);
                    }
                    else
                        Exception(Ex.ActionValue.x10004, typeof(ILocalObject.ReceiveEcho<ParamType>).FullName,
                            typeof(IInput<ParamType, IEchoReturn<ReturnValueType>>).FullName);
                }
                else
                    Exception(Ex.ActionValue.x10006, pEchoName);
            }
            else
                Exception(Ex.ActionValue.x10003);

            return default;
        }

        #endregion
    }
}
