namespace Butterfly.system.objects.main.information
{
    namespace description
    {
        public interface IManagerValue<Type>
        {
            void Set(Type pValue);
            Type Get();
            bool Compare(Type pValue);
            bool Replace(Type pReplaceValue, params Type[] pValueArray);

            void Destroy();
            void DeferredDestroying();
        }
    }


    public class State : description.IManagerValue<string>
    {
        public struct Data
        {
            public const string OCCURRENCE = "Occurence"; // Возникновение. В этот момент инициализируются все поля, запускаются конструкторы.
            public const string CREATING = "Creating"; // В  конструкторе.
            public const string CONFIGURATE = "Configurate"; // Настройка конфигураций.
            public const string START = "Start"; // Обьект запущен.
            public const string STARTING = "Starting"; // Обьект запускается.
            public const string CONTINUE_STARTING = "ContinueStarting"; // Продолжаем запуск обьекта.
            public const string PAUSE = "Pause"; // Обьект находится в состоянии ожидания.
            public const string PAUSING = "Pausing"; // Обьект готовится перейти в сотояние ожидания.
            public const string RESUME = "Resume"; // Обьект продолжает свою работу после паузы.
            public const string RESUMING = "Resuming"; // Обьект готовится продолжить свою работу после паузы.
            public const string STOP = "Stop"; // Обьект остановлен.
            public const string STOPPING = "Stopping"; // Обьект останавливается к остановке.

            public const string EXCEPTION = "Exeption"; // В обьекте произошла ошибка времени сборки, скора обькт остановится.
            public const string REALTIME_ERROR = "RealtimeError"; // В обьекте произошла ошибка времени выполнения, попытаемся ее исправить.
            public const string DESTROYING = "Destroying";
            public const string DESTROY = "Destroy"; // Обьект выставлен на уничтожение, начинается его остановка.

            public const string DEPENDENCY = "Dependency"; // Зависимости.
            public const string STOPPING_THREAD = "StoppingThread"; // Остановка всех потоков.
        }

        public readonly object Locker;
        private readonly collections.safe.String StateObject;

        private bool Destroy = false;
        public bool IsDestroy { get { lock (Locker) { return Destroy; } } }

        private bool DeferredDestroying = false;
        public bool IsDeferredDestroying { get { lock (Locker) { return DeferredDestroying; } } }

        void description.IManagerValue<string>.Destroy()
        {
            lock(Locker)
            {
                Destroy = true;
            }
        }

        void description.IManagerValue<string>.DeferredDestroying()
        {
            lock (Locker)
            {
                DeferredDestroying = true;
            }
        }

        public State()
        {
            Locker = new object();
            StateObject = new collections.safe.String(State.Data.OCCURRENCE, ref Locker);
        }

