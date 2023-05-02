namespace Butterfly
{
    public interface ILocalObject
    {
        /// <summary>
        /// Реализует метод принятия данных из echo.
        /// </summary>
        /// <typeparam name="ValueType"></typeparam>
        public interface ReceiveEcho<ValueType>
        {
            /// <summary>
            /// Принимает данные <typeparamref name="ValueType"/>, 
            /// после чего с помощью <paramref name="pSendResult"/> используя .To(in <typeparamref name="ValueType"/>)
            /// отправте данные обратно.
            /// </summary>
            /// <param name="pValue"></param>
            /// <param name="pEcho"></param>
            public void ReceiveEcho(ValueType pValue, IEchoReturn<ValueType> pSendResult);
        }

        /// <summary>
        /// Реализует метод принятия данных из echo.
        /// </summary>
        /// <typeparam name="ValueType"></typeparam>
        public interface ReceiveEcho<ReceiveValueType, ReturnValueType>
        {
            /// <summary>
            /// Принимает данные <typeparamref name="ReceiveValueType"/>, 
            /// после чего с помощью <paramref name="send"/> используя .To(in <typeparamref name="ReturnValueType"/>)
            /// отправте данные обратно.
            /// </summary>
            public void ReceiveEcho(ReceiveValueType value, IEchoReturn<ReturnValueType> send);
        }

        /// <summary>
        /// Реализет метод Construction(), который будет привязан к жизненому циклу обькта в нутри которого находится.
        /// </summary>
        public interface Construction
        {
            /// <summary>
            /// Вызовется после настройки родительского обьекта.
            /// </summary>
            void Construction();
        }
        /// <summary>
        /// Реализет метод start(), который будет привязан к жизненому циклу обькта в нутри которого находится.
        /// </summary>
        public interface Start
        {
            /// <summary>
            /// Вызовется до запуска родительского обьекта.
            /// </summary>
            void Start();

            /// <summary>
            /// Реализет метод Start(), который будет привязан к жизненому циклу обькта в нутри которого находится.
            /// </summary>
            public interface Before
            {
                /// <summary>
                /// Вызовется до запуска родительского обьекта.
                /// </summary>
                void Start();
            }
            /// <summary>
            /// Реализет метод Start(), который будет привязан к жизненому циклу обькта в нутри которого находится.
            /// </summary>
            public interface After
            {
                /// <summary>
                /// Вызовется после запуска родительского обьекта.
                /// </summary>
                void Start();
            }
        }

        
        /// <summary>
        /// Реализет метод Stop(), который будет привязан к жизненому циклу обькта в нутри которого находится.
        /// </summary>
        public interface Stop
        {
            /// <summary>
            /// Вызовется до того как родительский обьект остановится.
            /// </summary>
            void Stop();

            /// <summary>
            /// Реализет метод Stop(), который будет привязан к жизненому циклу обькта в нутри которого находится.
            /// </summary>
            public interface Before
            {
                /// <summary>
                /// Вызовется до того как родительский обьект остановится.
                /// </summary>
                void Stop();
            }
            /// <summary>
            /// Реализет метод Stop(), который будет привязан к жизненому циклу обькта в нутри которого находится.
            /// </summary>
            public interface After
            {
                /// <summary>
                /// Вызовется после того как родительский обьект остановится.
                /// </summary>
                void Stop();
            }
        }

        /// <summary>
        /// Реализет метод Configurate(), который будет привязан к жизненому циклу обькта в нутри которого находится.
        /// </summary>
        public interface Configurate
        {
            /// <summary>
            /// Данный метод вызовется до того как пройдет настройка в нутри родительского обьекта.
            /// </summary>
            void Configurate();

            /// <summary>
            /// Реализет метод Configurate(), который будет привязан к жизненому циклу обькта в нутри которого находится.
            /// </summary>
            public interface Before
            {
                /// <summary>
                /// Данный метод вызовется до того как пройдет настройка в нутри родительского обьекта.
                /// </summary>
                void Configurate();
            }

            /// <summary>
            /// Реализет метод Configurate(), который будет привязан к жизненому циклу обькта в нутри которого находится.
            /// </summary>
            public interface After
            {
                /// <summary>
                /// Данный метод вызовется после того как пройдет настройка в нутри родительского обьекта.
                /// </summary>
                void Configurate();
            }
        }
    }
}
