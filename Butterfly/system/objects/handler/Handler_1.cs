namespace Butterfly.system.objects.handler
{
    public abstract class Object<InputType> : main.thread.Object,
       IInput<InputType>, IInput, description.IRegisterInPoll
    {
        private readonly manager.action.Object<InputType> InputActionManager;

        public Object()
        {
            InputActionManager = new manager.action.Object<InputType>(manager.events.events.Type.Creator, 
                StateInformation, this, this, this, this, this, this);
        }

        void IInput.ToInput<InputValueType>(InputValueType pValue)
        {
            if (pValue is InputType valueReduse)
            {
                InputActionManager.Action.Invoke(valueReduse);
            }
            else
                Exception(Ex.Handler.x10002, typeof(InputValueType).FullName, typeof(InputType).FullName);
        }

        void IInput<InputType>.ToInput(InputType pValue)
        {
            InputActionManager.Action.Invoke(pValue);
        }

        #region Poll

        void description.IRegisterInPoll.RegisterInPoll(int pSizePoll, int pTimeDelay, string pName)
        {
            InputActionManager.RegisterInPoll(pSizePoll, pTimeDelay, pName);
        }

        #endregion


        #region Input

        /// <summary>
        /// Используется в методе Construction(). 
        /// Перенаправляет(дублирует) входящие данные <typeparamref name="InputType"/> в System.Action(in <typeparamref name="InputType"/>).
        /// </summary>
        /// <param name="pAction"></param>
        /// <returns></returns>
        protected void input_to(global::System.Action<InputType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            InputActionManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Используется в методе Construction(). 
        /// Перенаправляет(дублирует) входящие данные <typeparamref name="InputType"/> 
        /// в System.Func(in <typeparamref name="InputType"/>, out <typeparamref name="OutputValueType"/>).
        /// </summary>
        /// <returns></returns>
        protected description.IRestream<OutputValueType> input_to<OutputValueType>(global::System.Func<InputType, OutputValueType> pFunc,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Используется в методе Construction().
        /// Создает приватный обработчик <typeparamref name="PrivateHandlerType"/> и перенаправляет(дублирует) 
        /// в него входящие данные <typeparamref name="InputType"/>.
        /// </summary>
        /// <typeparam name="PrivateHandlerType"></typeparam>
        /// <param name="pPoll"></param>
        /// <param name="pTimeDelay"></param>
        /// <returns></returns>
        protected description.IRestream input_to<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PrivateHandlerType : main.Object, IInput, IInput<InputType>, description.IRegisterInPoll, description.IRestream, new()
        {
            return InputActionManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Используется в методе Construction().
        /// Перенаправляет/Дублирует входящие данные <typeparamref name="InputType"/> в публичный обработчик.
        /// </summary>
        protected void input_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IInput, IInput<InputType>, description.IRestream, new()
        {
            InputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Используется в методе Construction().
        /// Перенаправляет/Дублирует входящие данные <typeparamref name="InputType"/> в публичный обработчик.
        /// </summary>
        protected void input_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PublicHandlerType : main.Object, IInput, IInput<InputType>, description.IRestream, new()
        {
            InputActionManager.AddAction(pPublicHandler.Invoke(pHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Доставляет input данные <typeparamref name="InputType"/> обработчика в global echo или в node echo <paramref name="pEchoName"/>
        /// после чего продолжается работа других событий.
        /// После приема данных <typeparamref name="InputType"/> с помощью метода .await текущая цепочка событий 
        /// продолжает свою работу асинхронно относительно других событий.
        /// </summary>
        /// <typeparam name="LocationEcho"></typeparam>
        /// <returns></returns>
        protected description.IAsyncRestream<InputType> async_input_to_echo(string pEchoName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return InputActionManager.AddConnectingToAsyncEcho<InputType, InputType>
                (pEchoName, pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Доставляет input данные <typeparamref name="InputType"/> обработчика в global echo или в node echo <typeparamref name="LocationEcho"/>
        /// после чего продолжается работа других событий.
        /// После приема данных <typeparamref name="InputType"/> с помощью метода .await текущая цепочка событий 
        /// продолжает свою работу асинхронно относительно других событий.
        /// </summary>
        /// <typeparam name="LocationEcho"></typeparam>
        /// <returns></returns>
        protected description.IAsyncRestream<InputType> async_input_to_echo<LocationEcho>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
                where LocationEcho : main.Object, new()
        {
            return InputActionManager.AddConnectingToAsyncEcho<InputType, InputType>
                (typeof(LocationEcho).FullName + typeof(InputType).FullName + typeof(InputType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }


        /// <summary>
        /// Доставляет input данные <typeparamref name="InputType"/> обработчика в global echo или в node echo <typeparamref name="LocationEcho"/>
        /// После чего реализуйтся прием данных <typeparamref name="InputType"/> с помощью метода .output_to
        /// и синхроно продолжает свою работу вместе с дургими событиями.
        /// Если данные которые нам нужно принять отличны от отправляемых, то укажите это.
        /// </summary>
        /// <typeparam name="LocationEcho"></typeparam>
        /// <returns></returns>
        protected description.IRestream<InputType> input_to_echo<LocationEcho>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
                where LocationEcho : main.Object, new()
        {
            return InputActionManager.AddConnectingToEcho<InputType, InputType>
                (typeof(LocationEcho).FullName + typeof(InputType).FullName + typeof(InputType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Доставляет input данные <typeparamref name="InputType"/> обработчика в global echo или в node echo <typeparamref name="LocationEcho"/>
        /// После чего реализуйтся прием данных <typeparamref name="ReturnValueType"/> с помощью метода .await .
        /// </summary>
        /// <returns></returns>
        protected description.IAsyncRestream<ReturnValueType> async_input_to_echo<LocationEcho, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
                where LocationEcho : main.Object, new()
        {
            return InputActionManager.AddConnectingToAsyncEcho<InputType, ReturnValueType>
                (typeof(LocationEcho).FullName + typeof(InputType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Доставляет input данные <typeparamref name="InputType"/> обработчика в global echo или в node echo <paramref name="pEchoName"/>
        /// После чего реализуйтся прием данных <typeparamref name="ReturnValueType"/> с помощью метода .await .
        /// </summary>
        /// <returns></returns>
        protected description.IAsyncRestream<ReturnValueType> async_input_to_echo<LocationEcho, ReturnValueType>(string pEchoName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            //return InputValueManager.AddAsyncEchoObject<ReturnValueType>(pEchoName, this, pPollSize, pTimeDelay);

            return default;
        }

        #endregion
    }
}
