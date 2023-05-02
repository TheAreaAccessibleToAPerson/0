namespace Butterfly.system.objects.handler.objects.echo
{
    public class Object<ReceiveValueType, ReturnValueType> : main.Informing,
        IInput, IInput<ReceiveValueType>, handler.description.IAsyncRestream<ReturnValueType>, 
        handler.description.IRestream<ReturnValueType>, IEchoReturn<ReturnValueType>
    {
        public enum Type
        {
            /// <summary>
            /// В обьекте который воспользовался Echo, не прервалась цепочка событий.
            /// Поэтому мы просто вернем значение в нужную точку.
            /// </summary>
            Continue = 1,
            /// <summary>
            /// В обьекте который воспользовался Echo, прервалась цепочка событий.
            /// Поэтому мы сначало вернем значение в нужную точку завершим событие начиная с данного места 
            /// и после завершим все остальные события.
            /// </summary>
            Break = 2
        }

        private readonly manager.action.Object<ReturnValueType> OutputValueManager;

        /// <summary>
        /// Это место прияма данных у прослушивающего echo.
        /// </summary>
        private readonly System.Action<ReceiveValueType, Object<ReceiveValueType, ReturnValueType>> Action;

        /// <summary>
        /// Тип возрата значения. Если тип Continue, то значения просто вернутся в нужную точку.
        /// Если тип Break, то после возрата значений в нужную точку мы завершим все оставшиеся события.
        /// </summary>
        private Type ActionReturnType;
        /// <summary>
        /// Метод который в случае прерывание событий с помощью Break в используемом обьекте
        /// хранит ссылку на метод который запускает эти события на продолжение выполнение.
        /// Первый параметр это номер события после которого произошло прерывание.
        /// </summary>
        private readonly global::System.Action<int> ContinueExecutingEvents;
        private readonly int NumberOfTheInterruptedEvent; // Номер прерваного события.

        private readonly main.information.State StateInformation;
        private readonly main.information.description.access.get.INode NodeAccess;

        private readonly ulong ID;

        public Object(Type pActionReturnType, System.Action<int> pContinueExecutingEvents, int pNumberOfTheInterruptedEvent,
            main.IInforming pInforming,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerAccess,
            main.manager.objects.description.access.get.IShared pSharedObjectsAccess,
            main.information.State pStateInformation,
            main.information.description.access.get.INode pNodeAccess,
            poll.description.access.add.IPoll pPollAccess,
            main.description.access.add.IDependency pDependencyAccess,
            System.Action<ReceiveValueType, Object<ReceiveValueType, ReturnValueType>> pActionReceiveEcho,
                int pPollSize, int pTimeDelay, string pPollName)
            : base("EchoObject_2", pInforming)
        {
            ActionReturnType = pActionReturnType;

            ContinueExecutingEvents = pContinueExecutingEvents;
            NumberOfTheInterruptedEvent = pNumberOfTheInterruptedEvent;

            Console(pActionReturnType.ToString() + "!!!!!!!!!!!!!!!!!!!!!!!");

            ID = GetUniqueID();

            if (pActionReturnType.HasFlag(Type.Break))
            {
                OutputValueManager = new manager.action.Object<ReturnValueType>(manager.events.events.Type.Broker,
                    ContinueExecutingEvents, NumberOfTheInterruptedEvent,
                    pStateInformation, pNodeAccess, pPrivateHandlerAccess, pSharedObjectsAccess, pDependencyAccess, pInforming, pPollAccess);
            }
            else
            {
                OutputValueManager = new manager.action.Object<ReturnValueType>(manager.events.events.Type.Broker,
                pStateInformation, pNodeAccess, pPrivateHandlerAccess, pSharedObjectsAccess, pDependencyAccess, pInforming, pPollAccess);
            }
                
            StateInformation = pStateInformation;

            Action = pActionReceiveEcho;
        }

        public void SafeTo(ReturnValueType pValue)
        {
            lock (StateInformation.Locker)
            {
                if (StateInformation.StartProcess)
                {
                    // Продолжаем цепочку операций для async/await.
                    OutputValueManager.Action.Invoke(pValue);
                }
            }
        }

        public void To(ReturnValueType pValue)
        {
            lock (StateInformation.Locker)
            {
                if (StateInformation.IsStarting || StateInformation.StartProcess && StateInformation.IsDestroy == false)
                {
                    OutputValueManager.Action.Invoke(pValue);
                }
            }
        }

        void IInput<ReceiveValueType>.ToInput(ReceiveValueType pValue)
        {
            Action.Invoke(pValue, this);
        }

        /// <summary>
        /// Входящие данные через этот метод отправляются в прослушивающий echo.
        /// </summary>
        /// <param name="pValue"></param>
        void IInput.ToInput<ParamValueType>(ParamValueType pValue)
        {
            if (Action is System.Action<ParamValueType, Object<ReceiveValueType, ReturnValueType>> actionReceiveEchoReduse)
            {
                actionReceiveEchoReduse.Invoke(pValue, this);
            }
            else
                Exception(Ex.EchoReturn.x10001, typeof(ReceiveValueType).FullName, typeof(ParamValueType).FullName);
        }

        handler.description.IRestream<ReturnValueType> handler.description.IAsyncRestream<ReturnValueType>.await
            (global::System.Action<ReturnValueType> pAction, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputValueManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);
        }
        handler.description.IRestream<OutputValueType> handler.description.IAsyncRestream<ReturnValueType>.await<OutputValueType>
            (global::System.Func<ReturnValueType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputValueManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }
        handler.description.IRestream handler.description.IAsyncRestream<ReturnValueType>.await<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputValueManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
        void handler.description.IAsyncRestream<ReturnValueType>.await<PublicHandlerType>
            (global::System.Func<PublicHandlerType> pPublicHandler, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputValueManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void handler.description.IAsyncRestream<ReturnValueType>.await<PublicHandlerType>
            (global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputValueManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        handler.description.IRestream<ReturnValueType> handler.description.IRestream<ReturnValueType>.output_to
            (global::System.Action<ReturnValueType> pAction, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputValueManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);
        }

        handler.description.IRestream handler.description.IRestream<ReturnValueType>.output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputValueManager.AddPrivateHandlerIsType<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }

        handler.description.IRestream<OutputValueType> handler.description.IRestream<ReturnValueType>.output_to<OutputValueType>
            (global::System.Func<ReturnValueType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputValueManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }

        void handler.description.IRestream<ReturnValueType>.output_to<PublicHandlerType>
            (global::System.Func<PublicHandlerType> pPublicHandler, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputValueManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void handler.description.IRestream<ReturnValueType>.output_to<PublicHandlerType>
            (global::System.Func<string, PublicHandlerType> pPublicHandler,
            string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputValueManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        #region Information

        ulong IEchoReturn<ReturnValueType>.GetID() => ID;
        ulong IEchoReturn<ReturnValueType>.GetObjectID() => NodeAccess.GetID();
        ulong IEchoReturn<ReturnValueType>.GetNodeObjectID() => NodeAccess.GetNodeID();
        int IEchoReturn<ReturnValueType>.GetObjectAttachmentNodeNumberInSystem() => NodeAccess.GetAttachmentNodeNumberInSystem();
        int IEchoReturn<ReturnValueType>.GetObjectAttackmentNumberObjectInNode() => NodeAccess.GetAttackmentNumberObjectInNode();

        private static ulong UniqueID = 0;

        private static ulong GetUniqueID()
        {
            if (UniqueID == ulong.MaxValue) UniqueID = 0;

            return UniqueID++;
        }

        public handler.description.IAsyncRestream<ReturnValueType> async_output_to_echo<LocationEchoObjectType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
            where LocationEchoObjectType : main.Object, new()
        {
            return OutputValueManager.AddConnectingToAsyncEcho<ReceiveValueType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName, pPollSize, pTimeDelay, pPollName);
        }

        public handler.description.IAsyncRestream<ReturnValueType> async_output_to_echo
            (string pEchoName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            throw new System.NotImplementedException();
        }

        public handler.description.IAsyncRestream<ReturnValueType1> async_output_to_echo<LocationEchoObjectType, ReturnValueType1>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
            where LocationEchoObjectType : main.Object, new()
        {
            throw new System.NotImplementedException();
        }

        public handler.description.IRestream<ReturnValueType> output_to_echo<LocationEchoObjectType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : main.Object, new()
        {
            return OutputValueManager.AddConnectingToEcho<ReceiveValueType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        public handler.description.IRestream<ReturnValueType1> output_to_echo<LocationEchoObjectType, ReturnValueType1>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
            where LocationEchoObjectType : main.Object, new()
        {
            throw new System.NotImplementedException();
        }

        #endregion


    }
}