        /// <summary>
        /// Возникновение. В этот момент инициализируются все поля, запускаются конструкторы.
        /// </summary>
        public bool __IsOccurrence { get { return StateObject.Compare(Data.OCCURRENCE); } }
        /// <summary>
        /// Возникновение. В этот момент инициализируются все поля, запускаются конструкторы.
        /// </summary>
        public bool IsOccurrence { get { return StateObject.Value == Data.OCCURRENCE; } }
        /// <summary>
        /// Работает ли обьет
        /// </summary>
        public bool StartProcess { get { return StateObject.Value == Data.START; } }
        /// <summary>
        /// Обьект находится в рабочем состоянии.
        /// </summary>
        public bool __StartProcess { get { return StateObject.Compare(Data.START); } }
        /// <summary>
        /// Создание обьекта.
        /// </summary>
        public bool __IsCreating { get { return StateObject.Compare(Data.CREATING); } }
        /// <summary>
        /// Создание обьекта.
        /// </summary>
        public bool IsCreating { get { return StateObject.Value == Data.CREATING; } }
        /// <summary>
        /// Обьект готовится к запуску.
        /// </summary>
        public bool __IsStarting { get { return StateObject.Compare(Data.STARTING); } }
        /// <summary>
        /// Обьект готовится к запуску.
        /// </summary>
        public bool IsStarting { get { return StateObject.Value == Data.STARTING; } }
        /// <summary>
        /// В нутри обьекта произошла ошибка времени сборки.
        /// </summary>
        public bool __IsException { get { return StateObject.Compare(Data.EXCEPTION); } }
        /// <summary>
        /// В нутри обьекта произошла ошибка времени выполнения, попытаемся ее исправить.
        /// </summary>
        public bool __IsRealtimeError { get { return StateObject.Compare(Data.REALTIME_ERROR); } }
        /// <summary>
        /// Обьект полностью остановил свою работу.
        /// </summary>
        public bool __IsStop { get { return StateObject.Compare(Data.STOP); } }
        /// <summary>
        /// Обьект полностью остановил свою работу.
        /// </summary>
        public bool IsStop { get { return StateObject.Value == Data.STOP; } }
        public bool IsDestroying { get { return StateObject.Value == Data.DESTROYING; } }
        /// <summary>
        /// Обьект останавливается.
        /// </summary>
        public bool __IsStopping { get { return StateObject.Compare(Data.STOPPING); } }
        /// <summary>
        /// Обьект останавливается.
        /// </summary>
        public bool IsStopping { get { return StateObject.Value == Data.STOPPING; } }

        /// <summary>
        /// Обьект останавливается.
        /// </summary>
        public bool IsConfigurate { get { return StateObject.Value == Data.CONFIGURATE; } }

        /// <summary>
        /// Если произошла ошибка времени сборки или ошибка времени выполнения.
        /// </summary>
        public bool IsERROR { get { return (StateObject.Value == Data.EXCEPTION || StateObject.Value == Data.REALTIME_ERROR); } }

        public string Get() => StateObject.Get();

        string description.IManagerValue<string>.Get()
        {
            return StateObject.Get();
        }

        void description.IManagerValue<string>.Set(string pValue)
        {
            lock(Locker)
            {
                StateObject.Set(pValue);
            }
        }

        bool description.IManagerValue<string>.Compare(string pValue)
        {
            return StateObject.Compare(pValue);
        }

        /// <summary>
        /// Если текущее значение совподает хоть с одним pValueArray то выставим ReplaceValue.
        /// </summary>
        /// <param name="pReplaceValue"></param>
        /// <param name="pValueArray"></param>
        /// <returns></returns>
        bool description.IManagerValue<string>.Replace(string pReplaceValue, params string[] pValueArray)
        {
            lock(Locker)
            {
                return StateObject.Replace(pReplaceValue, pValueArray);
            }
        }

        /// <summary>
        /// Менеджер состояния MainObject.
        /// </summary>
        public class Manager : Informing
        {
            private readonly description.IManagerValue<string> State;

            private readonly Header Header;

            public Manager(State pState, Header pHeaderObject, IInforming pInforming)
                : base("StateManager", pInforming)
            {
                State = pState;
                Header = pHeaderObject;
            }

            public void Set(string pValue) => State.Set(pValue);
            public string Get() => State.Get();
            public bool Compare(string pValue) => State.Compare(pValue);
            public bool Replace(string pReplaceValue, params string[] pValueArray) => State.Replace(pReplaceValue, pValueArray);
            public void Destroy() => State.Destroy();
            public void DeferredDestroying() => State.DeferredDestroying();

            /// <summary>
            /// Меняет флаги при условии.
            /// </summary>
            /// <param name="pReplaceFlag">Имя флага который нужно выставить.</param>
            /// <param name="pCurrentFlags">Список флагов которые предположительно могут предшествовать ReplastFlag.</param>
            /// <returns></returns>
            public bool FlagReplase(string pReplaceFlag, params string[] pCurrentFlags)
            {
                bool result = true;
                {
                    if (State.Replace(pReplaceFlag, pCurrentFlags))
                    {
                        SystemInformation($":{Get()}");
                    }
                    else
                        result = false;
                }
                return result;
            }
        }
    }
}
