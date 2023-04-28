using System;

namespace Butterfly.system.objects.main.objects.listen
{
    public class Object<ListenerValueType> : Informing, IInput<ListenerValueType>,
        description.IRestream, description.IRestream<ListenerValueType>,
        main.objects.description.access.get.IInformationCreatingObject
    {
        private readonly manager.action.Object<ListenerValueType> InputActionManager;

        public readonly string ListenMessageType;

        private readonly string CreatorExplorer;
        private readonly ulong CreatorNodeID;
        private readonly ulong CreatorObjectID;

        public Object(string pCreatorExplorer, ulong pCreatorNodeID, ulong pCreatorObjectID, IInforming pInforming,
            information.State pStateInformation, information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManager,
            main.manager.objects.description.access.get.IShared pSharedObjectsManager,
            poll.description.access.add.IPoll pPoll,
            main.description.access.add.IDependency pDependency)
            : base("SendingListener_1", pInforming)
        {
            CreatorExplorer = pCreatorExplorer;
            CreatorNodeID = pCreatorNodeID;
            CreatorObjectID = pCreatorObjectID;

            ListenMessageType = typeof(ListenerValueType).FullName;

            InputActionManager = new manager.action.Object<ListenerValueType>
                (pStateInformation, pNodeAccess, pPrivateHandlerManager, pSharedObjectsManager,
                pDependency, pInforming, pPoll);
        }

        public string GetExplorerObject() => CreatorExplorer;
        public ulong GetIDNodeObject() => CreatorNodeID;
        public ulong GetIDObject() => CreatorObjectID;

        void IInput<ListenerValueType>.ToInput(ListenerValueType pValue)
        {
            InputActionManager.Action.Invoke(pValue);
        }

        description.IRestream description.IRestream.output_to<ParamValueType>
            (global::System.Action<ParamValueType> pAction, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            InputActionManager.AddActionIsType(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }

        description.IRestream<ReturnValueType> description.IRestream.output_to<ParamValueType, ReturnValueType>
            (global::System.Func<ParamValueType, ReturnValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddFuncIsType(pFunc, pPollSize, pTimeDelay, pPollName);
        }

        handler.description.IRestream description.IRestream.output_to<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddPrivateHandlerIsType<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream.output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler, 
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            InputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream.output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, 
            string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            InputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ReceiveValueType> description.IRestream.output_to_echo<LocationEchoObjectType, ReceiveValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddConnectingToEcho<ListenerValueType, ReceiveValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ListenerValueType).FullName + typeof(ReceiveValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ReturnValueType> description.IRestream.output_to_echo<LocationEchoObjectType, ReceiveValueType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddConnectingToEcho<ReceiveValueType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<OutputValueType> description.IRestream<ListenerValueType>.output_to<OutputValueType>
            (global::System.Func<ListenerValueType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ListenerValueType> description.IRestream<ListenerValueType>.output_to(global::System.Action<ListenerValueType> pAction, 
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            InputActionManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }

        handler.description.IRestream description.IRestream<ListenerValueType>.output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddPrivateHandlerIsType<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream<ListenerValueType>.output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler, 
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            InputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream<ListenerValueType>.output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, 
            string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            InputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ListenerValueType> description.IRestream<ListenerValueType>.output_to_echo<LocationEchoObjectType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddConnectingToEcho<ListenerValueType, ListenerValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ListenerValueType).FullName + typeof(ListenerValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<ReturnValueType> description.IRestream<ListenerValueType>.output_to_echo<LocationEchoObjectType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddConnectingToEcho<ListenerValueType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ListenerValueType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }
    }
}
