namespace Butterfly.system.objects.main.objects.action
{
    public class Object<ParamType> : IInput<ParamType>, IInput
    {
        // Сюда записывается Action который будет выполняться.
        private global::System.Action<ParamType> Event, Action;

        private readonly poll.description.access.add.IPoll PollAccess;
        private readonly main.description.access.add.IDependency Dependency;

        public readonly string Name;

        public Object(global::System.Action<ParamType> pAction, main.description.access.add.IDependency pDependencyMainObject,
            poll.description.access.add.IPoll pPollAccess, int pPollSize, int pTimeDelay, string pPollName)
        {
            Name = Hellper.GetName(pAction.Method);

            Dependency = pDependencyMainObject;
            PollAccess = pPollAccess;

            Event = Action = pAction;

            RegisterInPoll(pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Регистрируем метод PollProcess в пуллы.
        /// Меняем обычный способ приема данных с дальнейшей ретрансляцией
        /// на запись данных в хранилище. Далее в пуллах вызовется данный метод
        /// извлекет записаные данные и передаст дальше по цепочке до следующего
        /// места где так же реализован такой же способ передачи данных.
        /// </summary>
        public void RegisterInPoll(int pSizePoll, int pTimeDelay, string pName)
        {
            if (pSizePoll != 0)
            {
                // Инициализируем хранилище данных.
                ValuesSafeCollection = new collections.safe.Values<ParamType>();
                // Далее переопределяем стандартный способ работы с входными данными
                // (дальнейшая передача по цепочке) на запись в хранилище.
                Event = ValuesSafeCollection.Add;
                // Создаем регистрационый билет. Который передасться в систему
                // после того как все связи установятся.
                PollAccess.Add(PollProcess, pSizePoll, pTimeDelay, pName);
            }
        }

        // Теперь, обькт принимает данные и записывает их в хранилище.
        // Дальнейшая работа будет происходить в составе пула.
        private collections.safe.Values<ParamType> ValuesSafeCollection;
        private void PollProcess() // Даный метод будет подписан в пул.
        {
            if (ValuesSafeCollection.ExtractAll(out ParamType[] oValueArray))
            {
                foreach (ParamType value in oValueArray)
                {
                    DefaultInput(value);
                }
            }
        }

        public void ToInput(ParamType pValue)
        {
            Event(pValue);
        }

        public void ToInput<InputValueType>(InputValueType pValue)
        {
           
            if (pValue is ParamType valueReduse)
            {
                Event(valueReduse);
            }
        }

        private void DefaultInput(ParamType pValue)
        {
            Action.Invoke(pValue);
        }
    }
}
