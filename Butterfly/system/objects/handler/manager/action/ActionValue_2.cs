namespace Butterfly.system.objects.handler.manager.action
{
    public class Object<ParamType1, ParamType2> : main.Informing
    {
        /// <summary>
        /// Сдесь будет хранится способ обработки входящих данных.
        /// </summary>
        public global::System.Action<ParamType1, ParamType2> Event, Action;

        private global::System.Action<ParamType1, ParamType2>[] ActionArray = new global::System.Action<ParamType1, ParamType2>[0];

        private readonly main.information.State StateInformationMain;

        public Object(main.IInforming pInformingMainObject, main.information.State pStateMainObject)
            : base("ActionManager_2", pInformingMainObject)
        {
            StateInformationMain = pStateMainObject;

            Event = Action = DefaultInput;
        }

        // Теперь, обькт принимает данные и записывает их в хранилище.
        // Дальнейшая работа будет происходить в составе пула.
        private collections.safe.Values<ParamType1, ParamType2> ValuesSafeCollection;
        private void PollProcess() // Даный метод будет подписан в пул.
        {
            if (ValuesSafeCollection.ExtractAll(out ParamType1[] oValues1, out ParamType2[] oValues2))
            {
                for (int i = 0; i < oValues1.Length; i++)
                {
                    DefaultInput(oValues1[i], oValues2[i]);
                }
            }
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
                Action = ValuesSafeCollection.Add;
                // Создаем регистрационый билет. Который передасться в систему
                // после того как все связи установятся.
                //PollAccess.Add(PollProcess, pSizePoll, pTimeDelay, pName);
            }
        }

        // Стандартный метод вывода.
        private void DefaultInput(ParamType1 pValue1, ParamType2 pValue2)
        {
            for (int i = 0; i < ActionArray.Length; i++)
            {
                ActionArray[i].Invoke(pValue1, pValue2);
            }
        }

        public void AddAction(global::System.Action<ParamType1, ParamType2> pAction, uint pPollSize, uint pTimeDelay, string pPollName)
        {
            if (StateInformationMain.__IsCreating)
            {
                ActionArray = Hellper.ExpendArray(ActionArray, new objects.action.Object<ParamType1, ParamType2>(pAction).ToInput);
            }
            else
                Exception(Ex.ActionValue.x10007, pAction.GetType().FullName);
        }
    }

    public class Object : main.Informing
    {
        public Object(main.IInforming pInformingMainObject, main.information.State pStateMainObject)
            : base("Action_2", pInformingMainObject)
        {

        }

        public void DefaultInput()
        {

        }
    }
}
