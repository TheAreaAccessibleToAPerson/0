namespace Butterfly.system.objects.main.objects.receive.echo
{
    public class Object<ReceiveValueType, ReturnValueType> : Informing, 
        IInput<ReceiveValueType, IEchoReturn<ReturnValueType>>,
        description.IRestream<ReceiveValueType, IEchoReturn<ReturnValueType>>, 
        description.access.get.IInformationCreatingObject
    {   
        private readonly manager.events.Object<ReceiveValueType, IEchoReturn<ReturnValueType>> EventsManager;

        private readonly poll.description.access.add.IPoll PollAccess;
        private readonly main.description.access.add.IDependency Dependency;

        /// <summary>
        /// Хранит ID пространсва(NodeObject) в нутри которого был создан данный обьект.
        /// </summary>
        public readonly ulong CreatorNodeObjectID;
        /// <summary>
        /// Хранит ID обьекта в нутри которого был создан данный обьект.
        /// </summary>
        public readonly ulong CreatorObjectID;
        /// <summary>
        /// Хранит Explorer обьекта в нутри которого был создан данный обьект.
        /// </summary>
        public readonly string CreatorObjectExplorer;

        public Object(IInforming pInforming, information.State pStateInforming, poll.description.access.add.IPoll pPollAccess, 
            string pCreatorObjectExplorer, ulong pCreatorObjectID, ulong pCreatorNodeObjectID)
            : base("ReceiveEcho_2", pInforming)
        {
            PollAccess = pPollAccess;

            CreatorNodeObjectID = pCreatorNodeObjectID;
            CreatorObjectID = pCreatorObjectID;
            CreatorObjectExplorer = pCreatorObjectExplorer;

            EventsManager = new manager.events.Object<ReceiveValueType, IEchoReturn<ReturnValueType>>();
        }

        string description.access.get.IInformationCreatingObject.GetExplorerObject()
        {
            return CreatorObjectExplorer;
        }

        public void ToInput(ReceiveValueType pValue1, IEchoReturn<ReturnValueType> pValue2)
        {
            EventsManager.Run(pValue1, pValue2);
        }

        description.IRestream<ReceiveValueType, IEchoReturn<ReturnValueType>> description.IRestream<ReceiveValueType, IEchoReturn<ReturnValueType>>.output_to
            (global::System.Action<ReceiveValueType, IEchoReturn<ReturnValueType>> pAction, int pPollSize, int pTimeDelay, string pPollName)
        {
            action.Object<ReceiveValueType, IEchoReturn<ReturnValueType>> actionObject = new action.Object<ReceiveValueType, IEchoReturn<ReturnValueType>>
                (pAction, Dependency, PollAccess, pPollSize, pTimeDelay, pPollName);

            EventsManager.Add(actionObject.ToInput);

            return this;
        }

        public void To(ReturnValueType pValue)
        {
            throw new System.NotImplementedException();
        }

        public void SafeTo(ReturnValueType pValue)
        {
            throw new System.NotImplementedException();
        }

        public int GetObjectAttachmentNodeNumberInSystem()
        {
            throw new System.NotImplementedException();
        }

        public int GetObjectAttackmentNumberObjectInNode()
        {
            throw new System.NotImplementedException();
        }

        public string GetObjectExplorer()
        {
            return CreatorObjectExplorer;
        }

        public ulong GetIDNodeObject()
        {
            return CreatorNodeObjectID;
        }

        public ulong GetIDObject()
        {
            return CreatorObjectID;
        }
    }
}
