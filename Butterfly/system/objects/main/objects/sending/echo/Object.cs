using System;

namespace Butterfly.system.objects.main.objects.sending.echo
{
    public class Object<ReceiveType, ReturnType> : main.Informing,
        IInput, IInput<ReceiveType>, IEchoReturn<ReturnType>, objects.description.IRestream<ReturnType>
    {
        /// <summary>
        /// Это место прияма данных у прослушивающего echo.
        /// </summary>
        private readonly global::System.Action<ReceiveType, IEchoReturn<ReturnType>> ActionReceiveEcho;

        private readonly objects.manager.action.Object<ReturnType> OutputActionManager;

        private readonly main.information.State StateInformation;
        private readonly main.information.description.access.get.INode NodeAccess;

        private readonly ulong ID;

        public Object(main.IInforming pInforming,
            main.manager.handlers.description.access.add.IPrivate pPrivateHandlerAccess,
            main.manager.objects.description.access.get.IShared pSharedObjectsAccess,
            main.information.State pStateInformation,
            main.information.description.access.get.INode pNodeAccess,
            poll.description.access.add.IPoll pPollAccess,
            main.description.access.add.IDependency pDependencyAccess,
            global::System.Action<ReceiveType, IEchoReturn<ReturnType>> pActionReceiveEcho)
            : base("EchoObject_2", pInforming)
        {
            ID = GetUniqueID();

            StateInformation = pStateInformation;

            ActionReceiveEcho = pActionReceiveEcho;

            OutputActionManager = new objects.manager.action.Object<ReturnType>
                (pStateInformation, pNodeAccess, pPrivateHandlerAccess, pSharedObjectsAccess, pDependencyAccess, pInforming, pPollAccess);
        }

        public void SafeTo(ReturnType pValue)
        {
            lock (StateInformation.Locker)
            {
                if (StateInformation.StartProcess)
                {
                }
            }
        }

        public void To(ReturnType pValue)
        {
            lock (StateInformation.Locker)
            {
                if (StateInformation.IsStarting || StateInformation.StartProcess && StateInformation.IsDestroy == false)
                {
                    OutputActionManager.Action.Invoke(pValue);
                }
            }
        }

        /// <summary>
        /// Входящие данные через этот метод отправляются в прослушивающий echo.
        /// </summary>
        /// <param name="pValue"></param>
        void IInput.ToInput<ParamValueType>(ParamValueType pValue)
        {
            if (ActionReceiveEcho is System.Action<ParamValueType, Object<ReceiveType, ReturnType>> actionReceiveEchoReduse)
            {
                actionReceiveEchoReduse.Invoke(pValue, this);
            }
            else
                Exception(Ex.EchoReturn.x10001, typeof(ReceiveType).FullName, typeof(ParamValueType).FullName);
        }

        void IInput<ReceiveType>.ToInput(ReceiveType pValue)
        {
            ActionReceiveEcho.Invoke(pValue, this);
        }

        ulong IEchoReturn<ReturnType>.GetID() => ID;
        ulong IEchoReturn<ReturnType>.GetObjectID() => NodeAccess.GetID();
        ulong IEchoReturn<ReturnType>.GetNodeObjectID() => NodeAccess.GetNodeID();
        int IEchoReturn<ReturnType>.GetObjectAttachmentNodeNumberInSystem() => NodeAccess.GetAttachmentNodeNumberInSystem();
        int IEchoReturn<ReturnType>.GetObjectAttackmentNumberObjectInNode() => NodeAccess.GetAttackmentNumberObjectInNode();

        private static ulong UniqueID = 0;

        private static ulong GetUniqueID()
        {
            if (UniqueID == ulong.MaxValue) UniqueID = 0;

            return UniqueID++;
        }

        public objects.description.IRestream<OutputValueType> output_to<OutputValueType>
            (global::System.Func<ReturnType, OutputValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }

        public objects.description.IRestream<ReturnType> output_to
            (global::System.Action<ReturnType> pAction, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }

        handler.description.IRestream objects.description.IRestream<ReturnType>.output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }

        void objects.description.IRestream<ReturnType>.output_to<PublicHandlerType>
            (global::System.Func<PublicHandlerType> pPublicHandler, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") 
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void objects.description.IRestream<ReturnType>.output_to<PublicHandlerType>
            (global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName, int pPollSize = 0, 
                int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        objects.description.IRestream<ReturnType> objects.description.IRestream<ReturnType>.output_to_echo<LocationEchoObjectType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<ReceiveType, ReturnType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveType).FullName + typeof(ReturnType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        objects.description.IRestream<ReturnValueType> objects.description.IRestream<ReturnType>.output_to_echo<LocationEchoObjectType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            /*
            return OutputActionManager.AddConnectingToEcho<OutputType, OutputType>
                    (typeof(LocationEchoObjectType).FullName + typeof(OutputType).FullName + typeof(OutputType).FullName,
                    pPollSize, pTimeDelay, pPollName);
            */
            throw new NotImplementedException();
        }
    }
}

