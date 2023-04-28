namespace Butterfly.system.objects.main.objects.sending.listener
{
    public class Object<ListenerValueType> : Informing, IInput<ListenerValueType>,
         objects.description.IRestream<ListenerValueType>
    {
        private readonly handler.manager.action.Object<ListenerValueType> InputActionManager;

        private readonly main.manager.handlers.description.access.add.IPrivate PrivateHandlersManager;
        private readonly main.manager.objects.description.access.get.IShared pSharedObjectsManager;

        public Object(IInforming pInforming,
            information.State pStateInformation, information.description.access.get.INode pNodeAccess,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerManager,
            main.manager.objects.description.access.get.IShared pSharedObjectsManager,
            poll.description.access.add.IPoll pPoll,
            main.description.access.add.IDependency pDependency) 
            : base("SendingListener_1", pInforming)
        {
            InputActionManager = new handler.manager.action.Object<ListenerValueType>
                (handler.manager.events.events.Type.Broker, pStateInformation, pNodeAccess, pPrivateHandlerManager, pSharedObjectsManager,
                pDependency, pInforming, pPoll);
        }

        void IInput<ListenerValueType>.ToInput(ListenerValueType pValue)
        {
            InputActionManager.Action.Invoke(pValue);
        }


        objects.description.IRestream<ListenerValueType> objects.description.IRestream<ListenerValueType>.output_to
            (global::System.Action<ListenerValueType> pAction, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            InputActionManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }

        public objects.description.IRestream<OutputValueType> output_to<OutputValueType>(global::System.Func<ListenerValueType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            throw new System.NotImplementedException();
        }

        public handler.description.IRestream output_to<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
            where PrivateHandlerType : main.Object, IInput, IInput<ListenerValueType>, handler.description.IRestream, handler.description.IRegisterInPoll, new()
        {
            return InputActionManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }

        public void output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where PublicHandlerType : Object, objects.description.IRestream, IInput, IInput<ListenerValueType>, new()
        {
            throw new System.NotImplementedException();
        }

        public void output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where PublicHandlerType : Object, objects.description.IRestream, IInput, IInput<ListenerValueType>, new()
        {
            throw new System.NotImplementedException();
        }

        public objects.description.IRestream<ListenerValueType> output_to_echo<LocationEchoObjectType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : Object, new()
        {
            throw new System.NotImplementedException();
        }

        public objects.description.IRestream<ReturnValueType> output_to_echo<LocationEchoObjectType, ReturnValueType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : Object, new()
        {
            throw new System.NotImplementedException();
        }
    }
}
