using Butterfly.system.objects.handler.description;
using Butterfly.system.objects.main;


namespace Butterfly.system.objects.handler.objects.echo
{
    public class Object<ValueType> : main.Informing,
        IInput<ValueType>, handler.description.IAsyncRestream<ValueType>, handler.description.IRestream<ValueType>,
        IEchoReturn<ValueType>
    {
        public enum Type
        {
            /// <summary>
            /// В обьекте который воспользовался Echo, не прервалась цепочка событий.
            /// Поэтому мы просто вернем значение в нужную точку.
            /// </summary>
            Continue = 0,
            /// <summary>
            /// В обьекте который воспользовался Echo, прервалась цепочка событий.
            /// Поэтому мы сначало вернем значение в нужную точку завершим событие начиная с данного места 
            /// и после завершим все остальные события.
            /// </summary>
            Break = 1 
        }

        private readonly manager.action.Object<ValueType> OutputValueManager;

        /// <summary>
        /// Место получения эхо сигнала(Прослушивателя).
        /// </summary>
        private readonly global::System.Action<ValueType, Object<ValueType>> ActionReceiveEcho;

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
        private readonly System.Action<int> ContinueExecutingEvents;
        private readonly int NumberOfTheInterruptedEvent; // Номер прерваного события.

        private readonly main.information.State StateInformation;
        private readonly main.information.description.access.get.INode NodeAccess;

        /// <summary>
        /// Уникальный ID обьекта.
        /// </summary>
        private readonly ulong ID;

        public Object(Type pActionReturnType, System.Action<int> pContinueExecutingEvents, int pNumberOfTheInterruptedEvent, 
            main.IInforming pInformingMainObject,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManagerAccess,
            main.manager.objects.description.access.get.IShared pSharedObjectsManagerAccess,
            main.information.State pStateInformation,
            main.information.description.access.get.INode pNodeAccess,
            poll.description.access.add.IPoll pPollAccess,
            main.description.access.add.IDependency pDependencyAccess,
            System.Action<ValueType, Object<ValueType>> pActionReceiveEcho)
            : base("EchoObject_1", pInformingMainObject)
        {
            ID = GetUniqueID();

            ActionReturnType = pActionReturnType;
            ContinueExecutingEvents = pContinueExecutingEvents;
            NumberOfTheInterruptedEvent = pNumberOfTheInterruptedEvent;

            OutputValueManager = new manager.action.Object<ValueType>(manager.events.events.Type.Broker, pStateInformation, pNodeAccess, pPrivateHandlerManagerAccess, pSharedObjectsManagerAccess,
                pDependencyAccess, pInformingMainObject, pPollAccess);

            NodeAccess = pNodeAccess;
            StateInformation = pStateInformation;

            ActionReceiveEcho = pActionReceiveEcho;
        }

        /// <summary>
        /// Оправить ответ, на ожидающий async.
        /// </summary>
        public void To(ValueType pValue)
        {
            // Продолжаем цепочку операций для async/await.
            OutputValueManager.Action.Invoke(pValue);
            
            if (ActionReturnType.HasFlag(Type.Break))
            {
                ContinueExecutingEvents(NumberOfTheInterruptedEvent);
            }
        }

        public void SafeTo(ValueType pValue)
        {
            lock (StateInformation.Locker)
            {
                if (StateInformation.StartProcess)
                {
                    // Продолжаем цепочку операций для async/await.
                    OutputValueManager.Action.Invoke(pValue);

                    if (ActionReturnType.HasFlag(Type.Break))
                    {
                        ContinueExecutingEvents(NumberOfTheInterruptedEvent);
                    }
                }
            }
        }

        /// <summary>
        /// Принять входящие данные для прослушивающего echo сюда.
        /// </summary>
        /// <param name="pValue"></param>
        void IInput<ValueType>.ToInput(ValueType pValue)
        {
            ActionReceiveEcho(pValue, this);
        }

        handler.description.IRestream<ValueType> handler.description.IAsyncRestream<ValueType>.await(global::System.Action<ValueType> pAction, 
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputValueManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }
        handler.description.IRestream<OutputValueType> handler.description.IAsyncRestream<ValueType>.await<OutputValueType>
            (global::System.Func<ValueType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputValueManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }
        handler.description.IRestream handler.description.IAsyncRestream<ValueType>.await<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
        {
            return OutputValueManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
        void handler.description.IAsyncRestream<ValueType>.await<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler, 
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
        {
            OutputValueManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }
        void handler.description.IAsyncRestream<ValueType>.await<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler,
            string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputValueManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        handler.description.IRestream<OutputValueType> handler.description.IRestream<ValueType>.output_to<OutputValueType>
            (global::System.Func<ValueType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputValueManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }
        handler.description.IRestream<ValueType> handler.description.IRestream<ValueType>.output_to(global::System.Action<ValueType> pAction, 
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputValueManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }
        handler.description.IRestream handler.description.IRestream<ValueType>.output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
        {
            return OutputValueManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
        void handler.description.IRestream<ValueType>.output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler, 
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
        {
            OutputValueManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void handler.description.IRestream<ValueType>.output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler,
            string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputValueManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        #region Information

        ulong IEchoReturn<ValueType>.GetID() => ID;
        ulong IEchoReturn<ValueType>.GetObjectID() => NodeAccess.GetID();
        ulong IEchoReturn<ValueType>.GetNodeObjectID() => NodeAccess.GetNodeID();
        int IEchoReturn<ValueType>.GetObjectAttachmentNodeNumberInSystem() => NodeAccess.GetAttachmentNodeNumberInSystem();
        int IEchoReturn<ValueType>.GetObjectAttackmentNumberObjectInNode() => NodeAccess.GetAttackmentNumberObjectInNode();

        private static ulong UniqueID = 0;

        private static ulong GetUniqueID()
        {
            if (UniqueID == ulong.MaxValue) UniqueID = 0;

            return UniqueID++;
        }

        public IAsyncRestream<ValueType> async_output_to_echo<LocationEchoObjectType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : Object, new()
        {
            throw new System.NotImplementedException();
        }

        public IAsyncRestream<ValueType> async_output_to_echo(string pEchoName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            throw new System.NotImplementedException();
        }

        public IAsyncRestream<ReturnValueType> async_output_to_echo<LocationEchoObjectType, ReturnValueType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : Object, new()
        {
            throw new System.NotImplementedException();
        }

        public IRestream<ValueType> output_to_echo<LocationEchoObjectType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : Object, new()
        {
            throw new System.NotImplementedException();
        }

        public IRestream<ReturnValueType> output_to_echo<LocationEchoObjectType, ReturnValueType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : Object, new()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}

