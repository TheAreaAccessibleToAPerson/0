namespace Butterfly.system.objects.main.manager.objects
{
    /// <summary>
    /// Локальные обьекты которые живут только в нутри обьекта. Наследуют интерфейс ILocalObject.
    /// С помощью интерфейса IStateOfLife могут прожить жизнь вместе с MainObject.
    /// </summary>
    public class Local : Informing, description.access.add.ILocal
    {
        private global::System.Collections.Generic.Dictionary<string, object> Values;

        private readonly information.State StateInformation;

        private readonly IInforming Informing;

        public Local(information.State pStateInformation, IInforming pInforming)
            : base("LocalObjectsManager", pInforming)
        {
            Informing = pInforming;

            Values = null;

            StateInformation = pStateInformation;
        }

        public int Process(string pStateLife)
        {
            switch (pStateLife)
            {
                case information.State.Data.CREATING: foreach (var pair in Values) 
                        ((main.activity.description.ILife)pair.Value).Construction();
                    return Values.Count;

                case information.State.Data.DEPENDENCY: foreach (var pair in Values) 
                        ((main.activity.description.ILife)pair.Value).Dependency();
                    return Values.Count;

                case information.State.Data.STARTING: foreach (var pair in Values) 
                        ((main.activity.description.ILife)pair.Value).Start();
                    return Values.Count;

                default:
                    Exception(Ex.LocalObject.x10003);
                    break;
            }

            return -1;
        }

        /// <summary>
        /// Создает/Получает локальный обьект.
        /// </summary>
        /// <typeparam name="LocalObjectType"></typeparam>
        /// <returns></returns>
        public LocalObjectType TryAdd<LocalObjectType>(string pObjectName = "") where LocalObjectType : ILocalObject, new()
        {
            if (StateInformation.__IsCreating || StateInformation.__IsStarting)
            {
                if (Values == null) Values = new System.Collections.Generic.Dictionary<string, object>();

                //Если имя явно задано, то нужно проверить не определили ли мы 
                //уже обьект с таким же типом без явного указания имени.
                if (pObjectName != "")
                {
                    foreach (var value in Values)
                    {
                        if (value.Key == typeof(LocalObjectType).FullName)
                        {
                            Exception(Ex.LocalObject.x10002, typeof(LocalObjectType).FullName);

                            return default;
                        }
                    }

                    // Если обьект уже был определен, то вернем его...
                    if (Values.TryGetValue(pObjectName, out object oLocalObject))
                    {
                        return (LocalObjectType)oLocalObject;
                    }
                    //... если нет то определим его.
                    else
                    {
                        LocalObjectType localObject = new LocalObjectType();

                        if (localObject is description.access.definition.ILocalInformation localObjectName)
                        {
                            localObjectName.Define(Informing);
                        }
                        
                        Values.Add(pObjectName, localObject);

                        return localObject;
                    }
                }
                else
                {
                    if (Values.TryGetValue(typeof(LocalObjectType).FullName, out object pLocalObject))
                    {
                        return (LocalObjectType)pLocalObject;
                    }

                    LocalObjectType localObj = new LocalObjectType();

                    if (localObj is description.access.definition.ILocalInformation localObjReduse)
                    {
                        localObjReduse.Define(Informing);
                    }

                    Values.Add(typeof(LocalObjectType).FullName, localObj);

                    return localObj;
                }


            }
            else
                Exception(Ex.LocalObject.x10001, typeof(LocalObjectType).FullName);

            return default;
        }

        /// <summary>
        /// Вызвет во всех локальных обьектах методы представляющие их цикл жизни 
        /// операжающие цикл пространсва в котором они были определы. 
        /// </summary>
        public System.Tuple<string, bool>[] BeforeLifeCyrcleMainObject<DefaultState, BeforeState>()
        {
            System.Tuple<string, bool>[] result = null;

            if (Values == null) return result;
            
            foreach (object localObject in Values)
            {
                
                string systemInformation = "";

                if (localObject is ILocalObject.Start localObjectStartReduse)
                {
                    if (typeof(ILocalObject.Start).FullName == typeof(DefaultState).FullName)
                    {
                        systemInformation = "Start()";

                        localObjectStartReduse.Start();
                    }
                }
                else if (localObject is ILocalObject.Start.Before localObjectStartBeforeReduse)
                {
                    
                    if (typeof(ILocalObject.Start.Before).FullName == typeof(BeforeState).FullName)
                    {
                        
                        systemInformation = "Start()";

                        localObjectStartBeforeReduse.Start();
                    }
                }
                else if (localObject is ILocalObject.Configurate localObjectConfigurateReduse)
                {
                    if (typeof(ILocalObject.Configurate).FullName == typeof(DefaultState).FullName)
                    {
                        systemInformation = "Configurate()";

                        localObjectConfigurateReduse.Configurate();
                    }
                }
                else if (localObject is ILocalObject.Configurate.Before localObjectConfigurateBeforeReduse)
                {
                    if (typeof(ILocalObject.Configurate.Before).FullName == typeof(BeforeState).FullName)
                    {
                        systemInformation = "Configurate()";

                        localObjectConfigurateBeforeReduse.Configurate();
                    }
                }
                else if (localObject is ILocalObject.Stop localObjectStopReduse)
                {
                    if (typeof(ILocalObject.Stop).FullName == typeof(DefaultState).FullName)
                    {
                        systemInformation = "Stop()";

                        localObjectStopReduse.Stop();
                    }
                }
                else if (localObject is ILocalObject.Stop.Before localObjectStopBeforeReduse)
                {
                    if (typeof(ILocalObject.Stop.Before).FullName == typeof(BeforeState).FullName)
                    {
                        systemInformation = "Stop()";

                        localObjectStopBeforeReduse.Stop();
                    }
                }

                if (systemInformation != "")
                {
                    if (result == null)
                    {
                        result = new System.Tuple<string, bool>[1];
                    }
                    else
                    {
                        result = Hellper.ExpendArray(result, new System.Tuple<string, bool>(localObject.GetType().Name, !StateInformation.IsERROR));
                    }
                }
            }

            return result;
        }
        public System.Tuple<string, bool>[] AfterLifeCyrcleMainObject<AfterState>(string pNameObjectStopInvoke = "")
        {
            System.Tuple<string, bool>[] result = null;

            if (Values == null) return result;

            foreach (object localObject in Values)
            {
                string systemInformation = "";

                if (localObject is ILocalObject.Construction localObjectConstructionReduse)
                {
                    if (typeof(ILocalObject.Construction).FullName == typeof(AfterState).FullName)
                    {
                        systemInformation = "Construction()";

                        localObjectConstructionReduse.Construction();
                    }
                }
                else if (localObject is ILocalObject.Start.After localObjectStartReduse)
                {
                    if (typeof(ILocalObject.Start.After).FullName == typeof(AfterState).FullName)
                    {
                        systemInformation = "Start()";

                        localObjectStartReduse.Start();
                    }
                }
                else if (localObject is ILocalObject.Configurate.After localObjectConfigurateReduse)
                {
                    if (typeof(ILocalObject.Configurate.After).FullName == typeof(AfterState).FullName)
                    {
                        systemInformation = "Configurate()";

                        localObjectConfigurateReduse.Configurate();
                    }
                }
                else if (localObject is ILocalObject.Stop.After localObjectStopReduse)
                {
                    if (typeof(ILocalObject.Stop.After).FullName == typeof(AfterState).FullName)
                    {
                        systemInformation = "Stop()";

                        localObjectStopReduse.Stop();
                    }
                }

                if (systemInformation != "")
                {
                    if (result == null)
                    {
                        result = new System.Tuple<string, bool>[1];
                    }
                    else
                    {
                        result = Hellper.ExpendArray(result, new System.Tuple<string, bool>(localObject.GetType().Name, !StateInformation.IsERROR));
                    }
                }
            }

            return result;
        }
    }
}
