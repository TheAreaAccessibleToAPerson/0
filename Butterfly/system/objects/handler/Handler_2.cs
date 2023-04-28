namespace Butterfly.system.objects.handler
{
    public abstract class Object<InputType, OutputType> : Object<InputType>,
        IInput<InputType>, IInput, description.IRestream<OutputType>, description.IRestream
    {
        private readonly manager.action.Object<OutputType> OutputActionManager;

        public Object()
        {
            OutputActionManager = new manager.action.Object<OutputType>(manager.events.events.Type.Broker, StateInformation, 
                this, this, this, this, this, this);
        }

        protected void output(OutputType pValue)
        {
            OutputActionManager.Action.Invoke(pValue);
        }

        /// <summary>
        /// Доставляет output данные <typeparamref name="OutputType"/> обработчика в global echo или в node echo <paramref name="pEchoName"/>
        /// после чего продолжается работа других событий.
        /// После приема данных <typeparamref name="OutputType"/> с помощью метода .await текущая цепочка событий 
        /// продолжает свою работу асинхронно относительно других событий.
        /// </summary>
        /// <typeparam name="LocationEcho"></typeparam>
        /// <returns></returns>
        protected description.IAsyncRestream<ReturnValueType> async_output_to_echo<LocationEcho, ReturnValueType>
            (string pEchoName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<InputType, ReturnValueType>
                (typeof(LocationEcho).FullName + typeof(InputType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Доставляет output данные <typeparamref name="OutputType"/> обработчика в global echo или в node echo <typeparamref name="LocationEcho"/>
        /// после чего продолжается работа других событий.
        /// После приема данных <typeparamref name="OutputType"/> с помощью метода .await текущая цепочка событий 
        /// продолжает свою работу асинхронно относительно других событий.
        /// </summary>
        /// <typeparam name="LocationEcho"></typeparam>
        /// <returns></returns>
        protected description.IAsyncRestream<OutputType> async_output_to_echo<LocationEcho>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
                where LocationEcho : class
        {
            return OutputActionManager.AddConnectingToAsyncEcho<OutputType, OutputType>
                (typeof(LocationEcho).FullName + typeof(OutputType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        /// <summary>
        /// Используется в методе Construction().
        /// Перенапрвляет(дублирует) выходные данные <typeparamref name="OutputType"/> 
        /// в System.Action(in <typeparamref name="OutputType"/>).
        /// </summary>
        protected Object<InputType, OutputType> output_to(global::System.Action<OutputType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }
        /// <summary>
        /// Используется в методе Construction().
        /// Перенаправляет(дублирует) выходные данные <typeparamref name="OutputType"/> 
        /// в System.Func(in <typeparamref name="OutputType"/>, out <typeparamref name="OutputValueType"/>).
        /// </summary>
        protected description.IRestream<OutputValueType> output_to<OutputValueType>
            (global::System.Func<OutputType, OutputValueType> pFunc,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFunc(pFunc, this, pPollSize, pTimeDelay, pPollName);
        }
        /// <summary>
        /// Используется в методе Construction().
        /// Перенапрвляет(дублирует) выходные данные <typeparamref name="OutputType"/> 
        /// в <typeparamref name="PrivateHandlerType"/>
        /// </summary>
        protected description.IRestream output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
            where PrivateHandlerType : main.Object, IInput<OutputType>, IInput, description.IRestream, 
                description.IRegisterInPoll, new()
        {
            return OutputActionManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream<OutputValueType> description.IRestream<OutputType>.output_to<OutputValueType>
            (global::System.Func<OutputType, OutputValueType> pFunc,int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFunc(pFunc, pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream<OutputType> description.IRestream<OutputType>.output_to(global::System.Action<OutputType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }
        description.IRestream description.IRestream<OutputType>.output_to<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddPrivateHandler<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
        void description.IRestream<OutputType>.output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }
        void description.IRestream<OutputType>.output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream description.IRestream.output_to<InputValueType>(global::System.Action<InputValueType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddActionIsType(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }
        description.IRestream<OutputValueType> description.IRestream.output_to<InputValueType, OutputValueType>(global::System.Func<InputValueType, OutputValueType> pFunc,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFuncIsType(pFunc, pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream description.IRestream.output_to<PrivateHandlerType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddPrivateHandlerIsType<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
        void description.IRestream.output_to<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }
        void description.IRestream.output_to<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }

        void description.IRestream.await<PublicHandlerType>(global::System.Func<PublicHandlerType> pPublicHandler,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke().ToInput, pPollSize, pTimeDelay, pPollName);
        }
        void description.IRestream.await<PublicHandlerType>(global::System.Func<string, PublicHandlerType> pPublicHandler, string pPublicHandlerName,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddAction(pPublicHandler.Invoke(pPublicHandlerName).ToInput, pPollSize, pTimeDelay, pPollName);
        }

        description.IAsyncRestream<ReceiveValueType> description.IRestream.async_output_to_echo<LocationEchoObjectType, ReceiveValueType>(
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<ReceiveValueType, ReceiveValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReceiveValueType).FullName,
                        pPollSize, pTimeDelay, pPollName);
        }

        description.IRestream description.IRestream.await<ParamValueType>(global::System.Action<ParamValueType> pAction,
            int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            OutputActionManager.AddActionIsType(pAction, pPollSize, pTimeDelay, pPollName);

            return this;
        }

        description.IRestream<ReturnValueType> description.IRestream.await<ReceiveValueType, ReturnValueType>
            (global::System.Func<ReceiveValueType, ReturnValueType> pFunc, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddFuncIsType(pFunc, pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream description.IRestream.await<PrivateHandlerType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddPrivateHandlerIsType<PrivateHandlerType>(pPollSize, pTimeDelay, pPollName);
        }
        description.IAsyncRestream<ReceiveValueType> description.IRestream.async_output_to_echo
            <LocationEchoObjectType, ReceiveValueType, ReturnValueType> (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<ReceiveValueType, ReceiveValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }
        description.IAsyncRestream<ReceiveValueType> description.IRestream.async_output_to_echo<ReceiveValueType>
            (string pEchoName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToAsyncEcho<ReceiveValueType, ReceiveValueType>
                    (pEchoName,
                    pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream<ReceiveValueType> description.IRestream.output_to_echo<LocationEchoObjectType, ReceiveValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<ReceiveValueType, ReceiveValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReceiveValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }
        description.IRestream<ReturnValueType> description.IRestream.output_to_echo<LocationEchoObjectType, ReceiveValueType, ReturnValueType>
            (int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            return OutputActionManager.AddConnectingToEcho<ReceiveValueType, ReturnValueType>
                    (typeof(LocationEchoObjectType).FullName + typeof(ReceiveValueType).FullName + typeof(ReturnValueType).FullName,
                    pPollSize, pTimeDelay, pPollName);
        }

        description.IAsyncRestream<OutputType> description.IRestream<OutputType>.async_output_to_echo<LocationEchoObjectType>(int pPollSize, int pTimeDelay, string pPollName)
        {
            throw new System.NotImplementedException();
        }

        public description.IAsyncRestream<OutputType> async_output_to_echo(string pEchoName, int pPollSize = 0, int pTimeDelay = 0, string pPollName = "")
        {
            throw new System.NotImplementedException();
        }

        public description.IAsyncRestream<ReturnValueType> async_output_to_echo<LocationEchoObjectType, ReturnValueType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : main.Object, new()
        {
            throw new System.NotImplementedException();
        }

        public description.IRestream<OutputType> output_to_echo<LocationEchoObjectType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : main.Object, new()
        {
            throw new System.NotImplementedException();
        }

        public description.IRestream<ReturnValueType> output_to_echo<LocationEchoObjectType, ReturnValueType>(int pPollSize = 0, int pTimeDelay = 0, string pPollName = "") where LocationEchoObjectType : main.Object, new()
        {
            throw new System.NotImplementedException();
        }
    }
}
