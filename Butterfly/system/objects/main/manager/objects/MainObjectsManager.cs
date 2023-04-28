namespace Butterfly.system.objects.main.manager.objects
{
    public class Main : Informing
    {
        /// <summary>
        /// Если обьекты будут создоватся во время выполнения программы то они будут записаны сюда
        /// и будут являтся Node обьектами.
        /// </summary>
        private global::System.Collections.Generic.Dictionary<System.Tuple<string, System.Type>, Object> NodeValues;

        public int NodeCount
        {
            get
            {
                if (NodeValues == null)
                    return 0;
                else
                    return NodeValues.Count;
            }
        }

        /// <summary>
        /// Если обьект создается без имени,то если задасться имя пустое имя.
        /// </summary>
        private ulong EmptyKey = uint.MaxValue;

        /// <summary>
        /// Количесво обьектов выставленых на создание. Мне не сможем уничтожить текущий обьект пока это значение не
        /// будет указывать в 0.
        /// </summary>
        public int CreatingNodeObjectCount { private set; get; } = 0;

        // Если обьекты создаются в методе Contruction(), то они являются частью текущего обьекта
        // и становятся Branch.
        private string[] KeyValues;
        private Object[] ObjectValues;
        private string[] TypeValues;

        public int BranchCount { private set; get; } = 0;

        /// <summary>
        /// Текущий менеджер MainObjects запущен на уничтожение BranchMainObjects.
        /// </summary>
        private bool IsStartingToStopping = false;

        /// <summary>
        /// При первом обращении значение IsStartingToStopping выставится в true и вернет true
        /// что позволит запустить отсановку. При повторном обращении мы получим значение false
        /// которое будет означать что менеджер приватных обработчов уже поставлен на уничтожение.
        /// </summary>
        public bool IsStopping
        {
            get
            {
                if (IsStartingToStopping == false)
                {
                    IsStartingToStopping = true;

                    return true;
                }

                return false;
            }
        }

        public void RemoveBrachObject()
        {
            lock(StateInformation.Locker)
            {
                if (--BranchCount == 0)
                {
                    ((main.activity.description.ILife)NodeInformation.CurrentMainObject).ContinueStop();
                }
            }
        }


        private long ControllerKeyIndex;

        private readonly information.Header HeaderInformation;
        private readonly information.State.Manager StateInformationManager;
        private readonly information.State StateInformation;
        private readonly information.Node NodeInformation;

        private readonly Global GlobalObjectsManagerAccess;

        private readonly activity.Life LifeActivity;

        /// <summary>
        /// После того как будет вызвать абстрактный метод Stop(), запустится уничтожение обьектов.
        /// В первую очередь нужно уничтожить дочерние Node обьекты.
        /// В момент уничтожения они могут создоватся поэтому подождем пока они все создадутся.
        /// После остановим их а уже после остановим данный обьект.
        /// </summary>
        private readonly global::System.Action ContinueStop;

        public Main(IInforming pInforming, information.Header pHeaderInformation, information.State pStateInformation, 
            information.State.Manager pStateInformationManager, information.Node pNodeInformation, Global pGlobalObjects, 
            global::System.Action pContinueStop)
            : base("MainObjectsManager", pInforming)
        {
            KeyValues = new string[0];
            ObjectValues = new Object[0];
            TypeValues = new string[0];

            LifeActivity = new activity.Life(pInforming, activity.Life.Data.MAIN_OBJECTS_MANAGER);

            ContinueStop = pContinueStop;

            NodeValues = null;

            ControllerKeyIndex = short.MaxValue;

            StateInformationManager = pStateInformationManager;
            StateInformation = pStateInformation;
            NodeInformation = pNodeInformation;
            GlobalObjectsManagerAccess = pGlobalObjects;
            HeaderInformation = pHeaderInformation;
        }

        public System.Tuple<string, bool>[] LifeCyrcle(string pStateLife)
        {
            return LifeActivity.Process(pStateLife, ObjectValues);
        }

        public MainObjectType TryAddBranch<MainObjectType>(MainObjectType pControllerObject, string pKey = "", object pLocalValue = null)
            where MainObjectType : Object, new()
        {
            lock(StateInformation.Locker)
            {
                if (StateInformation.IsCreating)
                {
                    if (typeof(MainObjectType).FullName == HeaderInformation.SystemType.FullName)
                    {
                        Exception(Ex.MainObjectsManager.x10008, typeof(MainObjectType).FullName);

                        return default;
                    }

                    for (int i = 0; i < ObjectValues.Length; i++)
                    {
                        if (pKey == "")
                        {
                            // Получение обьекта по типу.
                            if (TypeValues[i] == typeof(MainObjectType).ToString())
                            {
                                // Если у обьека пустое имя то проверим явлется ли оно >= значения uint.MaxValue
                                // Чтобы убедится что оно создовалось с пустым именем.
                                if (ulong.TryParse(KeyValues[i], out ulong emptyName))
                                {
                                    if (emptyName >= uint.MaxValue)
                                    {
                                        // Если в этом обращении мы передали локально значение, то запишим его.
                                        SetValue("#" + pKey, pControllerObject, pLocalValue);

                                        // Обьект уже был создан вернем его.
                                        return (MainObjectType)ObjectValues[i];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (KeyValues[i] == pKey) // Если совподение по явно переданому имени то вернем это значение.
                            {
                                if (TypeValues[i] == typeof(MainObjectType).ToString())
                                {
                                    SetValue(pKey, pControllerObject, pLocalValue);

                                    return (MainObjectType)ObjectValues[i];
                                }
                                else
                                {
                                    Exception(Ex.MainObjectsManager.x10006, pKey, typeof(MainObjectType).FullName, TypeValues[i]);
                                }
                            }
                        }
                    }
                }

                if (pKey != "")
                {
                    // Если в качесве имени вы будете использоваться значение, то оно не должно
                    // превышать uint.MaxValue.
                    if (uint.TryParse(pKey, out uint checkMaxValueEmptyName))
                    {
                        if (checkMaxValueEmptyName >= uint.MaxValue)
                        {
                            Exception(Ex.MainObjectsManager.x10002, typeof(MainObjectType).FullName, uint.MaxValue.ToString());
                        }
                    }
                }
                else
                {
                    pKey = EmptyKey++.ToString();
                }


                // Создание обьекта в конструторе или методе Start().
                // Создоваемый обьект станет частью обьекта(Branch обьектом) в нутри которого создается.
                // Обьект не может быть Independent.
                if (StateInformation.IsCreating)
                {
                    // Настроим обьект.
                    ((main.description.definition.INode)pControllerObject).BranchDefine(NodeInformation.NodeObject,
                            NodeInformation.CurrentMainObject, NodeInformation.NearNodeIndependentObject,
                            NodeInformation.SystemAccess, GlobalObjectsManagerAccess.Get(), pKey, NodeInformation.NodeID,
                            NodeInformation.AttackmentNumberObjectInNode + 1,
                            NodeInformation.AttachmentNodeNumberInSystem + 1,
                            Hellper.ExpendArray(NodeInformation.IDNodeParents, NodeInformation.NodeObject.GetID()));

                    // Если в обьекте есть локальное значение, то его нужно передать только один раз.

                    SetValue(pKey, pControllerObject, pLocalValue);

                    KeyValues = Hellper.ExpendArray(KeyValues, pKey);
                    TypeValues = Hellper.ExpendArray(TypeValues, typeof(MainObjectType).FullName);
                    ObjectValues = Hellper.ExpendArray(ObjectValues, pControllerObject);

                    BranchCount++;

                    return pControllerObject;
                }
               
                return default;
            }
        }

        public void DeferredAddNode<MainObjectType>(string pKey, object pLocalValue = null)
            where MainObjectType : Object, new()
        {
            if (StateInformation.IsStarting)
            {
                if ( (HeaderInformation.IsNodeObject() && NodeInformation.NearNodeIndependentObject.StateInformation.IsDestroy == false) ||
                    HeaderInformation.IsBranchObject() && StateInformation.IsDestroy == false)
                {
                    ((main.description.access.add.IDependency)NodeInformation.CurrentMainObject).Add(() =>
                    {
                        if (AddNode(new MainObjectType(), pKey, pLocalValue).HeaderInformation.IsIndependent())
                            Exception(Ex.MainObjectsManager.x10011, typeof(MainObjectType).FullName);
                    });
                }
            }
            else
            {
                
                if ((StateInformation.IsOccurrence || StateInformation.IsCreating || StateInformation.IsConfigurate || StateInformation.StartProcess) && StateInformation.IsDestroy == false)
                    Exception(Ex.MainObjectsManager.x10012, typeof(MainObjectType).FullName);
            } 
        }

        public MainObjectType CreateNode<MainObjectType>(string pKey, object pLocalValue = null)
            where MainObjectType : Object, new()
        { 
            if (StateInformation.StartProcess && ((HeaderInformation.IsNodeObject() && NodeInformation.NearNodeIndependentObject.StateInformation.IsDestroy == false) ||
                    (HeaderInformation.IsBranchObject() && NodeInformation.ParentMainObject.HeaderInformation.IsNodeObject() == true && StateInformation.IsDestroy == false)))
            {
                return AddNode(new MainObjectType(), pKey, pLocalValue);
            }
            else
            {
                if ((StateInformation.IsOccurrence || StateInformation.IsCreating || StateInformation.IsConfigurate || StateInformation.IsStarting) 
                    && StateInformation.IsDestroy == false)
                {
                    Exception(Ex.MainObjectsManager.x10013, typeof(MainObjectType).FullName);
                }   
            }

            return default;
        }

        //1)Реализовать замену обьекта.
        private MainObjectType AddNode<MainObjectType>(MainObjectType pControllerObject, string pKey, object pLocalValue = null)
            where MainObjectType : Object, new()
        {
            lock(StateInformation.Locker)
            {
                ((main.description.definition.INode)pControllerObject).NodeDefine(NodeInformation.CurrentMainObject,
                                    NodeInformation.NearNodeIndependentObject, NodeInformation.SystemAccess,
                                        GlobalObjectsManagerAccess.Get(), pKey, NodeInformation.AttachmentNodeNumberInSystem + 1,
                                        Hellper.ExpendArray(NodeInformation.IDNodeParents, NodeInformation.NodeObject.GetID()));

                SetValue(pKey, pControllerObject, pLocalValue);

                // Системный node создает сам себя в себе, поэтому не нужно его записывать.
                if (pControllerObject.HeaderInformation.IsSystemController() == false)
                {
                    CreatingNodeObjectCount++;

                    if (NodeValues == null) NodeValues = new System.Collections.Generic.Dictionary<System.Tuple<string, System.Type>, Object>();

                    NodeValues.Add(new global::System.Tuple<string, global::System.Type>(pKey, pControllerObject.GetType()), pControllerObject);
                }

                NodeInformation.SystemAccess.AddActionInvoke(((main.description.activity.INode)pControllerObject).Create);

                return pControllerObject;
            }
        }

        /// <summary>
        /// Проинформируем что обьект закончил свое создание.
        /// </summary>
        public void InformCreateObject()
        {
            lock(StateInformation.Locker)
            {
                // Системый node не создает сам себя в себе, поэтому не должен отчитываться о создании.
                if (HeaderInformation.IsSystemController() == false)
                {
                    if ((CreatingNodeObjectCount - 1) == 0)
                    {
                        //Console("Все создались Total:" + NodeValues.Count);
                    }

                    if ((--CreatingNodeObjectCount) == 0)
                    {
                        // Родительский обьект находится на остановки, остановим все 
                        // дочерние Node обьекты.
                        if (StateInformation.IsDestroy)
                        {
                            StoppingAllNodeObject();
                        }
                    }
                }
                else
                {
                    // Если это системный обьект, то он сосздается сам в себе. После своего создания
                    // он инфрмирует сам себя. При уничтожении мы должны ожидать не 0, а -1.
                    if ((--CreatingNodeObjectCount) == -1)
                    {
                        CreatingNodeObjectCount = 0;

                        // Родительский обьект находится на остановки, остановим все 
                        // дочерние Node обьекты.
                        if (StateInformation.IsDestroy)
                        {
                            StoppingAllNodeObject();
                        }
                    }
                }
            }
             
        }

        public void Remove<MainObjectType>(string pKey)
        {
            if (NodeValues.Remove(new global::System.Tuple<string, global::System.Type>(pKey, typeof(MainObjectType)), out Object oObject))
            {
                // Если обьект останавливается, то при достижении нуля обьектов в Value, нужно продолжить уничтожение,
                // в вверх в плодь но Independent обьекта.
                if (CreatingNodeObjectCount == 0 && NodeValues.Count == 0 && StateInformation.IsDestroy)
                {
                    NodeInformation.SystemAccess.AddActionInvoke(((main.activity.description.ILife)NodeInformation.CurrentMainObject).Stop);
                }
            }   
            else
                Exception(Ex.MainObjectsManager.x10007, typeof(MainObjectType).FullName, pKey);
        }

        public void Remove(string pKey, global::System.Type pType)
        {
            lock(StateInformation.Locker)
            {
                if (NodeValues.Remove(new global::System.Tuple<string, global::System.Type>(pKey, pType), out Object oObject))
                {
                    //Console(NodeValues.Count.ToString());
                    // Если обьект останавливается, то при достижении нуля обьектов в Value, нужно продолжить уничтожение,
                    // в вверх в плодь но Independent обьекта.
                    if (CreatingNodeObjectCount == 0 && NodeValues.Count == 0 && StateInformation.IsDestroy)
                    {
                        NodeInformation.SystemAccess.AddActionInvoke(((main.activity.description.ILife)NodeInformation.CurrentMainObject).Stop);
                    }
                }
                else
                {
                    // Проверяем что обьект является обычным не системным контролер.
                    if (HeaderInformation.IsController())
                        Exception(Ex.MainObjectsManager.x10007, pType.FullName, pKey);
                }
            }
        }

        private void SetValue(string pInformationMainObject, Object pMainObject, object pObject)
        {
            if (pObject == null) return;

            if (pMainObject is local.description.access.set.IValue mainObjectValue)
            {
                // Задает локальное значение обьекту, если значение уже задано вернет false.
                if (mainObjectValue.TrySet(pObject))
                {
                    //...
                }
                else
                {
                    // Проверяем что обьект является обычным не системным контролер.
                    if (HeaderInformation.IsController())
                        Exception(Ex.MainObjectsManager.x10004, pInformationMainObject);
                }      
            }
        }

        /// <summary>
        /// Independent обьекту нужно добратся до самых крайних Node обьектов и проинфомировать их что нужно начать преостановку
        /// своей работы.
        /// Преостанавливает работу всех Node обьектов. Если какие ли бо обьекты находятся на стадии создания, 
        /// то подождем пока они до создадутся. После чего начнем остановку.
        /// </summary>
        public void StoppingAllNodeObject()
        {
            if (CreatingNodeObjectCount == 0)
            {
                if (NodeValues == null || NodeValues.Count == 0)
                {
                    SystemInformation("Это крайний Node обьект.");

                    NodeInformation.SystemAccess.AddActionInvoke(((main.activity.description.ILife)NodeInformation.CurrentMainObject).Stop);
                }
                else
                {
                    foreach (var nodeObject in NodeValues)
                    {
                        ((main.description.activity.INode)nodeObject.Value).Destroy();
                    }
                }
            }
        }

        /// <summary>
        /// Запускаем остановку всех Node обьектов.
        /// </summary>
        public void StoppingBackgroundNodeObject()
        {
            //if (StateInformation.IsStopping == false)
            {
                //...............................................
                Console("??????????????????????????????????????????????????????");
            }
        }
    }
}
