namespace Butterfly.system.objects.main.objects.manager.action
{
    class Object<ParamValue1, ParamValue2> : main.Informing
    {
        public global::System.Action<ParamValue1, ParamValue2> Action { private set; get; }

        private readonly manager.events.Object<ParamValue1, ParamValue2> EventsManager;

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
            poll.description.access.add.IPoll pPollAccess,
                int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            : base("ActionManager_1", pInforming)
        {
            EventsManager = new manager.events.Object<ParamValue1, ParamValue2>();

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

        /// <summary>
        /// Из за пулов может зайти дразу 2 операции.
        /// Будем в echo записывать на какой операции мы остановились.
        /// И после возрата продолжать с того же места.
        /// </summary>
        /// <param name="pValue"></param>
        private void DefaultInput(ParamValue1 pValue1, ParamValue2 pValue2)
        {
            EventsManager.Run(pValue1, pValue2);
        }

        public void AddAction(global::System.Action<ParamValue1, ParamValue2> pAction, int pPollSize, int pTimeDelay, string pPollName)
        {
            if (StateInformation.__IsCreating || StateInformation.__IsOccurrence)
            {
                objects.action.Object<ParamValue1, ParamValue2> actionObject = new objects.action.Object<ParamValue1, ParamValue2>
                    (pAction, DependencyAccess, PollAccess, pPollSize, pTimeDelay, pPollName);

                EventsManager.Add(actionObject.ToInput);
            }
            else
                Exception(Ex.ActionValue.x10001, pAction.GetType().FullName);
        }
    }
}
