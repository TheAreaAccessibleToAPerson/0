namespace Butterfly.system.objects.handler.manager.events
{
    namespace events
    {
        public enum Type
        {
            Creator = 1,
            Broker = 2
        }
    }

    public class Object<ParamType>
    {
        public struct Type
        {
            // Если не нужно обрывать выполнение ActionArray.
            public const string Continue = "Continue";
            // Если нужно оборвать выполнение всех событытий(ActionArray).
            // Незабудте делигировать продолжение выполнения ActionArray обьекту
            // который это сделал.
            public const string Break = "Break";
        }

        private global::System.Action<ParamType>[] ActionArray = new System.Action<ParamType>[0];
        private string[] ActionTypeArray = new string[0];
        //public int ActionCount { private set; get; } = 0;

        // Необходимо для синхроной работы обработчика.
        // Данные значения передаются начиная с из input_to обработчика.
        public readonly global::System.Action<int> ContinueExecutingEvents;
        public int NumberOfTheInterruptedEvent { private set; get; }

        /// <summary>
        /// Тип создателя. 
        /// Редиректы должны сохранить возможность продолжить синхроную работу обработчика.
        /// </summary>
        private readonly events.Type CreatorType;

        public Object(events.Type pCreatorType) 
        {
            CreatorType = pCreatorType;

            ContinueExecutingEvents = Run;
            NumberOfTheInterruptedEvent = 0;
        }

        public Object(events.Type pCreatorType, global::System.Action<int> pContinueExecutingEvents, int pNumberOfTheInterruptedEvent)
        {
            CreatorType = pCreatorType;

            ContinueExecutingEvents = pContinueExecutingEvents;
            NumberOfTheInterruptedEvent = pNumberOfTheInterruptedEvent;
        }

        /// <summary>
        /// Если мы прервали череду событий, то задача их возобнавления ложится обьект который это сделал.
        /// В момент прерывания нужно записать значение передоваемого параметра. Что бы в момент
        /// возобновления событий они смогли его получить.
        /// </summary>
        private System.Collections.Generic.Queue<ParamType> QueueParamValue = new System.Collections.Generic.Queue<ParamType>();

        public void Add(string pTransitionType, global::System.Action<ParamType> pAction)
        {
            ActionTypeArray = Hellper.ExpendArray(ActionTypeArray, pTransitionType);
            ActionArray = Hellper.ExpendArray(ActionArray, pAction);

            if (CreatorType.HasFlag(events.Type.Creator)) NumberOfTheInterruptedEvent++;
        }

        public void Run(ParamType pValue)
        {
            for (int i = 0; i < ActionArray.Length; i++)
            {
                if (ActionTypeArray[i] == Type.Break)
                {
                    QueueParamValue.Enqueue(pValue);

                    ActionArray[i].Invoke(pValue);

                    return;
                }

                ActionArray[i].Invoke(pValue);
            }
        }

        private void Run(int pStartIndex)
        {
            if ((++pStartIndex) >= NumberOfTheInterruptedEvent) return;

            ParamType param = QueueParamValue.Dequeue();

            for (int i = pStartIndex; i < NumberOfTheInterruptedEvent; i++)
            {
                if (ActionTypeArray[i] == Type.Break)
                {
                    QueueParamValue.Enqueue(param);

                    ActionArray[i].Invoke(param);

                    return;
                }

                ActionArray[i].Invoke(param);
            }
        }
    }
}
