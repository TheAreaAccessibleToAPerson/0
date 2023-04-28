namespace Butterfly.system.objects.main.objects.action
{
    class Object<ParamType1, ParamType2> : IInput<ParamType1, ParamType2>
    {
        private global::System.Action<ParamType1, ParamType2> Action, Event;

        private readonly poll.description.access.add.IPoll PollAccess;
        private readonly main.description.access.add.IDependency Dependency;

        public Object(global::System.Action<ParamType1, ParamType2> pAction, main.description.access.add.IDependency pDependencyMainObject,
            poll.description.access.add.IPoll pPollAccess, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            Event = Action = pAction;

            Dependency = pDependencyMainObject;
            PollAccess = pPollAccess;

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
                ValuesSafeCollection = new collections.safe.Values<ParamType1, ParamType2>();
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
        private collections.safe.Values<ParamType1, ParamType2> ValuesSafeCollection;
        private void PollProcess() // Даный метод будет подписан в пул.
        {
            if (ValuesSafeCollection.ExtractAll(out ParamType1[] oValues1, out ParamType2[] oValues2))
            {
                for(int i = 0; i < oValues1.Length; i++)
                {
                    DefaultInput(oValues1[i], oValues2[i]);
                }
            }
        }

        public void ToInput(ParamType1 pValue1, ParamType2 pValue2)
        {
            Event.Invoke(pValue1, pValue2);
        }

        private void DefaultInput(ParamType1 pValue1, ParamType2 pValue2)
        {
            Action.Invoke(pValue1, pValue2);
        }
    }
}
