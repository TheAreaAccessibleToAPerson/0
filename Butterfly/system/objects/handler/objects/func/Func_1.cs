namespace Butterfly.system.objects.handler.objects.func
{
    class Object<InputType, OutputType> : main.Informing, description.IRestream<OutputType>, description.IRestream
    {
        private readonly global::System.Func<InputType, OutputType> Function;

        private readonly manager.action.Object<OutputType> OutputActionManager;

        public readonly string Name;

        public Object(global::System.Func<InputType, OutputType> pFunc,
            main.IInforming pInforming,
            main.information.State pStateInformation, main.information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManager,
            main.manager.objects.description.access.get.IShared pSharedObjectsManager,
            poll.description.access.add.IPoll pPoll,
            main.description.access.add.IDependency pDependency, int pPollSize, int pTimeDelay, string pPollName) 
            : base("FuncObjects_1", pInforming)
        {
            Function = pFunc;

            Name = pFunc.GetType().FullName;

            OutputActionManager = new manager.action.Object<OutputType>(manager.events.events.Type.Broker, 
                pStateInformation, pNodeAccess, pPrivateHandlerManager, pSharedObjectsManager, pDependency, pInforming, pPoll,
                    pPollSize, pTimeDelay, pPollName);
        }

        public Object(global::System.Func<InputType, OutputType> pFunc,
            global::System.Action<int> pContinueExecutingEvents, int pNumberOfTheInterruptedEvent,
            main.IInforming pInforming,
            main.information.State pStateInformation, main.information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManager,
            main.manager.objects.description.access.get.IShared pSharedObjectsManager,
            poll.description.access.add.IPoll pPoll,
            main.description.access.add.IDependency pDependency, int pPollSize, int pTimeDelay, string pPollName)
            : base("FuncObjects_1", pInforming)
        {
            Function = pFunc;

            Name = pFunc.GetType().FullName;

            OutputActionManager = new manager.action.Object<OutputType>(manager.events.events.Type.Broker,
                pContinueExecutingEvents, pNumberOfTheInterruptedEvent,
                    pStateInformation, pNodeAccess, pPrivateHandlerManager, pSharedObjectsManager, pDependency, pInforming, pPoll,
                        pPollSize, pTimeDelay, pPollName);
        }


        #region Input

        public void ToInput(InputType pValue)
        {
            OutputActionManager.Action(Function.Invoke(pValue));
        }

        #endregion

        #region Output

        description.IRestream<OutputValueType> description.IRestream<OutputType>.output_to<OutputValueType>
            (global::System.Func<OutputType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream<OutputType> description.IRestream<OutputType>.output_to(global::System.Action<OutputType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }
        description.IRestream description.IRestream<OutputType>.output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
        void description.IRestream<OutputType>.output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler, 
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream<OutputType>.output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, 
            string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }


        description.IRestream description.IRestream.output_to<InputValueType>(global::System.Action<InputValueType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddActionIsType(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
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
            OutputActionManager.AddActionIsType(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
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

        description.IAsyncRestream<OutputType> description.IRestream<OutputType>.async_output_to_echo<LocationEchoObjectType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<OutputType, OutputType>
                    (typeof(LocationEchoObjectType).FullName + typeof(OutputType).FullName + typeof(OutputType).FullName, pPollSize, pTimeDelay, pPollName);
        }

        description.IAsyncRestream<OutputType> description.IRestream<OutputType>.async_output_to_echo
            (string pEchoName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<OutputType, OutputType>
                    (pEchoName,pPollSize, pTimeDelay, pPollName);
        }

        description.IAsyncRestream<ReturnValueType> description.IRestream<OutputType>.async_output_to_echo<LocationEchoObjectType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<OutputType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(OutputType).FullName + typeof(ReturnValueType).FullName, 
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<OutputType> description.IRestream<OutputType>.output_to_echo<LocationEchoObjectType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<OutputType, OutputType>
                    (typeof(LocationEchoObjectType).FullName + typeof(OutputType).FullName + typeof(OutputType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ReturnValueType> description.IRestream<OutputType>.output_to_echo<LocationEchoObjectType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<OutputType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(OutputType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        #endregion
    }
}
