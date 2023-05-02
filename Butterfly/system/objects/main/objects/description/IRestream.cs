namespace Butterfly.system.objects.main.objects.description
{
    public interface IRestream
    {
        /// <summary>
        /// Способ внешне указать путь вывода(дублирования) данных <typeparamref name="ParamValueType"/>
        /// в System.Action(in <typeparamref name="ParamValueType"/>).
        /// Требуется явно указать тип выводимых данных обработчика. 
        /// Используется в методе Construction(). 
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IRestream output_to<ParamValueType>(global::System.Action<ParamValueType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");

        /// <summary>
        /// Способ внешне указать путь вывода данных <typeparamref name="ParamValueType"/>
        /// в System.Func(in <typeparamref name="ParamValueType"/>, out <typeparamref name="ParamValueType"/>).
        /// Требуется явно указать тип выводимых данных обработчика и резульата выполнения System.Func. 
        /// Используется в методе Construction().
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IRestream<ReturnValueType> output_to<ParamValueType, ReturnValueType>(global::System.Func<ParamValueType, ReturnValueType> pFunc,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");

        /// <summary>
        /// Используется в методе Construction().
        /// Способ внешне указать путь вывода данных в приватный обработчик <typeparamref name="PrivateHandlerType"/> с проверкой на соответсвие типа.
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public handler.description.IRestream output_to<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PrivateHandlerType : main.Object, IInput, handler.description.IRestream, handler.description.IRegisterInPoll, 
            handler.description.IContinueInterrupting, new();

        /// <summary>
        /// Используется в методе Construction().
        /// Способ внешне указать путь вывода данных в публичный обработчик <typeparamref name="PublicHandlerType"/> с проверкой на соответсвие типа.
        /// </summary>
        public void output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IInput, IRestream, handler.description.IRegisterInPoll, new();

        /// <summary>
        /// Используется в методе Construction().
        /// Способ внешне указать путь вывода данных в публичный обработчик <typeparamref name="PublicHandlerType"/> с проверкой на соответсвие типа.
        /// </summary>
        public void output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IInput, IRestream, handler.description.IRegisterInPoll, new();
        

        /// <summary>
        /// Передает данные <typeparamref name="ReceiveValueType"/> в глобальный обьект <typeparamref name="LocationEchoObjectType"/>, до первого узла.
        /// После обьект перейдет в режим ожидания ответа. В ответ придуд данные <typeparamref name="ReceiveValueType"/> в метод .awate .
        /// После чего цепочка событий продолжит свое выполнение.И после завершения продолжаться выполнятся остальные события.
        /// Используется в методе Construction(). 
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IRestream<ReceiveValueType> output_to_echo<LocationEchoObjectType, ReceiveValueType>(
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where LocationEchoObjectType : main.Object, new();


        /// <summary>
        /// Передает данные <typeparamref name="ReceiveValueType"/> в глобальный обьект <typeparamref name="LocationEchoObjectType"/>, до первого узла.
        /// После обьект перейдет в режим ожидания ответа. В ответ придуд данные <typeparamref name="ReturnValueType"/> в метод .awate .
        /// После чего цепочка событий продолжит свое выполнение. И после завершения продолжаться выполнятся остальные события.
        /// Используется в методе Construction(). 
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IRestream<ReturnValueType> output_to_echo<LocationEchoObjectType, ReceiveValueType, ReturnValueType>(
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where LocationEchoObjectType : main.Object, new();
    }
}
