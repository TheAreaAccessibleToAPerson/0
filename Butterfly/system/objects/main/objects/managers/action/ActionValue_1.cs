namespace Butterfly.system.objects.main.objects.manager.action
{
    public class Object<ParamType> : main.Informing, IInput<ParamType>, IInput
    {
        /// <summary>
        /// Сдесь будет хранится способ обработки входящих данных.
        /// Обычная передача дальше по цепочке или запись в хранилище.
        /// </summary>
        public global::System.Action<ParamType> Action { private set; get; }

        private readonly manager.events.Object<ParamType> EventsManager;

        private readonly main.manager.handlers.description.access.add.IPrivate PrivateHandlerManagerAccess;
        private readonly main.manager.objects.description.access.get.IShared SharedObjectsManagerAccess;
        private readonly main.description.access.add.IDependency DependencyAccess;
        private readonly main.IInforming Informing;

        private readonly poll.description.access.add.IPoll PollAccess;

        private readonly main.information.description.access.get.INode NodeAccess;
        private readonly main.information.State StateInformation;

        public Object(main.information.State pStateInformation,
            main.information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManagerAccess,
            main.manager.objects.description.access.get.IShared pSharedObjectsManagerAccess,
            main.description.access.add.IDependency pDependencyAccess,
            main.IInforming pInforming,
            poll.description.access.add.IPoll pPollAccess)
            : base("ActionValue_1", pInforming)
        {
            EventsManager = new manager.events.Object<ParamType>();

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
        private collections.safe.Values<ParamType> ValuesSafeCollections;
        private void PollProcess() // Даный метод будет подписан в пул.
        {
            if (ValuesSafeCollections.ExtractAll(out ParamType[] oValueArray))
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
                ValuesSafeCollections = new collections.safe.Values<ParamType>();
                // Далее переопределяем стандартный способ работы с входными данными
                // (дальнейшая передача по цепочке) на запись в хранилище.
                Action = ValuesSafeCollections.Add;
                // Создаем регистрационый билет. Который передасться в систему
                // после того как все связи установятся.
                PollAccess.Add(PollProcess, pSizePoll, pTimeDelay, pName);
            }
        }

        public void ToInput(ParamType pValue)
        {
        }

        public void ToInput<ParamValueType>(ParamValueType pValue)
        {
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
        }

        public void AddAction(global::System.Action<ParamType> pAction, int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                objects.action.Object<ParamType> actionObject = new objects.action.Object<ParamType>
                    (pAction, DependencyAccess, PollAccess, pPollSize, pTimeDelay, pPollName);

                EventsManager.Add(actionObject.ToInput);
            }
            else
                Exception(Ex.ActionValue.x10001, pAction.GetType().FullName);
        }

        /// <summary>
        /// Создает обьект для Action'<typeparamref name="InputValueType"/>' и перенаправляет в его метод ToInput
        /// (c проверкой на сооветсвие типа) все входящие данные.
        /// Используется только когда MainObject находится в состоянии __IsCreating.
        /// </summary>
        public void AddActionIsType<InputValueType>(global::System.Action<InputValueType> pAction,
            int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                objects.action.Object<InputValueType> actionObject = new objects.action.Object<InputValueType>
                    (pAction, DependencyAccess, PollAccess, pPollSize, pTimeDelay, pPollName);

                if (actionObject is IInput<ParamType> actionObjectReduse)
                {
                    EventsManager.Add(actionObjectReduse.ToInput);
                }
                else
                    Exception(Ex.ActionValue.x10008, typeof(ParamType).FullName, actionObject.Name);
            }
            else
                Exception(Ex.ActionValue.x10007, pAction.GetType().FullName);
        }

        public description.IRestream<OutputValueType> AddFunc<OutputValueType>(global::System.Func<ParamType, OutputValueType> pFunc,
            int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                objects.func.Object<ParamType, OutputValueType> funcObject = new objects.func.Object<ParamType, OutputValueType>
                    (pFunc, Informing, StateInformation, NodeAccess, PrivateHandlerManagerAccess,
                    SharedObjectsManagerAccess, PollAccess, DependencyAccess);

                EventsManager.Add(funcObject.ToInput);

                return funcObject;
            }
            else
                Exception(Ex.ActionValue.x10001, pFunc.GetType().FullName);

            return default;
        }

        public handler.description.IRestream AddPrivateHandler<PrivateHandlerType>(int pPollSize, int pTimeDelay, string pPollName)
            where PrivateHandlerType : main.Object, IInput, IInput<ParamType>, handler.description.IRestream, handler.description.IRegisterInPoll,
                new()
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                PrivateHandlerType handler = PrivateHandlerManagerAccess.Add<PrivateHandlerType>();

                if (handler != null)
                {
                    handler.RegisterInPoll(pPollSize, pTimeDelay, pPollName);

                    EventsManager.Add(handler.ToInput);
                }
                else
                    Exception(Ex.ActionValue.x10009, typeof(PrivateHandlerType).FullName);

                return handler;
            }
            else
                Exception(Ex.ActionValue.x10001, typeof(PrivateHandlerType).FullName);

            return default;
        }

        public handler.description.IRestream AddPrivateHandlerIsReturnType<PrivateHandlerType>(int pPollSize, int pTimeDelay, string pPollName)
            where PrivateHandlerType : main.Object, IInput, IInput<ParamType>, handler.description.IRegisterInPoll, new()
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                PrivateHandlerType handler = new PrivateHandlerType();

                if (handler is handler.description.IRestream handlerReduse)
                {
                    PrivateHandlerManagerAccess.Add(handler);

                    if (handler != null)
                    {
                        handler.RegisterInPoll(pPollSize, pTimeDelay, pPollName);

                        EventsManager.Add(handler.ToInput);
                    }
                    else
                        Exception(Ex.ActionValue.x10009, typeof(PrivateHandlerType).FullName);

                    return handlerReduse;
                }
            }
            else
                Exception(Ex.ActionValue.x10001, typeof(PrivateHandlerType).FullName);

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
                objects.func.Object<InputValueType, OutputValueType> funcObject
                    = new objects.func.Object<InputValueType, OutputValueType>
                    (pFunc, Informing, StateInformation, NodeAccess, PrivateHandlerManagerAccess, 
                    SharedObjectsManagerAccess, PollAccess, DependencyAccess);

                if (funcObject is IInput<ParamType> funcObjectReduse)
                {
                    EventsManager.Add(funcObjectReduse.ToInput);
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
                objects.func.Object<ParamType, OutputValueType> funcObject = new objects.func.Object<ParamType, OutputValueType>
                    (pFunc, Informing, StateInformation, NodeAccess, pPrivateHandlerManagerAccess, SharedObjectsManagerAccess,
                    PollAccess, DependencyAccess);

                EventsManager.Add(funcObject.ToInput);

                return funcObject;
            }
            else
                Exception(Ex.ActionValue.x10007, pFunc.GetType().FullName);

            return default;
        }
        /// <summary>
        /// Создает приватный обработчик, устанавливает прямой доступ в <typeparamref name="PrivateHandlerType"/> 
        /// </summary>
        /// <typeparam name="PrivateHandlerType"></typeparam>
        /// <returns></returns>
        public handler.description.IRestream AddPrivateHandlerIsType<PrivateHandlerType>
            (int pPollSize, int pTimeDelay, string pPollName)
            where PrivateHandlerType : main.Object, handler.description.IRestream, IInput, handler.description.IRegisterInPoll, new()
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                PrivateHandlerType privateHandler = PrivateHandlerManagerAccess.Add<PrivateHandlerType>();

                if (privateHandler is IInput<ParamType> privateHandlerReduse)
                {
                    privateHandler.RegisterInPoll(pPollSize, pTimeDelay, pPollName);

                    EventsManager.Add(privateHandlerReduse.ToInput);
                }
                else
                    Exception(Ex.ActionValue.x10008, typeof(ParamType).FullName, typeof(PrivateHandlerType).FullName);

                return privateHandler;
            }
            else
                Exception(Ex.ActionValue.x10007, typeof(PrivateHandlerType).FullName);

            return default;
        }

        #region Соединение с echo. Ответ будет содержать другие типы данных отличные от отпровляемых.


        /// <summary>
        /// Добовляет соединение с echo. С дальнейшей реализасией синхроной работы в нутри обьекта.
        /// </summary>
        /// <param name="pEchoName"></param>
        /// <param name="pManagerObjectsGlobal"></param>
        /// <returns></returns>
        public description.IRestream<ReturnValueType> AddConnectingToEcho<ReceiveValueType, ReturnValueType>(string pEchoName,
            int pPollSize, int pTimeDelay, string pPollName)
        {
            return CreatingConnectingToEcho<ReceiveValueType, ReturnValueType>
                (pEchoName, pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Реализует связь с echo с одинаковым типов передоваемых и принимаемых данных.
        /// </summary>
        private sending.echo.Object<ReceiveValueType, ReturnValueType> CreatingConnectingToEcho<ReceiveValueType, ReturnValueType>
            (string pEchoName, int pPollSize, int pTimeDelay, string pPollName)
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
                        // Создаем echo для обратной связи.
                        sending.echo.Object<ReceiveValueType, ReturnValueType> echoObject = new sending.echo.Object<ReceiveValueType, ReturnValueType>
                            (Informing, PrivateHandlerManagerAccess, SharedObjectsManagerAccess, StateInformation, NodeAccess,
                            PollAccess, DependencyAccess, receiveEchoGlobalObjectReduse.ToInput);

                        // Получаем входной метод echo,
                        // который будет перенаправлять данные в прослушивающий глобальный echo.
                        if (echoObject is IInput<ParamType> echoObjectInputReduse)
                        {
                            EventsManager.Add(echoObjectInputReduse.ToInput);

                            return echoObject;
                        }
                        else
                            Exception(Ex.ActionValue.x10004, typeof(sending.echo.Object<ReceiveValueType, ReturnValueType>).FullName, typeof(IInput<ParamType>).FullName);
                    }
                    else
                        Exception(Ex.ActionValue.x10004, typeof(ILocalObject.ReceiveEcho<ParamType>).FullName,
                            typeof(IInput<ParamType, sending.echo.Object<ReceiveValueType, ReturnValueType>>).FullName);
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
