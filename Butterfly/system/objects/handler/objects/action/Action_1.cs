using Butterfly.system.objects.handler.description;
using System;

namespace Butterfly.system.objects.handler.objects.action
{
    public class Object<ParamType> : main.Informing, IInput<ParamType>, IInput,
        description.IRestream<ParamType>, description.IRestream
    {
        private global::System.Action<ParamType> Action, Event;

        private readonly manager.action.Object<ParamType> OutputActionManager;

        // Из Action невозможно вывести никакие данные. Поэтому для работы в составе пулла
        // нужен будликат.

        private readonly main.description.access.add.IDependency Dependency;

        private readonly poll.description.access.add.IPoll PollAccess;

        public readonly string Name;

        public Object(global::System.Action<ParamType> pAction,
            main.IInforming pInforming,
            main.information.State pStateInformation, main.information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManager,
            main.manager.objects.description.access.get.IShared pSharedObjectsManager,
            poll.description.access.add.IPoll pPollAccess,
            main.description.access.add.IDependency pDependency, int pPollSize, int pTimeDelay, string pPollName)
            : base("ActionObjects_1", pInforming)
        {
            PollAccess = pPollAccess;

            Name = Hellper.GetName(pAction.Method);

            Dependency = pDependency;

            Action = pAction;
            Event = DefaultInput;

            OutputActionManager = new manager.action.Object<ParamType>(manager.events.events.Type.Broker,
                pStateInformation, pNodeAccess, pPrivateHandlerManager, pSharedObjectsManager, pDependency, pInforming, pPollAccess);

            RegisterInPoll(pPollSize, pTimeDelay, pPollName);
        }

        public Object(global::System.Action<ParamType> pAction,
            global::System.Action<int> pContinueExecutingEvents, int pNumberOfTheInterruptedEvent,
            main.IInforming pInforming,
            main.information.State pStateInformation, main.information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManager,
            main.manager.objects.description.access.get.IShared pSharedObjectsManager,
            poll.description.access.add.IPoll pPollAccess,
            main.description.access.add.IDependency pDependency, int pPollSize, int pTimeDelay, string pPollName)
            : base("ActionObjects_1", pInforming)
        {
            PollAccess = pPollAccess;

            Name = Hellper.GetName(pAction.Method);

            Dependency = pDependency;

            Action = pAction;
            Event = DefaultInput;

            OutputActionManager = new manager.action.Object<ParamType>(manager.events.events.Type.Broker,
                pContinueExecutingEvents, pNumberOfTheInterruptedEvent,
                    pStateInformation, pNodeAccess, pPrivateHandlerManager, pSharedObjectsManager, pDependency, pInforming, pPollAccess);

            RegisterInPoll(pPollSize, pTimeDelay, pPollName);
        }

        public void ToInput(ParamType pValue)
        {
            Event.Invoke(pValue);
        }

        public void ToInput<InputValueType>(InputValueType pValue)
        {
            if (pValue is ParamType valueReduse)
            {
                Event.Invoke(valueReduse);
            }
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
                Event = ValuesSafeCollection.Add;
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
            Action.Invoke(pValue);

            OutputActionManager.Action.Invoke(pValue);
        }

        description.IRestream<OutputValueType> description.IRestream<ParamType>.output_to<OutputValueType>
           (global::System.Func<ParamType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream<ParamType> description.IRestream<ParamType>.output_to(global::System.Action<ParamType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream description.IRestream<ParamType>.output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
        void description.IRestream<ParamType>.output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream<ParamType>.output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler,
            string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream description.IRestream.output_to<InputValueType>(global::System.Action<InputValueType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddActionIsType(pAction, pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream<OutputValueType> description.IRestream.output_to<InputValueType, OutputValueType>
            (global::System.Func<InputValueType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFuncIsType(pFunc, pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream description.IRestream.output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddPrivateHandlerIsType<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
        void description.IRestream.await<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream.await<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream description.IRestream.await<ParamValueType>(global::System.Action<ParamValueType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddActionIsType(pAction, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ReturnValueType> description.IRestream.await<ReceiveValueType, ReturnValueType>
            (global::System.Func<ReceiveValueType, ReturnValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFuncIsType(pFunc, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream description.IRestream.await<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddPrivateHandlerIsType<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }

        description.IAsyncRestream<ReceiveValueType> description.IRestream.async_output_to_echo<LocationEchoObjectType, ReceiveValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<ReceiveValueType, ReceiveValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReceiveValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IAsyncRestream<ReceiveValueType> description.IRestream.async_output_to_echo
            <LocationEchoObjectType, ReceiveValueType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<ReceiveValueType, ReceiveValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }
        description.IAsyncRestream<ReceiveValueType> description.IRestream.async_output_to_echo<ReceiveValueType>
            (string pEchoName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<ReceiveValueType, ReceiveValueType>
                    (pEchoName, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ValueType> description.IRestream.output_to_echo<LocationEchoObjectType, ValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<ValueType, ValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ValueType).FullName + typeof(ValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ReturnValueType> description.IRestream.output_to_echo<LocationEchoObjectType, ReceiveValueType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<ReceiveValueType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream.output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream.output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler,
            string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        description.IAsyncRestream<ParamType> description.IRestream<ParamType>.async_output_to_echo<LocationEchoObjectType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<ParamType, ParamType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ParamType).FullName + typeof(ParamType).FullName, pPollSize, pTimeDelay, pPollName);
        }

        description.IAsyncRestream<ParamType> description.IRestream<ParamType>.async_output_to_echo
            (string pEchoName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<ParamType, ParamType>
                    (pEchoName, pPollSize, pTimeDelay, pPollName);
        }

        description.IAsyncRestream<ReturnValueType> description.IRestream<ParamType>.async_output_to_echo<LocationEchoObjectType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<ParamType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ParamType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ParamType> description.IRestream<ParamType>.output_to_echo<LocationEchoObjectType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<ParamType, ParamType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ParamType).FullName + typeof(ParamType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ReturnValueType> description.IRestream<ParamType>.output_to_echo<LocationEchoObjectType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<ParamType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ParamType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }
    }
}
