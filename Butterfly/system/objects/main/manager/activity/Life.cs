namespace Butterfly.system.objects.main.manager.activity
{
    public class Life : Informing
    {
        public struct Data
        {
            public const string MAIN_OBJECTS_MANAGER = "MainObjectsManager/LifeActivity";
            public const string PRIVATE_HANDLERS_MANAGER = "PrivateHandlerManager/LifeActivity";
            public const string PUBLIC_HANDLERS_MANAGER = "PublicHandlerManager/LifeActivity";
        }

        /// <summary>
        /// Имя менеджера который использует текущий класс.
        /// </summary>
        private readonly string ManagerName;

        /// <summary>
        /// Количесво сконструированых обьектов.
        /// </summary>
        public int CreatingCount { private set; get; }

        public Life(IInforming pInforming, string pName) : base(pName, pInforming)
        {
            ManagerName = pName;
        }

        public System.Tuple<string, bool>[] Process(string pStateLife, Object[] pValues)
        {
            System.Tuple<string, bool>[] result = null;

            int i = 0;
            switch (pStateLife)
            {
                case information.State.Data.CREATING:

                    if (pValues.Length > 0)
                        result = new global::System.Tuple<string, bool>[pValues.Length];

                    if (ManagerName == Data.MAIN_OBJECTS_MANAGER)
                    {
                        foreach (var value in pValues)
                        {
                            ((main.activity.description.ILife)value).Construction();

                            // Если после запуска конструктора в обьекте не был определен локальный обьект
                            // вызовем ошибку времени сборки.
                            if (value is local.description.access.set.IValue mainObjectLocalValueReduse)
                            {
                                if (mainObjectLocalValueReduse.IsEmpty())
                                    Exception(Ex.MainObjectsManager.x10003,
                                        value.HeaderInformation.Name, mainObjectLocalValueReduse.GetValueType());
                            }

                            result[i++] = new System.Tuple<string, bool>
                                (value.HeaderInformation.Name, value.StateInformation.IsDestroy == false);

                            CreatingCount++;
                        }
                    }
                    else
                    {
                        foreach (var value in pValues)
                        {
                            ((main.activity.description.ILife)value).Construction();

                            result[i++] = new System.Tuple<string, bool>
                                (value.HeaderInformation.Name, value.StateInformation.IsDestroy == false);
                        }
                    }

                    break;
                case information.State.Data.DEPENDENCY:
                    foreach (var value in pValues)
                    {
                        ((main.activity.description.ILife)value).Dependency();
                    }
                    break;

                case information.State.Data.STARTING:

                    if (pValues.Length > 0)
                        result = new global::System.Tuple<string, bool>[pValues.Length];

                    foreach (var value in pValues)
                    {
                        ((main.activity.description.ILife)value).Start();

                        result[i++] = new System.Tuple<string, bool>
                            (value.HeaderInformation.Name, value.StateInformation.IsDestroy == false);
                    }
                    break;

                case information.State.Data.CONTINUE_STARTING:

                    if (pValues.Length > 0)
                        result = new global::System.Tuple<string, bool>[pValues.Length];

                    foreach (var value in pValues)
                    {
                        ((main.activity.description.ILife)value).ContinueStart();

                        result[i++] = new System.Tuple<string, bool>
                            (value.HeaderInformation.Name, value.StateInformation.IsDestroy == false);
                    }
                    break;

                case information.State.Data.STOP:

                    if (pValues.Length > 0)
                        result = new global::System.Tuple<string, bool>[pValues.Length];

                    foreach (var value in pValues)
                    {
                        ((description.activity.INode)value).Destroy();

                        result[i++] = new System.Tuple<string, bool>
                            (value.HeaderInformation.Name, value.StateInformation.IsDestroy);
                    }

                    break;

                default:
                    Exception("Определено неизвестное состояние {0}", pStateLife);
                    break;
            }

            return result;
        }
    }
}
