namespace Butterfly.system.objects.handler.description
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
        /// Способ внешне указать путь вывода данных <typeparamref name="ParamValueType1"/>
        /// в System.Func(in <typeparamref name="ParamValueType1"/>, out <typeparamref name="ParamValueType2"/>).
        /// Требуется явно указать тип выводимых данных обработчика и резульата выполнения System.Func. 
        /// Используется в методе Construction().
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IRestream<ParamValueType2> output_to<ParamValueType1, ParamValueType2>(global::System.Func<ParamValueType1, ParamValueType2> pFunc,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");

        /// <summary>
        /// Используется в методе Construction().
        /// Способ внешне указать путь вывода данных в приватный обработчик <typeparamref name="PrivateHandlerType"/> с проверкой на соответсвие типа.
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IRestream output_to<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PrivateHandlerType : main.Object, IInput, IRestream, IRegisterInPoll, new();

        /// <summary>
        /// Используется в методе Construction().
        /// Способ внешне указать путь вывода данных в публичный обработчик <typeparamref name="PublicHandlerType"/> с проверкой на соответсвие типа.
        /// </summary>
        public void output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IInput, IRestream, IRegisterInPoll, new();

        /// <summary>
        /// Используется в методе Construction().
        /// Способ внешне указать путь вывода данных в публичный обработчик <typeparamref name="PublicHandlerType"/> с проверкой на соответсвие типа.
        /// </summary>
        public void output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IInput, IRestream, IRegisterInPoll, new();





        /// <summary>
        /// Ожидает ответа от global/node обьекта. После чего запутится цепочка событий и данные передадутся
        /// в публичный обработчик <typeparamref name="PublicHandlerType"/>.
        /// </summary>
        public void await<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IRestream, IInput, IRegisterInPoll, new();
        /// <summary>
        /// Ожидает ответа от global/node обьекта. После чего запутится цепочка событий и данные передадутся
        /// в публичный обработчик <typeparamref name="PublicHandlerType"/> c именем <paramref name="pPublicHandlerName"/>.
        /// </summary>
        public void await<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IRestream, IInput, IRegisterInPoll, new();
        /// <summary>
        /// Ожидает ответа от global/node обьекта. После чего запутится цепочка событий 
        /// и данные передадутся в System.Action[<typeparamref name="ParamValueType"/>].
        /// </summary>
        public IRestream await<ParamValueType>(global::System.Action<ParamValueType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");
        /// <summary>
        /// Ожидает ответа от global/node обьекта. После чего запутится цепочка событий 
        /// и данные передадутся в System.Func[in <typeparamref name="ReceiveValueType"/>, out <typeparamref name="ReturnValueType"/>].
        /// </summary>
        public IRestream<ReturnValueType> await<ReceiveValueType, ReturnValueType>(global::System.Func<ReceiveValueType, ReturnValueType> pFunc,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");
        /// <summary>
        /// Ожидает ответа от global/node обьекта. После чего запутится цепочка событий и данные передадутся в приватный обработчик
        /// <typeparamref name="PrivateHandlerType"/>.
        /// </summary>
        public IRestream await<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PrivateHandlerType : main.Object, IInput, IRestream, IRegisterInPoll, new();


        /// <summary>
        /// Передает данные <typeparamref name="ReceiveValueType"/> в глобальный обьект <typeparamref name="LocationEchoObjectType"/>, до первого узла.
        /// После продолжает выполнять все события в обьекте. В ответ придуд данные <typeparamref name="ReceiveValueType"/> в метод .awate .
        /// После чего цепочка событий продолжит свое выполнение.
        /// Используется в методе Construction(). 
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IAsyncRestream<ReceiveValueType> async_output_to_echo<LocationEchoObjectType, ReceiveValueType>(
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where LocationEchoObjectType : main.Object, new();

        /// <summary>
        /// Передает данные <typeparamref name="ReceiveValueType"/> в глобальный обьект <paramref name="pEchoName"/>, до первого узла.
        /// После продолжает выполнять все события в обьекте. В ответ придуд данные <typeparamref name="ReceiveValueType"/> в метод .awate .
        /// После чего цепочка событий продолжит свое выполнение.
        /// Используется в методе Construction(). 
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IAsyncRestream<ReceiveValueType> async_output_to_echo<ReceiveValueType>(
            string pEchoName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "");

        /// <summary>
        /// Передает данные <typeparamref name="ReceiveValueType"/> в глобальный обьект <typeparamref name="LocationEchoObjectType"/>, до первого узла.
        /// После продолжает выполнять все события в обьекте. В ответ придуд данные <typeparamref name="ReturnValueType"/> в метод .awate .
        /// После чего цепочка событий продолжит свое выполнение.
        /// Используется в методе Construction(). 
        /// Связь устанавливается через проверку на соответвие типа.
        /// </summary>
        public IAsyncRestream<ReceiveValueType> async_output_to_echo<LocationEchoObjectType, ReceiveValueType, ReturnValueType>(
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where LocationEchoObjectType : main.Object, new();

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
