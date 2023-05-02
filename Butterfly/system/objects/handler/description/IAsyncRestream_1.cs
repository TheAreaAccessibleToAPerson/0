namespace Butterfly.system.objects.handler.description
{
    /// <summary>
    /// Описывает место приема ответа.
    /// </summary>
    /// <typeparam name="ValueType"></typeparam>
    public interface IAsyncRestream<ValueType>
    {
        /// <summary>
        /// Ожидает доставки данных <typeparamref name="ValueType"/>. После чего продолжает выполнение цепочки задачь.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public IRestream<ValueType> await(global::System.Action<ValueType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");

        /// <summary>
        /// Ожидает доставки данных <typeparamref name="ValueType"/>. После чего продолжает выполнение цепочки задачь.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public IRestream<OutputValueType> await<OutputValueType>(global::System.Func<ValueType, OutputValueType> pFunc,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");

        /// <summary>
        /// Ожидает доставки данных <typeparamref name="ValueType"/>. После чего продолжает выполнение цепочки задачь.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public IRestream await<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PrivateHandlerType : main.Object, IRestream, IInput, IInput<ValueType>, IRegisterInPoll, handler.description.IContinueInterrupting, new();

        /// <summary>
        /// Перенаправляет данные <typeparamref name="ValueType"/> в публичный обработчик <typeparamref name="PublicHandlerType"/>.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public void await<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IInput, IInput<ValueType>, IRestream, new();

        /// <summary>
        /// Перенаправляет данные <typeparamref name="ValueType"/> в публичный обработчик <typeparamref name="PublicHandlerType"/>.
        /// Используется только в методе Construction().
        /// </summary>
        /// <returns></returns>
        public void await<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublic,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IRestream, IInput, IInput<ValueType>, new();
    }
}
