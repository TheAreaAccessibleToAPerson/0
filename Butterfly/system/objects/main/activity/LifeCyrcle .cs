namespace Butterfly.system.objects.main.activity
{
    public class LifeCyrcle : Informing
    {
        /// <summary>
        /// Контроль жизненых цилов обьекта, который отвечает за запуск всего что в нем определено.
        /// Каждый обьект будь то это локальный обьект, или же Breach обьект, или же Node обьект будет содержать свою жизненую карту.
        /// В случае возникновения проблем этот обьект просто пройдется в обратном напровлении с места до которого смог добратся.
        /// </summary>

        private readonly information.Header HeaderInformation;

        private readonly information.State StateInformation;
        private readonly information.State.Manager StateInformationManager;
        private readonly information.Node NodeInformation;
        private readonly information.Tegs TegsInformation;

        private readonly manager.objects.Main MainObjectsManager;
        private readonly manager.objects.Local LocalObjectsManager;
        private readonly manager.objects.Global GlobalObjectsManager;
        private readonly manager.handlers.Public PublicHandlersManager;
        private readonly manager.handlers.Private PrivateHandlersManager;

        private readonly manager.control.Subscription SubscriptionControlManager;

        /// <summary>
        /// Если обьект не использовал системный метод, то мы запишем его сюда.
        /// </summary>
        private string CurrentCallMethod = "";

        public struct SystemMethod
        {
            public const string CONSTRUCTION = "Contruction";
            public const string CONFIGURATE = "Configurate";
            public const string START = "Start";
            public const string STOP = "Stop";
        }

        /// <summary>
        /// Если обьет не использовал данный системный метод , то и информирвать о его запуске не нужно.
        /// </summary>
        /// <param name="pMethodName"></param>
        public void SetNotOverrideCallMethod(string pMethodName) => CurrentCallMethod = pMethodName;

        public LifeCyrcle(IInforming pInforming, information.Header pHeaderInformation, information.State pStateInformation, 
            information.State.Manager pStateInformationManager, information.Node pNodeInformation, information.Tegs pTagsInformation, manager.objects.Main pMainObjectsManager, 
            manager.objects.Local pLocalObjectsManager, manager.objects.Global pGlobalObjectsManager, manager.handlers.Public pPublicHandlersManager, 
            manager.handlers.Private pPrivateHandlersManager,  manager.control.Subscription pSubscriptionControlManager)
                : base("LifeCyrcleActivity", pInforming)
        {
            HeaderInformation = pHeaderInformation;
            StateInformation = pStateInformation;
            StateInformationManager = pStateInformationManager;
            NodeInformation = pNodeInformation;
            TegsInformation = pTagsInformation;

            MainObjectsManager = pMainObjectsManager;
            LocalObjectsManager = pLocalObjectsManager;
            GlobalObjectsManager = pGlobalObjectsManager;
            PublicHandlersManager = pPublicHandlersManager;
            PrivateHandlersManager = pPrivateHandlersManager;

            SubscriptionControlManager = pSubscriptionControlManager;
        }

        private void Information(System.Tuple<string, bool>[] pResult, 
            string pInformationTypeObject, string pInformationCyrcleLiveType)
        {
            if (pResult != null)
                foreach (var result in pResult)
                {
                    if (result.Item2)
                        SystemInformation($"[{pInformationTypeObject}:{result.Item1}:{pInformationCyrcleLiveType}:access]", System.ConsoleColor.Yellow);
                    else
                        SystemInformation($"[{pInformationTypeObject}:{result.Item1}:{pInformationCyrcleLiveType}:error]", System.ConsoleColor.Cyan);
                }    
        }

        private void Information(bool pResult, string pInformationTypeObject, string pInformationCyrcleLiveType)
        {
            if (pResult)
                SystemInformation($"[{pInformationTypeObject}:{pInformationCyrcleLiveType}:access]", System.ConsoleColor.Yellow);
            else
                SystemInformation($"[{pInformationTypeObject}:{pInformationCyrcleLiveType}:error]", System.ConsoleColor.Cyan);
        }


        /// <summary>
        /// Предназначен для установки связи между обьектами.
        /// Если эта связнь с глобальными обьектами, то начало работы с ними произайдет позже.
        /// Случае возникновения проблем произайдет оповещение NodeObject в котором изменится его текущий статус работы.
        /// 
        /// 1)Создаение Independent обьекта.
        /// Проверяем что в момент построение связей он был в состоянии OCCURRENCE которое выставляется при определение его 
        /// данные в нутри системы и при определении его сущности. При создании обьекта его родитель влючит счетчик подсчета обьектов
        /// которые были отданы на создание в системный обработчик, для того что бы дождатся пока все переданые обькты запустились.
        /// После чего прекратит их работу.
        /// </summary>
        /// <param name="pContructionMainObject"></param>
        public void Construction(global::System.Action pContructionMainObject)
        {
            if (!NodeInformation.NodeObject.StateInformation.IsStopping && !StateInformation.IsStopping &&
                    !NodeInformation.NodeObject.StateInformation.IsStop && !StateInformation.IsStop)
            {
                if (StateInformationManager.FlagReplase(information.State.Data.CREATING, information.State.Data.OCCURRENCE))
                {
                    //Абстрактный метод MainObject.
                    pContructionMainObject.Invoke();
                    {
                        if (StateInformation.IsDestroy == false && NodeInformation.NodeObject.StateInformation.IsDestroy == false)
                        {
                            // Если конструтор вызвался без проблем и все связи устрановились продвигаемся по карте.
                            //...

                            Information(true, "current object", SystemMethod.CONSTRUCTION);
                        }
                        else
                        {
                            Information(false, "current object", SystemMethod.CONSTRUCTION);

                            return;
                        }
                    }

                    var resultLocalObjectAfterConfigurate = LocalObjectsManager.AfterLifeCyrcleMainObject<ILocalObject.Construction>();
                    {
                        Information(resultLocalObjectAfterConfigurate, "local object", "After.Construction()");

                        if (StateInformation.IsDestroy == false && NodeInformation.NodeObject.StateInformation.IsDestroy == false)
                        {
                            // Все методы Contruction в локальных обьектах запустились, двигаемся дальше.
                            //...
                        }
                        else
                        {
                            // Если возникли проблемы, то мы не зависываем имя локального обьекта на котором произошел обрыв.
                            // Так как конструктор в локальных обьектах не предназначен для серьезных задач.
                            return;
                        }
                    }

                    var resultCreatingMainObjects = MainObjectsManager.LifeCyrcle(information.State.Data.CREATING);
                    {
                        Information(resultCreatingMainObjects, "controller", "Creating");

                        if (StateInformation.IsDestroy == false && NodeInformation.NodeObject.StateInformation.IsDestroy == false)
                        {
                            // Все связки в конструторе каждого приватного обработчика установились без проблем, двигаемся дальше.
                            //...
                        }
                        else
                        {
                            // Если возникли проблемы то их решение будет лежать в нутри конкретного приватного обработчика.
                            return;
                        }
                    }

                    var resultPublicHandlerCreating = PublicHandlersManager.LifeCyrcle(information.State.Data.CREATING);
                    {
                        Information(resultPublicHandlerCreating, "public handler", "creating");

                        if (StateInformation.IsDestroy == false && NodeInformation.NodeObject.StateInformation.IsDestroy == false)
                        {
                            // Все связки в конструторе каждого публичного обработчика установились без проблем, двигаемся дальше.
                            //...
                        }
                        else
                        {
                            // Если возникли проблемы то их решение будет лежать в нутри конкретного публичного обработчика.
                            return;
                        }
                    }

                    var resultPrivateHandlerCreating = PrivateHandlersManager.LifeCyrcle(information.State.Data.CREATING);
                    {
                        Information(resultPrivateHandlerCreating, "private handler", "creating");

                        if (StateInformation.IsDestroy == false && NodeInformation.NodeObject.StateInformation.IsDestroy == false)
                        {
                            // Все связки в конструторе каждого публичного обработчика установились без проблем, двигаемся дальше.
                            //...
                        }
                        else
                        {
                            // Если возникли проблемы то их решение будет лежать в нутри конкретного публичного обработчика.
                            return;
                        }
                    }

                    
                    //Все подписки на обьекты, на пулы происходят через Node обьект.
                    if (HeaderInformation.IsNodeObject())
                    {
                        if (StateInformation.IsDestroy == false)
                        {
                            string[] subscribesName = SubscriptionControlManager.StartSubscribe();

                            foreach (var name in subscribesName)
                            {
                                Information(true, $"node object[{name}]", "StartSubscribe()");
                            }
                        }
                        else
                            return;
                    }
                }
                else
                    Exception(Ex.MainObject.x10008, "Construction()", information.State.Data.OCCURRENCE, StateInformation.Get());
            }
        }


        /// <summary>
        /// Если запустился метод Start значит все связки установлены. 
        /// </summary>
        /// <param name="pStartMainObject"></param>
        /// <param name="pConfigurateMainObject"></param>
        public void Start(global::System.Action pStartMainObject, global::System.Action pConfigurateMainObject)
        {
            if (StateInformationManager.FlagReplase(information.State.Data.CONFIGURATE, information.State.Data.CREATING))
            {
                var returnMainObjectsStarting = MainObjectsManager.LifeCyrcle(information.State.Data.STARTING);
                {
                    Information(returnMainObjectsStarting, "controller", "starting");

                    if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                        || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false 
                            && StateInformation.IsDestroy == false))
                    {
                        // Если node обьект преостановил процесс конфигурации в методе Configurate() вызовом метода destroy(), то нам незачем
                        // создовать его Branch обьекты.
                    }
                    else
                        return;
                }

                var resultPublicHandlerStarting = PublicHandlersManager.LifeCyrcle(information.State.Data.STARTING);
                {
                    Information(resultPublicHandlerStarting, "public handler", "starting");

                    if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                        || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false
                            && StateInformation.IsDestroy == false))
                    {
                        // Если node обьект преостановил процесс конфигурации в методе Configurate() вызовом метода destroy(), то нам незачем
                        // создовать его Branch обьекты.
                    }
                    else
                        return;
                }

                var resultPrivateHandlerStarting = PrivateHandlersManager.LifeCyrcle(information.State.Data.STARTING);
                {
                    Information(resultPrivateHandlerStarting, "private handler", "starting");

                    if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                        || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false
                            && StateInformation.IsDestroy == false))
                    {
                        // Если node обьект преостановил процесс конфигурации в методе Configurate() вызовом метода destroy(), то нам незачем
                        // создовать его Branch обьекты.
                    }
                    else
                        return;
                }

                var resultLocalObjectBeforeConfigurate = LocalObjectsManager.BeforeLifeCyrcleMainObject<ILocalObject.Configurate, ILocalObject.Configurate.Before>();
                {
                    Information(resultLocalObjectBeforeConfigurate, "local object", "Before.Configurate()");

                    if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                        || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false
                            && StateInformation.IsDestroy == false))
                    {
                        // Все методы Contruction в локальных обьектах запустились, двигаемся дальше.
                        //...
                    }
                    else
                    {
                        // Если возникли проблемы, то мы не зависываем имя локального обьекта на котором произошел обрыв.
                        // Так как конструктор в локальных обьектах не предназначен для серьезных задач.
                        return;
                    }
                }

                pConfigurateMainObject.Invoke();
                {
                    if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                        || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false
                            && StateInformation.IsDestroy == false))
                    {
                        if (CurrentCallMethod != SystemMethod.CONFIGURATE)
                            Information(true, "current object", SystemMethod.CONFIGURATE);
                    }
                    else
                    {
                        if (CurrentCallMethod != SystemMethod.CONFIGURATE)
                            Information(false, "current object", SystemMethod.CONFIGURATE);

                        return;
                    }
                }

                var resultLocalObjectAfterConfigurate = LocalObjectsManager.AfterLifeCyrcleMainObject<ILocalObject.Configurate.After>();
                {
                    Information(resultLocalObjectAfterConfigurate, "local object", "After.Configurate()");

                    if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                        || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false
                            && StateInformation.IsDestroy == false))
                    {
                        // Все методы Contruction в локальных обьектах запустились, двигаемся дальше.
                        //...
                    }
                    else
                    {
                        // Если возникли проблемы, то мы не зависываем имя локального обьекта на котором произошел обрыв.
                        // Так как конструктор в локальных обьектах не предназначен для серьезных задач.
                        return;
                    }
                }

                if (StateInformationManager.FlagReplase(information.State.Data.STARTING, information.State.Data.CONFIGURATE))
                {
                    var resultLocalObjectBeforeStart = LocalObjectsManager.BeforeLifeCyrcleMainObject<ILocalObject.Start, ILocalObject.Start.Before>();
                    {
                        Information(resultLocalObjectBeforeStart, "local object", "Before.Start()");

                        if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                            || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false
                                && StateInformation.IsDestroy == false))
                        {
                            // Все методы Contruction в локальных обьектах запустились, двигаемся дальше.
                            //...
                        }
                        else
                        {
                            // Если возникли проблемы, то мы не зависываем имя локального обьекта на котором произошел обрыв.
                            // Так как конструктор в локальных обьектах не предназначен для серьезных задач.
                            return;
                        }
                    }

                    pStartMainObject();
                    {
                        if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                            || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false
                                && StateInformation.IsDestroy == false))
                        {
                            if (CurrentCallMethod != SystemMethod.START)
                                Information(true, "current object", SystemMethod.START);
                        }
                        else
                        {
                            if (CurrentCallMethod != SystemMethod.START)
                                Information(false, "current object", SystemMethod.START);

                            return;
                        }
                    }

                    var resultLocalObjectAfterStart = LocalObjectsManager.AfterLifeCyrcleMainObject<ILocalObject.Start.After>();
                    {
                        Information(resultLocalObjectAfterStart, "local object", "After.Start()");

                        if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                            || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false
                                && StateInformation.IsDestroy == false))
                        {
                            // Все методы Contruction в локальных обьектах запустились, двигаемся дальше.
                            //...
                        }
                        else
                        {
                            // Если возникли проблемы, то мы не зависываем имя локального обьекта на котором произошел обрыв.
                            // Так как конструктор в локальных обьектах не предназначен для серьезных задач.
                            return;
                        }
                    }
                }
            }
            else
                Exception(Ex.LifeCyrcleActivity.x10003, information.State.Data.CONFIGURATE, StateInformation.Get());
        }

        /// <summary>
        /// Если будет после запуска потока в Branch обьектах произойдет вызов уничтожение, то
        /// сообщение об уничтожении будет постоянно поступать системный ActionInvokeHandler до
        /// тех пор пока Node обьект полностью не запустится.
        /// </summary>
        /// <param name="pThreadActivity"></param>
        public void StartThread(thread.description.activity.IThread pThreadActivity = null)
        {
            if ((HeaderInformation.IsNodeObject() && StateInformation.IsDestroy == false)
                || (HeaderInformation.IsBranchObject() && NodeInformation.NodeObject.StateInformation.IsDestroy == false
                    && StateInformation.IsDestroy == false))
            {
                if (StateInformationManager.FlagReplase(information.State.Data.START, information.State.Data.STARTING))
                {
                    ((main.activity.description.ILife)NodeInformation.CurrentMainObject).Dependency();

                    MainObjectsManager.LifeCyrcle(information.State.Data.CONTINUE_STARTING);

                    PrivateHandlersManager.LifeCyrcle(information.State.Data.CONTINUE_STARTING);

                    PublicHandlersManager.LifeCyrcle(information.State.Data.CONTINUE_STARTING);

                    pThreadActivity?.Start();

                    SystemInformation($"running ...");
                }
            }

            if (StateInformation.IsDeferredDestroying)
            {
                NodeInformation.CurrentMainObject.destroy();
            }
        }

        public void Stop(global::System.Action pStopMainObject, thread.description.activity.IThread pThreadActivity = null)
        {
            if (TegsInformation.IsTeg(GlobalData.TASK_STOPPING_THREAD))
            {
                pThreadActivity.TaskStop();
            }
            else
                pThreadActivity?.Stop();

            var resultLocalObjectBeforeStop = LocalObjectsManager.BeforeLifeCyrcleMainObject<ILocalObject.Stop, ILocalObject.Stop.Before>();
            {
                Information(resultLocalObjectBeforeStop, "local object", "Before.Stop()");

                if (StateInformation.StartProcess || StateInformation.IsStarting || StateInformation.IsConfigurate
                    || (HeaderInformation.IsBranchObject() && StateInformation.IsCreating)) // Ветка может быть в состоянии IsCreating.
                {
                    // Все методы Contruction в локальных обьектах запустились, двигаемся дальше.
                    //...
                }
                else
                {
                    // Если возникли проблемы, то мы не зависываем имя локального обьекта на котором произошел обрыв.
                    // Так как конструктор в локальных обьектах не предназначен для серьезных задач.
                    return;
                }
            }


            if (StateInformation.StartProcess || StateInformation.IsStarting || StateInformation.IsConfigurate 
                || (HeaderInformation.IsBranchObject() && StateInformation.IsCreating)) // Ветка может быть в состоянии IsCreating.
            {
                pStopMainObject.Invoke();
                {
                    if (StateInformation.__IsException == false)
                    {
                        if (CurrentCallMethod != SystemMethod.STOP)
                            Information(true, "current object", SystemMethod.STOP);
                    }
                    else
                    {
                        if (CurrentCallMethod != SystemMethod.STOP)
                            Information(false, "current object", SystemMethod.STOP);
                    }
                }

                var resultLocalObjectAfterStop = LocalObjectsManager.AfterLifeCyrcleMainObject<ILocalObject.Stop.After>();
                {
                    Information(resultLocalObjectAfterStop, "local object", "After.Stop()");

                    if (StateInformation.StartProcess || StateInformation.IsStarting || StateInformation.IsConfigurate
                    || (HeaderInformation.IsBranchObject() && StateInformation.IsCreating))
                    {
                        // Все методы Contruction в локальных обьектах запустились, двигаемся дальше.
                        //...
                    }
                    else
                    {
                        // Если возникли проблемы, то мы не зависываем имя локального обьекта на котором произошел обрыв.
                        // Так как конструктор в локальных обьектах не предназначен для серьезных задач.
                        return;
                    }
                }

                StateInformationManager.Set(information.State.Data.STOP);

                if (HeaderInformation.IsNodeObject())
                {
                    NodeInformation.SystemAccess.AddActionInvoke(SubscriptionControlManager.StartUnsubscribe);
                }
                else if (HeaderInformation.IsBranchObject())
                    ((main.activity.description.ILife)NodeInformation.CurrentMainObject).ContinueStop();
            }
        }

        /// <summary>
        /// Продолжаем остановку.Она продожится только после того когда все Node дочерние обьекты будут доведены до своего полного создания.
        /// И будут готовы к уничтожению.
        /// </summary>
        public void ContinueStop(global::System.Action pStopMainObject)
        {
            SystemInformation("Pr.Handler:" + PrivateHandlersManager.Count + ", Pu.Handler:" + PublicHandlersManager.Count +
                        ", NM.Objects: " + MainObjectsManager.NodeCount + ", BM.Objects:" + MainObjectsManager.BranchCount);

            // Текущий обьект будет уничтожен после уничтожения всех Node и Brench обьектов находящихся в нутри него.
            if (PrivateHandlersManager.Count == 0 && PublicHandlersManager.Count == 0 && MainObjectsManager.NodeCount == 0 &&
                MainObjectsManager.BranchCount == 0)
            {
                // Удаляем глобальные обьекты созданые в текущем обьекте.
                GlobalObjectsManager.RemoveCreatingGlobalObjectsInCurrentObject();

                StateInformationManager.Set(information.State.Data.DESTROYING);

                SystemInformation($"destroy...");

                if (HeaderInformation.IsNodeObject())
                {
                    ((manager.objects.description.access.remove.IMain)NodeInformation.ParentMainObject)
                            .Remove(NodeInformation.KeyObject, HeaderInformation.SystemType);
                }
                else if (HeaderInformation.IsPublicHandler())
                {
                    ((main.description.activity.IBranch)NodeInformation.ParentMainObject).RemovePublicHandler();
                }
                else if (HeaderInformation.IsPrivateHandler())
                {
                    ((main.description.activity.IBranch)NodeInformation.ParentMainObject).RemovePrivateHandler();
                }
                else if (HeaderInformation.IsController() && HeaderInformation.IsBranchObject())
                {
                    ((main.description.activity.IBranch)NodeInformation.ParentMainObject).RemoveBranchMainObject();
                }
                else
                    Exception(Ex.LifeCyrcleActivity.x10002, HeaderInformation.SystemType.ToString());
            }
            else
            {
                // Остановка Branch обьектов начнется только когда будут полностью остановлены и уничтожены все
                // Node обьекты. После того как Node обьектов в MainObjectsManager станет ноль(или окажется что их
                // небыло) MainObjectsManager запустит данных метод и зайдет сюда.
                if (MainObjectsManager.NodeCount == 0)
                {
                    if (PublicHandlersManager.Count > 0 && PublicHandlersManager.IsStopping)
                    {
                        var resultPublicHandlerManager = PublicHandlersManager.LifeCyrcle(information.State.Data.STOP);
                        {
                            Information(resultPublicHandlerManager, "public handler", "stop");

                            if (StateInformation.IsStop)
                            {
                                //...
                            }
                            else
                            {
                                Exception(Ex.LifeCyrcleActivity.x10001, "Stop()", "PublicHandlersManager");

                                return;
                            }
                        }
                    }

                    if (PrivateHandlersManager.Count > 0 && PrivateHandlersManager.IsStopping)
                    {
                        var resultPrivateHandlerManager = PrivateHandlersManager.LifeCyrcle(information.State.Data.STOP);
                        {
                            Information(resultPrivateHandlerManager, "private handler", "stop");

                            if (StateInformation.IsStop)
                            {
                                //...
                            }
                            else
                            {
                                Exception(Ex.LifeCyrcleActivity.x10001, "Stop()", "PrivateHandlersManager");

                                return;
                            }
                        }
                    }

                    if (MainObjectsManager.BranchCount > 0 && MainObjectsManager.IsStopping)
                    {
                        var resultMainObjectsManager = MainObjectsManager.LifeCyrcle(information.State.Data.STOP);
                        {
                            Information(resultMainObjectsManager, "main branch object", "stop");

                            if (StateInformation.IsStop)
                            {
                                //...
                            }
                            else
                            {
                                Exception(Ex.LifeCyrcleActivity.x10001, "Stop()", "MainObjectsManager");

                                return;
                            }
                        }
                    }
                }
            }
        }

        public void Destroy()
        {
            lock (StateInformation.Locker)
            {
                if (StateInformation.IsCreating || StateInformation.IsOccurrence)
                {
                    StateInformationManager.DeferredDestroying();
                }
                // На случай если обьект будет остановлен в потоке, что бы при повторном вызове не биться в локер.
                else if (StateInformation.IsDestroy == false || (StateInformation.IsDestroy == false && StateInformation.IsDeferredDestroying))
                {
                    if (HeaderInformation.IsBranchObject())
                    {
                        StateInformationManager.Destroy();

                        ((main.description.activity.INode)NodeInformation.NodeObject).Destroy();
                        ((main.description.activity.INode)NodeInformation.NearNodeIndependentObject).Destroy();
                    }
                    else if (HeaderInformation.IsNodeObject() && HeaderInformation.IsIndependent() == false)
                    {
                        ((main.description.activity.INode)NodeInformation.CurrentMainObject).Destroy();
                        ((main.description.activity.INode)NodeInformation.NearNodeIndependentObject).Destroy();
                    }
                    else if (HeaderInformation.IsIndependent())
                    {
                        ((main.description.activity.INode)NodeInformation.CurrentMainObject).Destroy();
                    }
                    else
                        Exception("Неизвестный тип обьекта.");
                }
            }
            
        }
    }
}
