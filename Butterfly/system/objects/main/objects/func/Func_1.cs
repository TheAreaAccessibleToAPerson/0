namespace Butterfly.system.objects.main.objects.func
{
    public class Object<InputType, OutputType> : main.Informing, description.IRestream, 
        description.IRestream<OutputType>, IInput<InputType>
    {
        private readonly global::System.Func<InputType, OutputType> Function;

        private readonly manager.action.Object<OutputType> OutputActionManager;

        private readonly main.manager.handlers.description.access.add.IPrivate PrivateHandlersManager;
        private readonly main.manager.objects.description.access.get.IShared SharedObjectsManager;

        public readonly string Name;

        public Object(global::System.Func<InputType, OutputType> pFunc,
            main.IInforming pInforming,
            main.information.State pStateInformation, main.information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManager,
            main.manager.objects.description.access.get.IShared pSharedObjectsManager,
            poll.description.access.add.IPoll pPoll,
            main.description.access.add.IDependency pDependency)
            : base("FuncObjects_1", pInforming)
        {
            Function = pFunc;

            Name = pFunc.GetType().FullName;

            OutputActionManager = new manager.action.Object<OutputType>
                (pStateInformation, pNodeAccess, pPrivateHandlerManager, pSharedObjectsManager, pDependency, pInforming, pPoll);

            PrivateHandlersManager = pPrivateHandlerManager;
            SharedObjectsManager = pSharedObjectsManager;
        }

        #region Input

        public void ToInput(InputType pValue)
        {
            OutputActionManager.Action(Function.Invoke(pValue));
        }

        #endregion

        description.IRestream<OutputValueType> description.IRestream<OutputType>.output_to<OutputValueType>
            (global::System.Func<OutputType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<OutputType> description.IRestream<OutputType>.output_to
            (global::System.Action<OutputType> pAction, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }

        void description.IRestream<OutputType>.output_to<PublicHandlerType>
            (global::System.Func<PublicHandlerType> pPublicHandler, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream<OutputType>.output_to<PublicHandlerType>
            (global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, 
            string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
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

        description.IRestream description.IRestream.output_to<ParamValueType>
            (global::System.Action<ParamValueType> pAction, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddActionIsType(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }

        description.IRestream<ReturnValueType> description.IRestream.output_to<ParamValueType, ReturnValueType>
            (global::System.Func<ParamValueType, ReturnValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFuncIsType(pFunc, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream.output_to<PublicHandlerType>
            (global::System.Func<PublicHandlerType> pPublicHandler, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream.output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, 
            string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ReceiveValueType> description.IRestream.output_to_echo<LocationEchoObjectType, ReceiveValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<OutputType, ReceiveValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(OutputType).FullName + typeof(ReceiveValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ReturnValueType> description.IRestream.output_to_echo<LocationEchoObjectType, ReceiveValueType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<ReceiveValueType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        handler.description.IRestream description.IRestream<OutputType>.output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
        {
            return OutputActionManager.AddPrivateHandlerIsType<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }

        handler.description.IRestream description.IRestream.output_to<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddPrivateHandlerIsType<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
    }
}
