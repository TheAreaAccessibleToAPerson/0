namespace Butterfly.system
{
    public struct GlobalData
    {
        public const string SYSTEM_NAMESPACE = "Butterfly";

        public const string SYSTEM_HEADER_INFORMATION = "Butterfly alpha version";


        public const string CONTROLLER_NAMESPACE = "";


        /// <summary>
        /// Рассылка сообщений всем подписаным в данный момент обьектам.
        /// Данные хранятся в многомерных массивах, а ранг расширяется автоматически.
        /// Стандартное значение для размера массива.
        /// </summary>
        public const int DEFAULT_VALUE_SENDING_MESSAGE_ARRAY_SIZE = 8192;


        /// <summary>
        /// Обьекты в которых нужно остановить работу потоков с помощью Task.
        /// </summary>
        public const string TASK_STOPPING_THREAD = "TASK_STOPPING_THREAD";

        /// <summary>
        /// Если поток залипает при выполнении проинформировать об этом через...
        /// </summary>
        public const int THREAD_STACK = 600000;

        public const int SYSTEM_ACTION_INVOKE_TIME_DELAY = 50;

        /// <summary>
        /// Имя основного системного обьекта.
        /// </summary>
        public const string SYSTEM_OBJECT_NAME = "SystemObject";

        /// <summary>
        /// Размер максимальный размер эластичтого пула.
        /// </summary>

        public const int ELASTIC_POLL_SIZE = 1000000;


        public const int CHECK_POLL_OBJECTS_TIME_DELAY = 10000;
    }
}
