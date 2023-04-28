namespace Butterfly.system.objects.main.objects.description
{
    /// <summary>
    /// Описывает способ перенаправления данных <typeparamref name="ValueType"/>.
    /// </summary>
    /// <typeparam name="ValueType"></typeparam>
    public interface IRestream<ValueType>
    {
        /// <summary>
        /// Перенаправляет данные <typeparamref name="ValueType"/> в <paramref name="pFunc"/>.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public IRestream<OutputValueType> output_to<OutputValueType>(global::System.Func<ValueType, OutputValueType> pFunc,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");
        /// <summary>
        /// Перенаправляет/Дублирует данные <typeparamref name="ValueType"/> в <paramref name="pAction"/>.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public IRestream<ValueType> output_to(global::System.Action<ValueType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");
        /// <summary>
        /// Перенаправляет данные <typeparamref name="ValueType"/> в приватный обработчик <typeparamref name="PrivateHandlerType"/>.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public handler.description.IRestream output_to<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PrivateHandlerType : main.Object, IInput, IInput<ValueType>, handler.description.IRestream, handler.description.IRegisterInPoll, new();

        /// <summary>
        /// Перенаправляет данные <typeparamref name="ValueType"/> в публичный обработчик <typeparamref name="PublicHandlerType"/>.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public void output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IRestream, IInput, IInput<ValueType>, new();

        /// <summary>
        /// Перенаправляет данные <typeparamref name="ValueType"/> в публичный обработчик <typeparamref name="PublicHandlerType"/>.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public void output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IRestream, IInput, IInput<ValueType>, new();

        /// <summary>
        /// Передает данные <typeparamref name="ValueType"/> в глобальный обьект <typeparamref name="LocationEchoObjectType"/>, до первого узла.
        /// После обьект перейдет в режим ожидания ответа. В ответ придуд данные <typeparamref name="ValueType"/> в метод .awate .
        /// После чего цепочка событий продолжит свое выполнение.И после завершения продолжаться выполнятся остальные события.
        /// Используется в методе Construction(). 
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IRestream<ValueType> output_to_echo<LocationEchoObjectType>(
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where LocationEchoObjectType : main.Object, new();


        /// <summary>
        /// Передает данные <typeparamref name="ValueType"/> в глобальный обьект <typeparamref name="LocationEchoObjectType"/>, до первого узла.
        /// После обьект перейдет в режим ожидания ответа. В ответ придуд данные <typeparamref name="ReturnValueType"/> в метод .awate .
        /// После чего цепочка событий продолжит свое выполнение. И после завершения продолжаться выполнятся остальные события.
        /// Используется в методе Construction(). 
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IRestream<ReturnValueType> output_to_echo<LocationEchoObjectType, ReturnValueType>(
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where LocationEchoObjectType : main.Object, new();
    }
}
