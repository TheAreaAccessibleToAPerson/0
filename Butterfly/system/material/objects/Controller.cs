namespace Butterfly
{
    /// <summary>
    /// Обьект контроллер. 
    /// </summary>
    public abstract class Controller : system.objects.main.Object
    {
        /// <summary>
        /// Указывает что этот обьект является главным узлом, в случае приостановки работы
        /// все процессы системы завершат свою работу до этого обьекта.
        /// </summary>
        public abstract class Independent : Controller
        { }

        /// <summary>
        /// Реализует возможноть указать локальное значение при создании обьекта.
        /// </summary>
        public sealed class Local
        {
            /// <summary>
            /// Добовляет возможность при создании обьекта указать ему локальное значение, которое будет доступно через поле localValue.
            /// </summary>
            /// <typeparam name="LocalValueType"></typeparam>
            public abstract class value<LocalValueType> : system.objects.main.local.value.Object.Value<LocalValueType>
            {
                /// <summary>
                /// Указывает что этот обьект является главным узлом, в случае приостановки работы
                /// все процессы системы завершат свою работу до этого обьекта.
                /// </summary>
                public abstract class Independent : value<LocalValueType>
                {}
            }
        }

        /// <summary>
        /// Добовляет возможноть использования в системном методe start() метод add_thread(...);
        /// </summary>
        public abstract class Thread : system.objects.main.thread.Object
        {
            /// <summary>
            /// Реализует возможноть указать локальное значение при создании обьекта.
            /// </summary>
            public sealed class Local
            {
                /// <summary>
                /// Добовляет возможность при создании обьекта указать ему локальное значение, которое будет доступно через поле localValue.
                /// </summary>
                /// <typeparam name="LocalValueType"></typeparam>
                public abstract class value<LocalValueType> : system.objects.main.local.value.thread.Object.Value<LocalValueType>
                {
                    /// <summary>
                    /// Указывает что этот обьект является главным узлом, в случае приостановки работы
                    /// все процессы системы завершат свою работу до этого обьекта.
                    /// </summary>
                    public abstract class Independent : value<LocalValueType>
                    {}
                }
            }

            /// <summary>
            /// Указывает что этот обьект является главным узлом, в случае приостановки работы
            /// все процессы системы завершат свою работу до этого обьекта.
            /// </summary>
            public abstract class Independent : Thread
            {
            }
        }
    }

    /// <summary>
    /// Динамический контроллер. Является главным узлом. В методе Contruction() можно создать его ветки, в их методе Contruction() можно создать 
    /// их ветки(ветки для веток), для текущего контроллера эта уже будут ответвления. Жизнь всех веток и ответвлений будут неразрывно связаны с текущем контроллером.
    /// В методе Start() можно создать отложеный вызов(deferred_create_object()) другого контроллера не являющегося Independent
    /// чья жизнь будет также связана.
    /// В отличии от обычного Controller даже в случае выставления с помощью метода destroy() этот обьект не прекратит создание дочерних Controller обьектов
    /// и приступит к уничтожению всех созданых улов только после того как все дочерние обьекты прекратят свою работу.
    /// Используется если в текущем контроллере реализуется жизнено необходимая логика для правильной работы дочерних обьектов.
    /// Дочерние обьекты могут еще не знать о том что они скора уничтожется, поэтому могут порождать другие котроллеры чья логика напрямую зависит
    /// от созданых дочерних констроллеров сдесь. 
    /// </summary>
    public abstract class DynamicController : system.objects.main.Object
    {
        /// <summary>
        /// Указывает что этот обьект является главным узлом, в случае приостановки работы
        /// все процессы системы завершат свою работу до этого обьекта.
        /// </summary>
        public abstract class Independent : DynamicController
        { }

        /// <summary>
        /// Реализует возможноть указать локальное значение при создании обьекта.
        /// </summary>
        public sealed class Local
        {
            /// <summary>
            /// Добовляет возможность при создании обьекта указать ему локальное значение, которое будет доступно через поле localValue.
            /// </summary>
            /// <typeparam name="LocalValueType"></typeparam>
            public abstract class value<LocalValueType> : system.objects.main.local.value.Object.Value<LocalValueType>
            {
                /// <summary>
                /// Указывает что этот обьект является главным узлом, в случае приостановки работы
                /// все процессы системы завершат свою работу до этого обьекта.
                /// </summary>
                public abstract class Independent : value<LocalValueType>
                { }
            }
        }

        /// <summary>
        /// Добовляет возможноть использования в системном методe start() метод add_thread(...);
        /// </summary>
        public abstract class Thread : system.objects.main.thread.Object
        {
            /// <summary>
            /// Реализует возможноть указать локальное значение при создании обьекта.
            /// </summary>
            public sealed class Local
            {
                /// <summary>
                /// Добовляет возможность при создании обьекта указать ему локальное значение, которое будет доступно через поле localValue.
                /// </summary>
                /// <typeparam name="LocalValueType"></typeparam>
                public abstract class value<LocalValueType> : system.objects.main.local.value.thread.Object.Value<LocalValueType>
                {
                    /// <summary>
                    /// Указывает что этот обьект является главным узлом, в случае приостановки работы
                    /// все процессы системы завершат свою работу до этого обьекта.
                    /// </summary>
                    public abstract class Independent : value<LocalValueType>
                    { }
                }
            }

            /// <summary>
            /// Указывает что этот обьект является главным узлом, в случае приостановки работы
            /// все процессы системы завершат свою работу до этого обьекта.
            /// </summary>
            public abstract class Independent : Thread
            {
            }
        }
    }
}
